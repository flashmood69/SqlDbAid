using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDbAid
{
    public static class SqlFormatter
    {
        public enum KeywordCaseStyle
        {
            Upper,
            Lower,
            Preserve
        }

        public enum IdentifierQuoteStyle
        {
            None,
            DoubleQuotes,
            SquareBrackets
        }

        public enum CommaPosition
        {
            After,
            Before,
            BeforeNoSpace
        }

        public enum OnPosition
        {
            Before,
            After
        }

        public enum TablePosition
        {
            SameLine,
            NextLine
        }

        /// <summary>Options corresponding to the CleanSQL formatting controls.</summary>
        public sealed class Options
        {
            public KeywordCaseStyle KeywordCase { get; set; } = KeywordCaseStyle.Upper;
            public IdentifierQuoteStyle IdentifierQuoting { get; set; } = IdentifierQuoteStyle.None;
            public bool UseTabs { get; set; } = true;
            public int IndentSize { get; set; } = 4;
            public bool LogicalOperatorNewlineBefore { get; set; } = true;
            public CommaPosition CommaPosition { get; set; } = CommaPosition.BeforeNoSpace;
            public OnPosition OnPosition { get; set; } = OnPosition.After;
            public TablePosition TablePosition { get; set; } = TablePosition.NextLine;
            public int JoinIndentation { get; set; }
            public bool IndentJoinConditions { get; set; } = true;
            /// <summary>Break IN/VALUES element lists onto separate lines and indent CASE inside parentheses.</summary>
            public bool IndentLists { get; set; } = true;
            public int LinesBetweenQueries { get; set; } = 2;
        }

        private enum TokenKind
        {
            Word,
            Number,
            Literal,
            QuotedIdentifier,
            Comment,
            Symbol,
            BatchSeparator
        }

        private enum ParenKind
        {
            Regular,
            Subquery,
            List
        }

        private sealed class Token
        {
            public Token(string text, TokenKind kind)
            {
                Text = text;
                Kind = kind;
            }

            public string Text { get; }
            public TokenKind Kind { get; }
        }

        private static readonly string[] JoinPrefixes =
        {
            "INNER", "LEFT", "RIGHT", "FULL", "CROSS", "NATURAL"
        };

        /// <summary>Formats SQL using the supplied options or CleanSQL-compatible defaults.</summary>
        public static string Format(string sql, Options options = null)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));

            options = options ?? new Options();
            ValidateOptions(options);

            var tokens = Tokenize(sql);
            var writer = new SqlWriter(options);
            FormatTokens(tokens, options, writer);
            return writer.Finish();
        }

        private static void ValidateOptions(Options options)
        {
            if (!Enum.IsDefined(typeof(KeywordCaseStyle), options.KeywordCase))
                throw new ArgumentOutOfRangeException(nameof(options.KeywordCase));
            if (!Enum.IsDefined(typeof(IdentifierQuoteStyle), options.IdentifierQuoting))
                throw new ArgumentOutOfRangeException(nameof(options.IdentifierQuoting));
            if (!Enum.IsDefined(typeof(CommaPosition), options.CommaPosition))
                throw new ArgumentOutOfRangeException(nameof(options.CommaPosition));
            if (!Enum.IsDefined(typeof(OnPosition), options.OnPosition))
                throw new ArgumentOutOfRangeException(nameof(options.OnPosition));
            if (!Enum.IsDefined(typeof(TablePosition), options.TablePosition))
                throw new ArgumentOutOfRangeException(nameof(options.TablePosition));
            if (options.IndentSize < 1 || options.IndentSize > 16)
                throw new ArgumentOutOfRangeException(nameof(options.IndentSize), "Indent size must be between 1 and 16.");
            if (options.JoinIndentation < 0 || options.JoinIndentation > 2)
                throw new ArgumentOutOfRangeException(nameof(options.JoinIndentation), "Join indentation must be 0, 1, or 2.");
            if (options.LinesBetweenQueries < 0 || options.LinesBetweenQueries > 10)
                throw new ArgumentOutOfRangeException(nameof(options.LinesBetweenQueries));
        }

        private static void FormatTokens(List<Token> tokens, Options options, SqlWriter writer)
        {
            var parenKinds = new Stack<ParenKind>();
            var outerStatementIndents = new Stack<int>();
            var outerClauses = new Stack<string>();
            var outerLastWords = new Stack<string>();
            var outerBetweenPending = new Stack<bool>();
            int statementIndent = 0;
            string clause = "";
            string lastSignificantWord = "";
            bool betweenPending = false;
            var caseIndents = new Stack<int>();
            var relocatedCommaIndexes = new HashSet<int>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (relocatedCommaIndexes.Contains(i))
                    continue;

                Token token = tokens[i];
                string upper = token.Kind == TokenKind.Word ? token.Text.ToUpperInvariant() : "";

                if (token.Kind == TokenKind.BatchSeparator)
                {
                    // GO ends the batch: keep it on its own line directly below the batch.
                    writer.RemoveTrailingBlankLines();
                    writer.NewLine(0);
                    writer.Append(new Token(
                        ApplyCase(token.Text.Substring(0, 2), options.KeywordCase) + token.Text.Substring(2),
                        TokenKind.BatchSeparator));
                    writer.NewLine(0);
                    if (i < tokens.Count - 1)
                        writer.AddBlankLines(options.LinesBetweenQueries);

                    parenKinds.Clear();
                    outerStatementIndents.Clear();
                    outerClauses.Clear();
                    outerLastWords.Clear();
                    outerBetweenPending.Clear();
                    caseIndents.Clear();
                    statementIndent = 0;
                    clause = "";
                    lastSignificantWord = "";
                    betweenPending = false;
                    continue;
                }

                if (token.Kind == TokenKind.Comment)
                {
                    if (token.Text.StartsWith("--", StringComparison.Ordinal) &&
                        IsClauseContext(parenKinds) && options.CommaPosition == CommaPosition.After &&
                        IsCommaListClause(clause))
                    {
                        int commaIndex = i + 1;
                        while (commaIndex < tokens.Count &&
                            tokens[commaIndex].Kind == TokenKind.Comment &&
                            tokens[commaIndex].Text.StartsWith("--", StringComparison.Ordinal))
                        {
                            commaIndex++;
                        }

                        if (commaIndex < tokens.Count &&
                            tokens[commaIndex].Kind == TokenKind.Symbol &&
                            tokens[commaIndex].Text == "," &&
                            relocatedCommaIndexes.Add(commaIndex))
                        {
                            writer.Append(tokens[commaIndex]);
                        }
                    }

                    writer.Append(token);
                    if (token.Text.StartsWith("--", StringComparison.Ordinal))
                        writer.NewLine(writer.CurrentIndentLevel);
                    continue;
                }

                if (token.Kind == TokenKind.Symbol && token.Text == "(")
                {
                    writer.Append(token);
                    bool subquery = IsSubqueryStart(tokens, i);
                    bool list = !subquery && options.IndentLists && IsListStart(tokens, i, clause);
                    if (subquery || list)
                    {
                        parenKinds.Push(subquery ? ParenKind.Subquery : ParenKind.List);
                        outerStatementIndents.Push(statementIndent);
                        outerClauses.Push(clause);
                        outerLastWords.Push(lastSignificantWord);
                        outerBetweenPending.Push(betweenPending);
                        statementIndent = writer.CurrentIndentLevel + 1;
                        clause = "";
                        lastSignificantWord = "";
                        betweenPending = false;
                        writer.NewLine(statementIndent);
                    }
                    else
                    {
                        parenKinds.Push(ParenKind.Regular);
                    }
                    continue;
                }

                if (token.Kind == TokenKind.Symbol && token.Text == ")")
                {
                    ParenKind kind = parenKinds.Count > 0 ? parenKinds.Pop() : ParenKind.Regular;
                    if (kind != ParenKind.Regular)
                    {
                        writer.NewLine(Math.Max(0, statementIndent - 1));
                        if (outerStatementIndents.Count > 0)
                            statementIndent = outerStatementIndents.Pop();
                        if (outerClauses.Count > 0)
                            clause = outerClauses.Pop();
                        if (outerLastWords.Count > 0)
                            lastSignificantWord = outerLastWords.Pop();
                        if (outerBetweenPending.Count > 0)
                            betweenPending = outerBetweenPending.Pop();
                    }
                    writer.Append(token);
                    continue;
                }

                if (IsClauseContext(parenKinds) && token.Kind == TokenKind.Symbol && token.Text == ";")
                {
                    writer.Append(token);
                    writer.NewLine(statementIndent);
                    clause = "";
                    lastSignificantWord = "";
                    if (i < tokens.Count - 1)
                        writer.AddBlankLines(options.LinesBetweenQueries);
                    continue;
                }

                if (token.Kind == TokenKind.Word && upper == "CASE" &&
                    (IsClauseContext(parenKinds) || options.IndentLists))
                {
                    int caseIndent = writer.CurrentIndentLevel;
                    if (writer.HasCurrentLineContent)
                    {
                        caseIndent++;
                        writer.NewLine(caseIndent);
                    }
                    writer.AppendWord(token.Text, options);
                    caseIndents.Push(caseIndent);
                    writer.NewLine(caseIndent + 1);
                    lastSignificantWord = upper;
                    continue;
                }

                if (token.Kind == TokenKind.Word && caseIndents.Count > 0 && (upper == "WHEN" || upper == "ELSE"))
                {
                    writer.NewLine(caseIndents.Peek() + 1);
                    writer.AppendWord(token.Text, options);
                    lastSignificantWord = upper;
                    continue;
                }

                if (token.Kind == TokenKind.Word && upper == "END" && caseIndents.Count > 0)
                {
                    int caseIndent = caseIndents.Pop();
                    writer.NewLine(caseIndent);
                    writer.AppendWord(token.Text, options);
                    lastSignificantWord = upper;
                    continue;
                }

                if (IsClauseContext(parenKinds) && token.Kind == TokenKind.Word)
                {
                    if (TryReadJoin(tokens, i, out string joinText, out int joinLength))
                    {
                        writer.NewLine(statementIndent + options.JoinIndentation);
                        writer.AppendWord(joinText, options);
                        clause = "FROM";
                        lastSignificantWord = "JOIN";
                        i += joinLength - 1;
                        if (options.TablePosition == TablePosition.NextLine)
                            writer.NewLine(statementIndent + options.JoinIndentation + 1);
                        else
                            writer.AppendSpace();
                        continue;
                    }

                    if (upper == "ON" && clause == "FROM")
                    {
                        int onIndent = statementIndent + options.JoinIndentation + (options.IndentJoinConditions ? 1 : 0);
                        if (options.OnPosition == OnPosition.Before)
                        {
                            writer.NewLine(onIndent);
                            writer.AppendWord(token.Text, options);
                            writer.AppendSpace();
                        }
                        else
                        {
                            writer.AppendWord(token.Text, options);
                            writer.NewLine(onIndent + 1);
                        }
                        lastSignificantWord = "ON";
                        continue;
                    }

                    if (TryReadClause(tokens, i, out string clauseText, out string clauseName, out int clauseLength))
                    {
                        writer.NewLine(statementIndent);
                        writer.AppendWord(
                            options.KeywordCase == KeywordCaseStyle.Preserve
                                ? ReadPhrase(tokens, i, clauseLength)
                                : clauseText,
                            options);
                        clause = clauseName;
                        lastSignificantWord = clauseName;

                        if (clauseName == "WITH")
                        {
                            writer.AppendSpace();
                        }
                        else if (clauseName == "FROM" || clauseName == "INSERT" || clauseName == "UPDATE")
                        {
                            if (options.TablePosition == TablePosition.NextLine)
                                writer.NewLine(statementIndent + 1);
                            else
                                writer.AppendSpace();
                        }
                        else
                        {
                            writer.NewLine(statementIndent + 1);
                        }

                        i += clauseLength - 1;
                        continue;
                    }

                    if (upper == "AND" && betweenPending && IsConditionClause(clause) && IsClauseContext(parenKinds))
                    {
                        writer.AppendWord(token.Text, options);
                        betweenPending = false;
                        lastSignificantWord = upper;
                        continue;
                    }

                    if ((upper == "AND" || upper == "OR" || upper == "XOR") &&
                        IsConditionClause(clause) && IsClauseContext(parenKinds))
                    {
                        int conditionIndent = statementIndent + (clause == "FROM"
                            ? options.JoinIndentation + 1 + (options.IndentJoinConditions ? 1 : 0)
                            : 1);

                        if (options.LogicalOperatorNewlineBefore)
                            writer.NewLine(conditionIndent);

                        writer.AppendWord(token.Text, options);

                        if (!options.LogicalOperatorNewlineBefore)
                            writer.NewLine(conditionIndent);

                        lastSignificantWord = upper;
                        continue;
                    }

                    if (upper == "FROM")
                    {
                        writer.NewLine(statementIndent);
                        writer.AppendWord(token.Text, options);
                        clause = "FROM";
                        lastSignificantWord = upper;
                        if (options.TablePosition == TablePosition.NextLine)
                            writer.NewLine(statementIndent + 1);
                        else
                            writer.AppendSpace();
                        continue;
                    }
                }

                bool listComma = token.Kind == TokenKind.Symbol && token.Text == "," && IsListContext(parenKinds);
                if (listComma ||
                    (IsClauseContext(parenKinds) && token.Kind == TokenKind.Symbol && token.Text == "," &&
                    IsCommaListClause(clause)))
                {
                    int commaIndent = listComma ? statementIndent : statementIndent + 1;

                    if (options.CommaPosition == CommaPosition.After)
                    {
                        writer.Append(token);
                        if (i + 1 < tokens.Count &&
                            tokens[i + 1].Kind == TokenKind.Comment &&
                            tokens[i + 1].Text.StartsWith("--", StringComparison.Ordinal))
                        {
                            writer.Append(tokens[++i]);
                            writer.NewLine(commaIndent);
                        }
                        else
                        {
                            writer.NewLine(commaIndent);
                        }
                    }
                    else
                    {
                        writer.NewLine(commaIndent);
                        writer.AppendLeadingComma(options.CommaPosition == CommaPosition.BeforeNoSpace);
                    }
                    lastSignificantWord = "";
                    continue;
                }

                Token formattedToken = TransformToken(tokens, i, clause, lastSignificantWord, options);
                writer.Append(formattedToken);

                if (token.Kind == TokenKind.Word)
                {
                    lastSignificantWord = upper;
                    if (IsClauseContext(parenKinds) && upper == "BETWEEN")
                        betweenPending = true;
                }
                else
                    lastSignificantWord = "";
            }
        }

        private static string ReadPhrase(List<Token> tokens, int start, int length)
        {
            var parts = new List<string>();
            for (int i = start; i < start + length; i++)
                parts.Add(tokens[i].Text);
            return string.Join(" ", parts);
        }

        private static bool TryReadJoin(List<Token> tokens, int index, out string joinText, out int length)
        {
            joinText = "";
            length = 0;

            if (tokens[index].Kind != TokenKind.Word)
                return false;

            string first = tokens[index].Text.ToUpperInvariant();
            if (first == "JOIN")
            {
                joinText = "JOIN";
                length = 1;
                return true;
            }

            // CROSS APPLY and OUTER APPLY introduce a table source like a join.
            if ((first == "CROSS" || first == "OUTER") &&
                index + 1 < tokens.Count &&
                tokens[index + 1].Kind == TokenKind.Word &&
                tokens[index + 1].Text.Equals("APPLY", StringComparison.OrdinalIgnoreCase))
            {
                joinText = tokens[index].Text + " " + tokens[index + 1].Text;
                length = 2;
                return true;
            }

            bool isPrefix = false;
            for (int i = 0; i < JoinPrefixes.Length; i++)
            {
                if (first == JoinPrefixes[i])
                {
                    isPrefix = true;
                    break;
                }
            }
            if (!isPrefix || index + 1 >= tokens.Count || tokens[index + 1].Kind != TokenKind.Word)
                return false;

            int joinIndex = index + 1;
            string second = tokens[joinIndex].Text.ToUpperInvariant();
            if (second == "OUTER" && joinIndex + 1 < tokens.Count &&
                tokens[joinIndex + 1].Kind == TokenKind.Word &&
                tokens[joinIndex + 1].Text.Equals("JOIN", StringComparison.OrdinalIgnoreCase))
            {
                joinIndex++;
            }
            else if (second != "JOIN")
            {
                return false;
            }

            if (!tokens[joinIndex].Text.Equals("JOIN", StringComparison.OrdinalIgnoreCase))
                return false;

            var parts = new List<string>();
            for (int i = index; i <= joinIndex; i++)
                parts.Add(tokens[i].Text);
            joinText = string.Join(" ", parts);
            length = joinIndex - index + 1;
            return true;
        }

        private static bool TryReadClause(
            List<Token> tokens,
            int index,
            out string clauseText,
            out string clauseName,
            out int length)
        {
            clauseText = "";
            clauseName = "";
            length = 0;
            if (tokens[index].Kind != TokenKind.Word)
                return false;

            string word = tokens[index].Text.ToUpperInvariant();
            string second = index + 1 < tokens.Count && tokens[index + 1].Kind == TokenKind.Word
                ? tokens[index + 1].Text.ToUpperInvariant()
                : "";

            if ((word == "GROUP" || word == "ORDER" || word == "PARTITION") && second == "BY")
            {
                clauseText = word + " BY";
                clauseName = word == "GROUP" ? "GROUP BY" : word == "ORDER" ? "ORDER BY" : "PARTITION BY";
                length = 2;
                return true;
            }
            if (word == "INSERT" && second == "INTO")
            {
                clauseText = "INSERT INTO";
                clauseName = "INSERT";
                length = 2;
                return true;
            }
            if (word == "SELECT" && second == "DISTINCT")
            {
                clauseText = "SELECT DISTINCT";
                clauseName = "SELECT";
                length = 2;
                return true;
            }
            if (word == "SELECT" || word == "FROM" || word == "WHERE" || word == "HAVING" ||
                word == "LIMIT" || word == "OFFSET" || word == "RETURNING" || word == "VALUES" ||
                word == "UPDATE" || word == "SET" || word == "UNION" || word == "EXCEPT" ||
                word == "INTERSECT" || word == "DELETE" || word == "WITH")
            {
                clauseText = word;
                clauseName = word;
                length = 1;
                return true;
            }
            return false;
        }

        private static bool IsConditionClause(string clause)
        {
            return clause == "WHERE" || clause == "HAVING" || clause == "FROM" || clause == "ON";
        }

        /// <summary>True when the token at index is an opening parenthesis that begins a subquery.</summary>
        private static bool IsSubqueryStart(List<Token> tokens, int index)
        {
            int next = index + 1;
            while (next < tokens.Count && tokens[next].Kind == TokenKind.Comment)
                next++;
            if (next >= tokens.Count || tokens[next].Kind != TokenKind.Word)
                return false;
            string word = tokens[next].Text.ToUpperInvariant();
            return word == "SELECT" || word == "WITH";
        }

        /// <summary>True when the parenthesis at index starts an IN/VALUES element list with more than one item.</summary>
        private static bool IsListStart(List<Token> tokens, int index, string clause)
        {
            bool listContext = false;

            if (index > 0 && tokens[index - 1].Kind == TokenKind.Word &&
                tokens[index - 1].Text.Equals("IN", StringComparison.OrdinalIgnoreCase))
                listContext = true;
            else if (clause == "VALUES")
                listContext = true;

            return listContext && HasTopLevelComma(tokens, index);
        }

        private static bool HasTopLevelComma(List<Token> tokens, int openIndex)
        {
            int depth = 0;

            for (int i = openIndex + 1; i < tokens.Count; i++)
            {
                Token token = tokens[i];

                if (token.Kind == TokenKind.Symbol && token.Text == "(")
                    depth++;
                else if (token.Kind == TokenKind.Symbol && token.Text == ")")
                {
                    if (depth == 0)
                        return false;
                    depth--;
                }
                else if (depth == 0 && token.Kind == TokenKind.Symbol && token.Text == ",")
                    return true;
            }

            return false;
        }

        /// <summary>
        /// True when clause-level formatting applies: at the top level of a statement or directly
        /// inside a subquery, but not inside a regular parenthesised expression.
        /// </summary>
        private static bool IsClauseContext(Stack<ParenKind> parenKinds)
        {
            return parenKinds.Count == 0 || parenKinds.Peek() == ParenKind.Subquery;
        }

        /// <summary>True when the current parenthesis is a multi-line IN/VALUES element list.</summary>
        private static bool IsListContext(Stack<ParenKind> parenKinds)
        {
            return parenKinds.Count > 0 && parenKinds.Peek() == ParenKind.List;
        }

        private static bool IsCommaListClause(string clause)
        {
            return clause.Length == 0 || clause == "SELECT" || clause == "GROUP BY" || clause == "ORDER BY" ||
                clause == "PARTITION BY" || clause == "VALUES" || clause == "SET" ||
                clause == "RETURNING";
        }

        private static Token TransformToken(
            List<Token> tokens,
            int index,
            string clause,
            string lastWord,
            Options options)
        {
            Token token = tokens[index];
            if (token.Kind != TokenKind.Word)
                return token;

            string upper = token.Text.ToUpperInvariant();
            string value = token.Text;
            bool isCatalogWord = SqlKeywords.Words.ContainsKey(upper);
            if (isCatalogWord)
                value = ApplyCase(value, options.KeywordCase);

            // Variables (@name) and system functions (@@name) can never be quoted.
            if (options.IdentifierQuoting == IdentifierQuoteStyle.None ||
                value.StartsWith("@", StringComparison.Ordinal) ||
                isCatalogWord ||
                IsFunctionName(tokens, index) || lastWord == "AS" ||
                IsImplicitTableAlias(clause, lastWord))
            {
                return new Token(value, TokenKind.Word);
            }

            string quoted;
            if (options.IdentifierQuoting == IdentifierQuoteStyle.SquareBrackets)
                quoted = "[" + value.Replace("]", "]]") + "]";
            else
                quoted = "\"" + value.Replace("\"", "\"\"") + "\"";
            return new Token(quoted, TokenKind.QuotedIdentifier);
        }

        private static bool IsFunctionName(List<Token> tokens, int index)
        {
            return index + 1 < tokens.Count &&
                tokens[index + 1].Kind == TokenKind.Symbol &&
                tokens[index + 1].Text == "(";
        }

        private static bool IsImplicitTableAlias(string clause, string lastWord)
        {
            return (clause == "FROM" || clause == "UPDATE") &&
                lastWord.Length > 0 &&
                lastWord != "FROM" && lastWord != "JOIN" && lastWord != "UPDATE" &&
                lastWord != "AS";
        }

        private static string ApplyCase(string value, KeywordCaseStyle keywordCase)
        {
            switch (keywordCase)
            {
                case KeywordCaseStyle.Upper:
                    return value.ToUpperInvariant();
                case KeywordCaseStyle.Lower:
                    return value.ToLowerInvariant();
                default:
                    return value;
            }
        }

        private static List<Token> Tokenize(string sql)
        {
            var tokens = new List<Token>();
            int i = 0;

            while (i < sql.Length)
            {
                char c = sql[i];
                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                if (c == '-' && i + 1 < sql.Length && sql[i + 1] == '-')
                {
                    int start = i;
                    i += 2;
                    while (i < sql.Length && sql[i] != '\r' && sql[i] != '\n')
                        i++;
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Comment));
                    continue;
                }

                if (c == '/' && i + 1 < sql.Length && sql[i + 1] == '*')
                {
                    int start = i;
                    i += 2;
                    int nesting = 1;
                    while (i < sql.Length && nesting > 0)
                    {
                        if (i + 1 < sql.Length && sql[i] == '/' && sql[i + 1] == '*')
                        {
                            nesting++;
                            i += 2;
                        }
                        else if (i + 1 < sql.Length && sql[i] == '*' && sql[i + 1] == '/')
                        {
                            nesting--;
                            i += 2;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Comment));
                    continue;
                }

                if ((c == 'N' || c == 'n' || c == 'X' || c == 'x') &&
                    i + 1 < sql.Length && sql[i + 1] == '\'')
                {
                    int start = i++;
                    ReadQuoted(sql, ref i, '\'');
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Literal));
                    continue;
                }

                if (c == '\'')
                {
                    int start = i;
                    ReadQuoted(sql, ref i, '\'');
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Literal));
                    continue;
                }

                if (c == '"' || c == '`')
                {
                    int start = i;
                    ReadQuoted(sql, ref i, c);
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.QuotedIdentifier));
                    continue;
                }

                if (c == '[')
                {
                    int start = i++;
                    while (i < sql.Length)
                    {
                        if (sql[i] == ']' && i + 1 < sql.Length && sql[i + 1] == ']')
                            i += 2;
                        else if (sql[i++] == ']')
                            break;
                    }
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.QuotedIdentifier));
                    continue;
                }

                if (c == 'G' || c == 'g')
                {
                    string separator = TryReadBatchSeparator(sql, ref i);
                    if (separator != null)
                    {
                        tokens.Add(new Token(separator, TokenKind.BatchSeparator));
                        continue;
                    }
                }

                if (char.IsLetter(c) || c == '_' || c == '@' || c == '#' || c == '$')
                {
                    int start = i++;
                    while (i < sql.Length &&
                        (char.IsLetterOrDigit(sql[i]) || sql[i] == '_' || sql[i] == '$' || sql[i] == '#' ||
                        sql[i] == '@'))
                        i++;
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Word));
                    continue;
                }

                if (char.IsDigit(c))
                {
                    int start = i++;
                    while (i < sql.Length && (char.IsLetterOrDigit(sql[i]) || sql[i] == '_'))
                        i++;
                    if (i < sql.Length && sql[i] == '.' && i + 1 < sql.Length && char.IsDigit(sql[i + 1]))
                    {
                        i++;
                        while (i < sql.Length && (char.IsDigit(sql[i]) || sql[i] == '_'))
                            i++;
                    }
                    if (i < sql.Length && (sql[i] == 'e' || sql[i] == 'E'))
                    {
                        int exponent = i++;
                        if (i < sql.Length && (sql[i] == '+' || sql[i] == '-'))
                            i++;
                        int digits = i;
                        while (i < sql.Length && char.IsDigit(sql[i]))
                            i++;
                        if (digits == i)
                            i = exponent;
                    }
                    tokens.Add(new Token(sql.Substring(start, i - start), TokenKind.Number));
                    continue;
                }

                string op = ReadOperator(sql, ref i);
                tokens.Add(new Token(op, TokenKind.Symbol));
            }

            return tokens;
        }

        private static void ReadQuoted(string sql, ref int index, char quote)
        {
            index++;
            while (index < sql.Length)
            {
                if (sql[index] == quote)
                {
                    if (index + 1 < sql.Length && sql[index + 1] == quote)
                    {
                        index += 2;
                        continue;
                    }
                    index++;
                    return;
                }
                // T-SQL has no backslash escape; a doubled quote is the only escape.
                index++;
            }
        }

        /// <summary>
        /// Reads a GO batch separator (GO, optionally followed by a count and a line comment) when it
        /// is the only content on its line; returns its normalized text, or null when it is not one.
        /// </summary>
        private static string TryReadBatchSeparator(string sql, ref int index)
        {
            int i = index;
            if (i + 1 >= sql.Length ||
                (sql[i] != 'G' && sql[i] != 'g') ||
                (sql[i + 1] != 'O' && sql[i + 1] != 'o'))
                return null;

            for (int p = i - 1; p >= 0 && sql[p] != '\n' && sql[p] != '\r'; p--)
            {
                if (!char.IsWhiteSpace(sql[p]))
                    return null;
            }

            var text = new StringBuilder(sql.Substring(i, 2));
            i += 2;
            int afterGo = i;
            while (i < sql.Length && (sql[i] == ' ' || sql[i] == '\t'))
                i++;
            int countStart = i;
            while (i < sql.Length && char.IsDigit(sql[i]))
                i++;
            if (i > countStart)
            {
                if (countStart == afterGo)
                    return null;
                text.Append(' ').Append(sql, countStart, i - countStart);
                while (i < sql.Length && (sql[i] == ' ' || sql[i] == '\t'))
                    i++;
            }

            if (i + 1 < sql.Length && sql[i] == '-' && sql[i + 1] == '-')
            {
                int commentStart = i;
                while (i < sql.Length && sql[i] != '\r' && sql[i] != '\n')
                    i++;
                text.Append(' ').Append(sql, commentStart, i - commentStart);
            }

            if (i < sql.Length && sql[i] != '\r' && sql[i] != '\n')
                return null;

            index = i;
            return text.ToString().TrimEnd();
        }

        private static string ReadOperator(string sql, ref int index)
        {
            string[] operators =
            {
                "->>", "<=", ">=", "!=", "<>", "||", "::", "->",
                "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^="
            };
            for (int i = 0; i < operators.Length; i++)
            {
                string candidate = operators[i];
                if (index + candidate.Length <= sql.Length &&
                    string.Compare(sql, index, candidate, 0, candidate.Length, StringComparison.Ordinal) == 0)
                {
                    index += candidate.Length;
                    return candidate;
                }
            }
            return sql[index++].ToString();
        }

        private sealed class SqlWriter
        {
            private readonly Options _options;
            private readonly List<string> _lines = new List<string>();
            private readonly StringBuilder _line = new StringBuilder();
            private Token _previous;
            private int _indentLevel;

            public SqlWriter(Options options)
            {
                _options = options;
            }

            public int CurrentIndentLevel => _indentLevel;
            public bool HasCurrentLineContent => _line.Length > 0;

            public void AppendWord(string text, Options options)
            {
                Append(new Token(ApplyCase(text, options.KeywordCase), TokenKind.Word));
            }

            public void Append(Token token)
            {
                EnsureIndent();
                if (_previous != null && NeedsSpace(_previous, token))
                    _line.Append(' ');
                _line.Append(token.Text);
                _previous = token;
            }

            public void AppendSpace()
            {
                EnsureIndent();
                if (_line.Length > 0 && _line[_line.Length - 1] != ' ')
                    _line.Append(' ');
                _previous = null;
            }

            public void AppendLeadingComma(bool noSpace)
            {
                EnsureIndent();
                _line.Append(',');
                if (!noSpace)
                    _line.Append(' ');
                _previous = null;
            }

            public void NewLine(int indentLevel)
            {
                FlushLine();
                _indentLevel = Math.Max(0, indentLevel);
                _previous = null;
            }

            public void AddBlankLines(int count)
            {
                FlushLine();
                int added = 0;
                for (int i = _lines.Count - 1; i >= 0 && _lines[i].Length == 0; i--)
                    added++;
                while (added < count)
                {
                    _lines.Add("");
                    added++;
                }
                _previous = null;
            }

            public void RemoveTrailingBlankLines()
            {
                FlushLine();
                while (_lines.Count > 0 && _lines[_lines.Count - 1].Length == 0)
                    _lines.RemoveAt(_lines.Count - 1);
                _previous = null;
            }

            public string Finish()
            {
                FlushLine();
                while (_lines.Count > 0 && _lines[_lines.Count - 1].Length == 0)
                    _lines.RemoveAt(_lines.Count - 1);
                return string.Join(Environment.NewLine, _lines);
            }

            private void EnsureIndent()
            {
                if (_line.Length != 0)
                    return;
                string unit = _options.UseTabs ? "\t" : new string(' ', _options.IndentSize);
                for (int i = 0; i < _indentLevel; i++)
                    _line.Append(unit);
            }

            private void FlushLine()
            {
                if (_line.Length == 0)
                    return;
                _lines.Add(_line.ToString().TrimEnd());
                _line.Clear();
            }

            private static bool NeedsSpace(Token previous, Token current)
            {
                string p = previous.Text;
                string c = current.Text;

                if (c == "," || c == ";" || c == ")" || c == "." || c == "::")
                    return false;
                if (p == "(" || p == "." || p == "::")
                    return false;
                if (c == "(")
                {
                    if (previous.Kind == TokenKind.Word &&
                        (p.Equals("IN", StringComparison.OrdinalIgnoreCase) ||
                        p.Equals("VALUES", StringComparison.OrdinalIgnoreCase) ||
                        p.Equals("EXISTS", StringComparison.OrdinalIgnoreCase) ||
                        p.Equals("AS", StringComparison.OrdinalIgnoreCase)))
                        return true;
                    return previous.Kind != TokenKind.Word || !IsFunctionLike(previous.Text);
                }
                if (IsOperator(p) || IsOperator(c))
                    return true;
                if (previous.Kind == TokenKind.Comment || current.Kind == TokenKind.Comment)
                    return true;
                return true;
            }

            private static bool IsFunctionLike(string value)
            {
                SqlKeywordUsage usage;
                if (!SqlKeywords.Words.TryGetValue(value, out usage))
                    return true;

                return (usage & SqlKeywordUsage.Keyword) == 0 ||
                    (usage & (SqlKeywordUsage.Function | SqlKeywordUsage.Type)) != 0;
            }

            private static bool IsOperator(string value)
            {
                return value == "=" || value == "<" || value == ">" || value == "<=" ||
                    value == ">=" || value == "!=" || value == "<>" || value == "+" ||
                    value == "-" || value == "*" || value == "/" || value == "%" ||
                    value == "||" || value == "::" || value == "+=" || value == "-=" ||
                    value == "->" || value == "->>";
            }
        }
    }
}
