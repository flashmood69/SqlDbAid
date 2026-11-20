using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlDbAid
{
    class SqlHighlighter
    {
        private static readonly Dictionary<string, Color> mKW;

        private static readonly Color COL_OPERATOR = Color.Gray;
        private static readonly Color COL_TYPE = Color.BlueViolet;
        private static readonly Color COL_KEYWORD = Color.Blue;
        private static readonly Color COL_VARFUN = Color.Magenta;
        private static readonly Color COL_COMMENT = Color.Green;
        private static readonly Color COL_STRING = Color.Red;
        private static readonly Color COL_NORMAL = Color.Black;
        private static readonly Color COL_SEARCH = Color.Yellow;

        static SqlHighlighter()
        {
            mKW = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, SqlKeywordUsage> entry in SqlKeywords.Words)
            {
                SqlKeywordUsage usage = entry.Value;
                Color color = COL_KEYWORD;
                if ((usage & SqlKeywordUsage.Function) != 0)
                    color = COL_VARFUN;
                if ((usage & SqlKeywordUsage.Type) != 0)
                    color = COL_TYPE;
                if ((usage & SqlKeywordUsage.Operator) != 0)
                    color = COL_OPERATOR;
                mKW.Add(entry.Key, color);
            }
        }

        public void Highlight(RichTextBox rtbCode, Regex searchRegex = null)
        {
            string text = rtbCode.Text;
            MatchCollection searchMatches = searchRegex == null ? null : searchRegex.Matches(text);
            StringBuilder rtf = CreateRtfHeader(rtbCode, text.Length + 256);

            int position = 0;
            int plainTextStart = 0;
            int searchMatchIndex = 0;
            while (position < text.Length)
            {
                int tokenStart = position;
                int colorIndex = 0;
                char current = text[position];

                if (current == '-' && position + 1 < text.Length && text[position + 1] == '-')
                {
                    position += 2;
                    while (position < text.Length && text[position] != '\n')
                    {
                        position++;
                    }
                    if (position < text.Length)
                    {
                        position++;
                    }
                    colorIndex = 3;
                }
                else if (current == '/' && position + 1 < text.Length && text[position + 1] == '*')
                {
                    position += 2;
                    int nesting = 1;
                    while (position < text.Length && nesting > 0)
                    {
                        if (position + 1 < text.Length && text[position] == '/' && text[position + 1] == '*')
                        {
                            nesting++;
                            position += 2;
                        }
                        else if (position + 1 < text.Length && text[position] == '*' && text[position + 1] == '/')
                        {
                            nesting--;
                            position += 2;
                        }
                        else
                        {
                            position++;
                        }
                    }
                    colorIndex = 3;
                }
                else if (current == '\'' || current == '"')
                {
                    char quote = current;
                    position++;
                    while (position < text.Length)
                    {
                        if (text[position] == quote)
                        {
                            position++;
                            if (position < text.Length && text[position] == quote)
                            {
                                position++;
                                continue;
                            }
                            break;
                        }
                        position++;
                    }
                    colorIndex = 4;
                }
                else if (current == '[')
                {
                    position++;
                    while (position < text.Length && text[position] != ']')
                    {
                        position++;
                    }
                    if (position < text.Length)
                    {
                        position++;
                    }
                    colorIndex = 2;
                }
                else if (IsTokenDelimiter(current))
                {
                    position++;
                }
                else
                {
                    position++;
                    while (position < text.Length && !IsTokenDelimiter(text[position]) &&
                        !(text[position] == '-' && position + 1 < text.Length && text[position + 1] == '-') &&
                        !(text[position] == '/' && position + 1 < text.Length && text[position + 1] == '*'))
                    {
                        position++;
                    }

                    Color color;
                    if (mKW.TryGetValue(text.Substring(tokenStart, position - tokenStart), out color))
                    {
                        colorIndex = GetColorIndex(color);
                    }
                }

                if (colorIndex != 0)
                {
                    AppendRtfText(rtf, text, plainTextStart, tokenStart - plainTextStart, searchMatches, ref searchMatchIndex);
                    rtf.Append("\\cf").Append(colorIndex).Append(' ');
                    AppendRtfText(rtf, text, tokenStart, position - tokenStart, searchMatches, ref searchMatchIndex);
                    rtf.Append("\\cf1 ");
                    plainTextStart = position;
                }
            }

            AppendRtfText(rtf, text, plainTextStart, text.Length - plainTextStart, searchMatches, ref searchMatchIndex);
            if (text.Length > 0 && (text[text.Length - 1] == '\r' || text[text.Length - 1] == '\n'))
            {
                // RichTextBox drops the final empty paragraph when importing RTF.
                rtf.Append(@"\par ");
            }
            rtf.Append('}');

            rtbCode.Rtf = rtf.ToString();
            rtbCode.Select(0, 0);
            rtbCode.Invalidate();
        }

        private static StringBuilder CreateRtfHeader(RichTextBox rtbCode, int capacity)
        {
            Color[] colors =
            {
                rtbCode.ForeColor,
                COL_NORMAL,
                COL_COMMENT,
                COL_STRING,
                COL_KEYWORD,
                COL_OPERATOR,
                COL_TYPE,
                COL_VARFUN,
                COL_SEARCH
            };
            StringBuilder rtf = new StringBuilder(capacity);
            rtf.Append(@"{\rtf1\ansi\deff0\uc1{\fonttbl{\f0\fcharset0 ");
            AppendRtfFontName(rtf, rtbCode.Font.Name);
            rtf.Append(@";}}{\colortbl ;");
            foreach (Color color in colors)
            {
                rtf.Append(@"\red").Append(color.R)
                    .Append(@"\green").Append(color.G)
                    .Append(@"\blue").Append(color.B).Append(';');
            }
            rtf.Append(@"}\viewkind4\pard\f0\fs")
                .Append((int)Math.Round(rtbCode.Font.Size * 2))
                .Append(@"\cf1 ");

            FontStyle fontStyle = rtbCode.Font.Style;
            rtf.Append(fontStyle.HasFlag(FontStyle.Bold) ? @"\b " : @"\b0 ");
            rtf.Append(fontStyle.HasFlag(FontStyle.Italic) ? @"\i " : @"\i0 ");
            rtf.Append(fontStyle.HasFlag(FontStyle.Underline) ? @"\ul " : @"\ul0 ");
            rtf.Append(fontStyle.HasFlag(FontStyle.Strikeout) ? @"\strike " : @"\strike0 ");
            return rtf;
        }

        private static void AppendRtfFontName(StringBuilder rtf, string fontName)
        {
            foreach (char value in fontName)
            {
                if (value == '\\' || value == '{' || value == '}')
                {
                    rtf.Append('\\');
                }
                rtf.Append(value);
            }
        }

        private static void AppendRtfText(StringBuilder rtf, string text, int start, int length)
        {
            int end = start + length;
            for (int i = start; i < end; i++)
            {
                char value = text[i];
                switch (value)
                {
                    case '\\':
                    case '{':
                    case '}':
                        rtf.Append('\\').Append(value);
                        break;
                    case '\r':
                        if (i + 1 < end && text[i + 1] == '\n')
                        {
                            i++;
                        }
                        rtf.Append(@"\par ");
                        break;
                    case '\n':
                        rtf.Append(@"\par ");
                        break;
                    case '\t':
                        rtf.Append(@"\tab ");
                        break;
                    default:
                        if (value < 32 || value > 126)
                        {
                            rtf.Append(@"\u").Append(unchecked((short)value)).Append('?');
                        }
                        else
                        {
                            rtf.Append(value);
                        }
                        break;
                }
            }
        }

        private static void AppendRtfText(
            StringBuilder rtf,
            string text,
            int start,
            int length,
            MatchCollection searchMatches,
            ref int searchMatchIndex)
        {
            if (searchMatches == null || searchMatches.Count == 0)
            {
                AppendRtfText(rtf, text, start, length);
                return;
            }

            int position = start;
            int end = start + length;
            while (position < end)
            {
                while (searchMatchIndex < searchMatches.Count &&
                    ((Match)searchMatches[searchMatchIndex]).Index + ((Match)searchMatches[searchMatchIndex]).Length <= position)
                {
                    searchMatchIndex++;
                }

                if (searchMatchIndex >= searchMatches.Count ||
                    ((Match)searchMatches[searchMatchIndex]).Index >= end)
                {
                    AppendRtfText(rtf, text, position, end - position);
                    break;
                }

                Match match = (Match)searchMatches[searchMatchIndex];
                if (match.Index > position)
                {
                    int plainEnd = Math.Min(match.Index, end);
                    AppendRtfText(rtf, text, position, plainEnd - position);
                    position = plainEnd;
                }
                else
                {
                    int matchEnd = Math.Min(match.Index + match.Length, end);
                    rtf.Append(@"\highlight9 ");
                    AppendRtfText(rtf, text, position, matchEnd - position);
                    rtf.Append(@"\highlight0 ");
                    position = matchEnd;
                }
            }
        }

        private static int GetColorIndex(Color color)
        {
            if (color == COL_NORMAL) return 2;
            if (color == COL_COMMENT) return 3;
            if (color == COL_STRING) return 4;
            if (color == COL_KEYWORD) return 5;
            if (color == COL_OPERATOR) return 6;
            if (color == COL_TYPE) return 7;
            if (color == COL_VARFUN) return 8;
            return 1;
        }

        private static bool IsTokenDelimiter(char value)
        {
            return char.IsWhiteSpace(value) ||
                value == '[' || value == ']' || value == '(' || value == ')' ||
                value == ',' || value == ';' || value == '\'' || value == '"' ||
                value == '=' || value == '!' || value == '<' || value == '>' ||
                value == '+' || value == '-' || value == '*' || value == '/' ||
                value == '%' || value == '&' || value == '|' || value == '^';
        }

    }
}
