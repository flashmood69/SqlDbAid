using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlDbAid
{
    class HttpUrl
    {
        public static string RepoOwner = "flashmood69";
        public static string RepoName = "SqlDbAid";
        public static string AppVersionUrl = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
        public static string AppUrl = $"https://github.com/{RepoOwner}/{RepoName}";
    }

    class FileHelper
    {
        public static string CleanFileName(string fileName)
        {
            return Regex.Replace(fileName, @"[\\/:*?""<>|]", "");
        }
    }

    class ClipboardHelper
    {
        public static string RemoveFirstTab(string clipboardText)
        {
            string[] lines = Regex.Split(clipboardText, "\r\n");

            StringBuilder newClipData = new StringBuilder();
            foreach (String line in lines)
            {
                newClipData.AppendLine(line.Substring(line.IndexOf("\t") + 1));
            }

            return newClipData.ToString();
        }
    }

    class QueryHelper
    {
        public enum QueryId
        {
            GuiDatabase,
            GuiObjType,
            GuiObjCode,

            RunnableCheck,
            FeatureTest,

            MissingFkIndexes,
            MissingIndexes,
            IndexesStatus,
            ExistingIndexes,

            TableStatistics,
            TableMBytes,

            DatabaseFiles,
            DatabasePermissions,
            DatabaseRoles,

            ServerConfigurations,
            ServerProperties,
            ServerRoles,
            Processes,
            Jobs,
            Deadlocks,
            Locks,
            TopQueries,
            Cpu,

            PlansCache,
            DatabasesCache,
            ObjectsCache,

            ScriptViewSelect,
            ScriptSelect,
            ScriptInsert,
            ScriptUpdate,
            ScriptCountDistinct,

            ColumnInconsistency,

            ExportData,

            DropTemplate,
            DropSchemaTemplate,
            DropTypeTemplate,

            ViewData,

            DataSearch
        }

        public static string Query(QueryId queryId)
        {
            if (queryId == QueryId.GuiDatabase)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT 1 id, [name] database_name
FROM sys.databases
{0}WHERE name NOT IN (N'master', N'model', N'msdb', N'tempdb')
UNION ALL
SELECT 0 id, 'All DBs' database_name
ORDER BY 1,2
/*sda*/
";
            }
            else if (queryId == QueryId.GuiObjType)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	[type] obj_value,
	CASE [type]
		WHEN 'FN' THEN 'Function'
		WHEN 'P' THEN 'Procedure'
		WHEN 'V' THEN 'View'
		WHEN 'U' THEN 'Table'
		WHEN 'TR' THEN 'Trigger'
		WHEN 'SN' THEN 'Synonym'
		WHEN 'SC' THEN 'Schema'
		WHEN 'UD' THEN 'Type'
		ELSE ''
	END + ' (' + CONVERT(VARCHAR, COUNT(*)) + ')' obj_desc
FROM
	(
		SELECT
			CASE
				WHEN O.type IN ('FN','IF','TF','FS','FT') THEN 'FN'
				ELSE O.type
			END [type]
		FROM
			sys.objects O
		WHERE
			NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties EP WHERE EP.major_id = O.object_id AND EP.name = 'microsoft_database_tools_support') AND
			O.is_ms_shipped = 0 AND
			O.type IN ('U','P','V','TR','SN', 'FN','IF','TF','FS','FT')
		UNION ALL
		SELECT 'SC' FROM sys.schemas WHERE schema_id BETWEEN 5 AND 16383
		UNION ALL
		SELECT 'UD' FROM sys.types WHERE is_user_defined = 1
	) T
GROUP BY
	[type]
ORDER BY 2
/*sda*/
";
            }
            else if (queryId == QueryId.GuiObjCode)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET ARITHABORT ON
/*
SELECT
	CONVERT(BIT, 1) selected,
	CONVERT(VARCHAR(50), @type) obj_type,
	CONVERT(NVARCHAR(128), '') obj_schema,
	CONVERT(NVARCHAR(128), '') obj_name,
	CONVERT(VARCHAR(30), GETDATE(), 120) modify_date,
	CONVERT(NVARCHAR(max), '') code,
	CONVERT(INT, 0) ord_id
WHERE
	@index IN ('Y', 'N') AND
	@contains = ''
*/
--SQL Approach - 2008-09-16; 2020-05-22
--ScriptSqlObjects by Miken
--DECLARE @type VARCHAR(50)
--DECLARE @index VARCHAR(1)
--DECLARE @contains NVARCHAR(200)
--SET @type = '#U#V#' --AL=All, U=UserTable, P=Procedure, V=View, FN=Function, TR=Trigger, SN=Synonym, SC=Schema, UD=UserDataType
--SET @index = 'Y'
--SET @contains = ''

DECLARE @DEPTH INT
DECLARE @COUNT INT

DECLARE @collation_name NVARCHAR(128)

DECLARE @MAX_ROW_NUM INT
DECLARE @I INT

IF OBJECT_ID('tempdb..#OBJ') IS NOT NULL
    DROP TABLE #OBJ

IF NOT OBJECT_ID('tempdb..#TB') IS NULL
	DROP TABLE #TB

--store object depth
CREATE TABLE #OBJ
(
	object_id INT NOT NULL,
	depth INT,
	ord_id INT,
	obj_name NVARCHAR(300),
	obj_type VARCHAR(10),
	PRIMARY KEY (object_id)
)

--store table definition rows
CREATE TABLE #TB
(
	row_num INT NOT NULL,
	object_id INT NOT NULL,
	column_id INT,
	PRIMARY KEY (object_id, row_num)
)

SELECT @collation_name = collation_name FROM sys.databases WHERE database_id = DB_ID()
/*sda*/
EXEC('ALTER TABLE #TB ADD definition NVARCHAR(max) COLLATE ' + @collation_name)

--init object depth
INSERT INTO #OBJ
(
	object_id,
	depth,
	obj_name,
	obj_type
)
SELECT
	O.object_id,
	0 depth,
	SCHEMA_NAME(O.schema_id) + '.' + O.name,
	CASE
		WHEN O.type = 'U' THEN 'A'
		WHEN O.type = 'TR' THEN 'B'
		WHEN O.type IN ('FN','IF','TF','FS','FT') THEN 'C'
		WHEN O.type = 'V' THEN 'D'
		WHEN O.type = 'P' THEN 'Y'
		WHEN O.type = 'SN' THEN 'Z'
		ELSE O.type
	END obj_type
FROM
	sys.objects O
WHERE
	NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties EP WHERE EP.major_id = O.object_id AND EP.name = 'microsoft_database_tools_support') AND
	O.is_ms_shipped = 0 AND
	O.type IN ('U','P','V','TR','SN', 'FN','IF','TF','FS','FT')
/*sda*/

SET @COUNT = @@ROWCOUNT         
SET @DEPTH = 0

--compute object depth
WHILE @COUNT > 0 AND @DEPTH < 25 --stop circular references
BEGIN
	SET @DEPTH = @DEPTH + 1

	UPDATE T1 SET
		DEPTH = @DEPTH
	FROM
		#OBJ T1
			INNER JOIN
		(
			SELECT parent_object_id, referenced_object_id FROM sys.foreign_keys
			UNION
			SELECT object_id, referenced_major_id FROM sys.sql_dependencies
			UNION
			SELECT object_id, parent_id FROM sys.triggers
		) F ON
			F.parent_object_id = T1.object_id
			INNER JOIN
		#OBJ T2 ON
			T2.object_id = F.referenced_object_id
	WHERE
		T1.object_id <> T2.object_id AND --avoid self references
		T2.depth = @DEPTH-1
/*sda*/

	SET @COUNT = @@ROWCOUNT
END

--generate order
UPDATE X SET
	ord_id = Y.ord_id
FROM
	#OBJ X
		INNER JOIN
	(
		SELECT
			object_id,
			ROW_NUMBER() OVER (ORDER BY depth, obj_type, obj_name) ord_id
		FROM
			#OBJ
	) Y ON
		Y.object_id = X.object_id
/*sda*/

--create table definition
IF @type LIKE '%#AL#%' OR @type LIKE '%#U#%'
BEGIN
	INSERT INTO #TB
	SELECT
		--tb_schema,
		--tb_name,
		ROW_NUMBER() OVER(PARTITION BY X.tb_schema, X.tb_name ORDER BY X.column_id, X.cl_constraint) row_num,
		X.object_id,
		X.column_id,
		CASE WHEN X.column_id = Y.min_column_id THEN 'CREATE TABLE ' + X.tb_schema + '.' + X.tb_name + CHAR(13) + CHAR(10) + '(' + CHAR(13) + CHAR(10) ELSE '' END +
		CASE WHEN X.column_id > Y.min_column_id AND X.column_id < Y.max_column_id THEN ',' + CHAR(13) + CHAR(10) ELSE '' END +
		CASE X.cl_name WHEN '' THEN '' ELSE CHAR(9) + X.cl_name + ' ' + X.cl_type + X.cl_len END +
		CASE X.cl_collation WHEN '' THEN '' ELSE ' ' + X.cl_collation END +
		CASE X.cl_identity WHEN '' THEN '' ELSE ' ' + X.cl_identity END +
		CASE X.cl_null WHEN '' THEN '' ELSE ' ' + X.cl_null END +
		CASE X.cl_computed WHEN '' THEN '' ELSE X.cl_computed END +
		CASE
			WHEN X.cl_constraint = '' THEN ''
			WHEN X.cl_constraint NOT LIKE '% DEFAULT %' THEN CHAR(9) + X.cl_constraint
			ELSE ' ' + X.cl_constraint
		END +
		CASE WHEN X.column_id = Y.max_column_id THEN CHAR(13) + CHAR(10) +')' ELSE '' END AS definition
	FROM
		(
			SELECT
				QUOTENAME(SCHEMA_NAME(TB.schema_id)) tb_schema,
				QUOTENAME(TB.name) tb_name,
				QUOTENAME(CL.name) cl_name,
				CASE
					WHEN CL.is_computed = 1 THEN ''
					WHEN TY.is_user_defined = 1 THEN QUOTENAME(SCHEMA_NAME(TY.schema_id)) + '.' + QUOTENAME(TY.name)
					ELSE QUOTENAME(TY.name)
				END cl_type,
				CASE
					WHEN CL.is_computed = 1 THEN ''
					WHEN TY.name = 'xml' THEN
						CASE CL.is_xml_document
							WHEN 1 THEN (SELECT '(DOCUMENT ' + QUOTENAME(SCHEMA_NAME(XSC.schema_id)) + '.' + QUOTENAME(XSC.name) + ')' FROM sys.xml_schema_collections XSC WHERE XSC.xml_collection_id = CL.xml_collection_id)
							ELSE ''
						END
					WHEN CL.max_length = -1 AND TY.name NOT IN ('geography','geometry') THEN '(max)'
					WHEN TY.name IN ('binary','char','varbinary','varchar') THEN '(' + CONVERT(VARCHAR, CL.max_length) + ')'
					WHEN TY.name IN ('nchar','nvarchar') THEN '(' + CONVERT(VARCHAR, CL.max_length/2) + ')'
					WHEN TY.name IN ('decimal','numeric') THEN '(' + CONVERT(VARCHAR, CL.precision) + ',' + CONVERT(VARCHAR, CL.scale) + ')'
					WHEN TY.name IN ('datetime2','datetimeoffset','time') THEN '(' + CONVERT(VARCHAR, CL.scale) + ')'
					ELSE ''
				END cl_len,
				CASE
					WHEN CL.is_computed = 1 THEN ''
					WHEN TY.is_user_defined = 1 THEN ''
					ELSE ISNULL('COLLATE ' + CL.collation_name, '')
				END cl_collation,
				CASE
					WHEN CL.is_identity = 1 THEN 'IDENTITY(' + CONVERT(VARCHAR, IC.seed_value) + ',' + CONVERT(VARCHAR, IC.increment_value) + ')'
					ELSE ''
				END cl_identity,
				CASE
					WHEN CL.is_computed = 1 THEN ''
					WHEN CL.is_nullable = 1 THEN 'NULL'
					ELSE 'NOT NULL'
				END cl_null,
				ISNULL('AS ' + CC.definition, '') +
				CASE
					WHEN CC.is_persisted = 1 THEN ' PERSISTED'
					ELSE ''
				END cl_computed,
				ISNULL('CONSTRAINT ' + QUOTENAME(SC.name) + ' DEFAULT ' + SC.definition, '') +
				CASE
					WHEN CL.is_rowguidcol = 1 THEN ' ROWGUIDCOL'
					ELSE ''
				END cl_constraint,
				CL.column_id,
				TB.object_id
			FROM
				sys.tables TB
					INNER JOIN
				sys.columns CL ON
					CL.object_id = TB.object_id
					INNER JOIN
				sys.types TY ON
					TY.user_type_id = CL.user_type_id
					LEFT JOIN
				sys.default_constraints SC ON
					SC.object_id = CL.default_object_id AND
					SC.parent_column_id = CL.column_id
					LEFT JOIN
				sys.identity_columns IC ON
					IC.object_id = TB.object_id AND
					IC.column_id = CL.column_id
					LEFT JOIN
				sys.computed_columns CC ON
					CC.object_id = TB.object_id AND
					CC.column_id = CL.column_id
			UNION ALL
			SELECT
				QUOTENAME(SCHEMA_NAME(TB.schema_id)) tb_schema,
				QUOTENAME(TB.name) tb_name,
				'' cl_name,
				'' cl_type,
				'' cl_len,
				'' cl_collation,
				'' cl_identity,
				'' cl_null,
				'' cl_computed,
				'CONSTRAINT ' + QUOTENAME(CC.name) + ' CHECK ' + CC.definition cl_constraint,
				8000 column_id,
				TB.object_id
			FROM
				sys.tables TB
					INNER JOIN
				sys.check_constraints CC ON
					CC.parent_object_id = TB.object_id
			UNION ALL
			SELECT
				QUOTENAME(tb_schema) tb_schema,
				QUOTENAME(tb_name) tb_name,
				'' cl_name,
				'' cl_type,
				'' cl_len,
				'' cl_collation,
				'' cl_identity,
				'' cl_null,
				'' cl_computed,
				'CONSTRAINT ' + QUOTENAME(cn_name) + ' ' + cn_type + ' ' + ix_type + ' (' +
				CONVERT(XML,'<r>' + SUBSTRING(cl_constraint, 1, LEN(cl_constraint)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')' cl_constraint,
				column_id,
				object_id
			FROM
				(
					SELECT
						SCHEMA_NAME(TB.schema_id) tb_schema,
						TB.name tb_name,
						i.name cn_name,
						(
							SELECT
								QUOTENAME(c.name) + CASE WHEN kic.is_descending_key = 1 THEN ' DESC,' ELSE ',' END
							FROM
								sys.index_columns kic
									INNER JOIN
								sys.columns c ON
									c.object_id = kic.object_id AND
									c.column_id = kic.column_id
							WHERE
								kic.object_id = i.object_id AND
								kic.index_id = i.index_id
							ORDER BY kic.key_ordinal
							FOR XML PATH('')
						) cl_constraint,
						CASE kc.type WHEN 'PK' THEN 'PRIMARY KEY' ELSE 'UNIQUE' END cn_type,
						CASE i.type WHEN 1 THEN 'CLUSTERED' ELSE 'NONCLUSTERED' END ix_type,
						CASE kc.type WHEN 'PK' THEN 8001 ELSE 8002 END column_id,
						TB.object_id
					FROM
						sys.tables TB
							INNER JOIN
						sys.key_constraints kc ON
							kc.parent_object_id = TB.object_id
							INNER JOIN
						sys.indexes i ON
							i.object_id = kc.parent_object_id AND
							i.index_id = kc.unique_index_id
					WHERE
						kc.type IN ('PK', 'UQ')
				) PK
			UNION ALL
			SELECT
				QUOTENAME(SCHEMA_NAME(TB.schema_id)) tb_schema,
				QUOTENAME(TB.name) tb_name,
				'' cl_name,
				'' cl_type,
				'' cl_len,
				'' cl_collation,
				'' cl_identity,
				'' cl_null,
				'' cl_computed,
				'CONSTRAINT ' + QUOTENAME(FK.name) + ' FOREIGN KEY (' + CONVERT(XML,'<r>' + SUBSTRING(FK.parent_columns, 1, LEN(FK.parent_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ') ' +
				'REFERENCES ' + QUOTENAME(SCHEMA_NAME(TBR.schema_id)) + '.' + QUOTENAME(TBR.name) + ' (' + CONVERT(XML,'<r>' + SUBSTRING(FK.referenced_columns, 1, LEN(FK.referenced_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')' +
				FK.update_referential_action + FK.delete_referential_action cl_constraint,
				8003 column_id,
				TB.object_id
			FROM
				(
					SELECT
						fk.name,
						fk.parent_object_id,
						fk.referenced_object_id,
						CASE fk.update_referential_action
							WHEN 1 THEN ' ON UPDATE CASCADE'
							WHEN 2 THEN ' ON UPDATE SET NULL'
							WHEN 3 THEN ' ON UPDATE SET DEFAULT'
							ELSE ''
						END update_referential_action,
						CASE fk.delete_referential_action
							WHEN 1 THEN ' ON DELETE CASCADE'
							WHEN 2 THEN ' ON DELETE SET NULL'
							WHEN 3 THEN ' ON DELETE SET DEFAULT'
							ELSE ''
						END delete_referential_action,
						(
							SELECT QUOTENAME(cp.name) + ','
							FROM
								sys.foreign_key_columns fkc
									INNER JOIN
								sys.columns cp ON
									fkc.parent_object_id = cp.object_id AND
									fkc.parent_column_id = cp.column_id
							WHERE
								fkc.constraint_object_id = fk.object_id
							ORDER BY fkc.constraint_column_id
							FOR XML PATH('')
						) parent_columns,
						(
							SELECT QUOTENAME(cr.name) + ','
							FROM
								sys.foreign_key_columns fkc
									INNER JOIN
								sys.columns cr ON
									fkc.referenced_object_id = cr.object_id AND
									fkc.referenced_column_id = cr.column_id
							WHERE
								fkc.constraint_object_id = fk.object_id
							ORDER BY fkc.constraint_column_id
							FOR XML PATH('')
						) referenced_columns
					FROM
						sys.foreign_keys fk
				) FK
					INNER JOIN
				sys.tables TB ON
					TB.object_id = FK.parent_object_id
					INNER JOIN
				sys.tables TBR ON
					TBR.object_id = FK.referenced_object_id
			UNION ALL
			SELECT
				QUOTENAME(SCHEMA_NAME(TB.schema_id)) tb_schema,
				QUOTENAME(TB.name) tb_name,
				'' cl_name,
				'' cl_type,
				'' cl_len,
				'' cl_collation,
				'' cl_identity,
				'' cl_null,
				'' cl_computed,
				'' cl_constraint,
				9000 column_id,
				TB.object_id
			FROM
				sys.tables TB
			) X
				INNER JOIN
			(
				SELECT
					MIN(CL.column_id) min_column_id,
					9000 max_column_id,
					TB.object_id
				FROM
					sys.tables TB
						INNER JOIN
					sys.columns CL ON
						CL.object_id = TB.object_id
				GROUP BY
					TB.object_id
			) Y ON
				Y.object_id = X.object_id
	ORDER BY
		tb_schema,
		tb_name,
		row_num
/*sda*/

	IF @index = 'Y'
	BEGIN
		INSERT INTO #TB
		SELECT
			TB.row_num + ROW_NUMBER() OVER (PARTITION BY TB.object_id ORDER BY IX.index_name) row_num,
			TB.object_id,
			0 column_id,
			CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + 'CREATE' + cn_type + ix_type + ' INDEX ' + QUOTENAME(index_name) + ' ON ' + QUOTENAME(object_schema) + '.' + QUOTENAME([object_name]) +
			CASE
				WHEN ix_type LIKE '%COLUMNSTORE%' THEN ''
				WHEN IX.type IN (5, 6) THEN ' (' + CONVERT(XML,'<r>' + SUBSTRING(inc_columns, 1, LEN(inc_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')'
				ELSE ' (' + CONVERT(XML,'<r>' + SUBSTRING(idx_columns, 1, LEN(idx_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')'
			END
			 +
			CASE
				WHEN ISNULL(inc_columns, '') = '' OR IX.type NOT IN (1, 2) THEN ''
				ELSE ' INCLUDE (' + CONVERT(XML,'<r>' + SUBSTRING(inc_columns, 1, LEN(inc_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')'
			END
			{0}+ CASE WHEN IX.filter_definition IS NOT NULL THEN ' WHERE ' + IX.filter_definition ELSE '' END
			+ CASE
				WHEN idk + flf + pad <> '' THEN	REPLACE(' WITH' + idk + flf + pad, 'WITH,', 'WITH')
				ELSE ''
			END definition
		FROM
			(SELECT object_id, MAX(row_num) row_num FROM #TB GROUP BY object_id) TB
				INNER JOIN
			(
				SELECT
					O.object_id,
					SCHEMA_NAME(O.schema_id) object_schema,
					O.name [object_name],
					I.name index_name,
					(
						SELECT
							QUOTENAME(C.name) + CASE WHEN IC.is_descending_key = 1 THEN ' DESC,' ELSE ',' END
						FROM
							sys.index_columns IC
								inner join
							sys.columns C on
								C.object_id = IC.object_id and
								C.column_id = IC.column_id
						WHERE
							IC.is_included_column = 0 AND
							IC.object_id = I.object_id and
							IC.index_id = I.index_id
						ORDER BY IC.key_ordinal
						FOR XML PATH('')
					) idx_columns,
					(
						SELECT QUOTENAME(C.name) + ','
						FROM
							sys.index_columns IC
								inner join
							sys.columns C on
								C.object_id = IC.object_id and
								C.column_id = IC.column_id
						WHERE
							IC.is_included_column = 1 AND
							IC.object_id = I.object_id and
							IC.index_id = I.index_id
						ORDER BY IC.key_ordinal
						FOR XML PATH('')
					) inc_columns,
					CASE I.is_unique WHEN 0 THEN '' ELSE ' UNIQUE' END cn_type,
					' ' + I.type_desc COLLATE DATABASE_DEFAULT ix_type,
					CASE I.ignore_dup_key WHEN 0 THEN '' ELSE ', IGNORE_DUP_KEY' END idk,
					CASE I.fill_factor WHEN 0 THEN '' ELSE ', FILLFACTOR = ' + CONVERT(VARCHAR, I.fill_factor) END flf,
					CASE I.is_padded WHEN 0 THEN '' ELSE ', PAD_INDEX' END pad,
                    {0}I.filter_definition,
					I.type
				FROM
					sys.objects O
						inner join
					sys.indexes I on
						I.object_id = O.object_id
				WHERE
					NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = O.object_id AND ep.name = 'microsoft_database_tools_support') AND
					O.is_ms_shipped = 0 AND
					I.is_hypothetical = 0 AND
					I.type IN (1, 2, 5, 6) AND
					I.is_primary_key = 0 AND
					I.is_unique_constraint = 0
			) IX ON
				IX.object_id = TB.object_id
		ORDER BY
			object_schema,
			[object_name],
			ix_type,
			index_name
/*sda*/
	END

	--concatenate definition rows
	IF EXISTS(SELECT TOP 1 1 FROM #TB)
	BEGIN
		SELECT @MAX_ROW_NUM = MAX(row_num) FROM #TB
		SET @I = 2
		WHILE @I <= @MAX_ROW_NUM
		BEGIN
			UPDATE FST SET
				FST.definition = FST.definition + ACT.definition
			FROM
				#TB FST
					INNER JOIN
				#TB ACT ON
					ACT.object_id = FST.object_id
			WHERE
				FST.row_num = 1 AND
				ACT.row_num = @I
/*sda*/
			SET @I = @I + 1
		END
	END
END

--final query
SELECT
	CONVERT(BIT, 1) selected,
	RTRIM(obj_type) obj_type,
	obj_schema,
	obj_name,
	modify_date,
	code,
	CONVERT(INT, ROW_NUMBER() OVER (ORDER BY X.grp_id, X.ord_id)) ord_id
FROM
	(
		SELECT
			CASE
				WHEN O.type = 'U' THEN 'Table'
				WHEN O.type = 'V' THEN 'View'
				WHEN O.type = 'P' THEN 'Procedure'
				WHEN O.type = 'TR' THEN 'Trigger'
				WHEN O.type = 'SN' THEN 'Synonym'
				WHEN O.type IN ('FN','IF','TF','FS','FT') THEN 'Function'
				ELSE O.type
			END obj_type,
			SCHEMA_NAME(O.schema_id) obj_schema,
			O.name obj_name,
			CONVERT(VARCHAR, O.modify_date, 120) modify_date,
			CONVERT(NVARCHAR(MAX), COALESCE(U.definition, SN.definition, SM.definition)) code,
			D.ord_id,
            2 grp_id
		FROM
			sys.objects O
				INNER JOIN
			#OBJ D ON
				D.object_id = O.object_id
				LEFT JOIN
			#TB U ON
				U.row_num = 1 AND
				U.object_id = O.object_id
				LEFT JOIN
			(
				SELECT
					object_id,
					CONVERT(NVARCHAR(MAX), 'CREATE SYNONYM ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ' FOR ' + base_object_name) definition
				FROM
					sys.synonyms
			) SN ON
				SN.object_id = O.object_id
				LEFT JOIN
			sys.sql_modules SM ON
				SM.object_id = O.object_id
		WHERE
			NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties EP WHERE EP.major_id = O.object_id AND EP.name = 'microsoft_database_tools_support') AND
			O.is_ms_shipped = 0 AND
			O.type IN ('U','P','V','TR','SN', 'FN','IF','TF','FS','FT') AND
			CASE
				WHEN @type LIKE '%#AL#%' THEN 1
				WHEN @type LIKE '%#' + RTRIM(O.type) + '#%' THEN 1
				WHEN @type LIKE '%#FN#%' AND O.type IN ('FN','IF','TF','FS','FT') THEN 1
				ELSE 0
			END = 1
		UNION ALL
		SELECT 'Schema' obj_type, '' obj_schema, name obj_name, '' modify_date, 'CREATE SCHEMA ' + QUOTENAME(name) code, schema_id ord_id, 0 grp_id FROM sys.schemas WHERE schema_id BETWEEN 5 AND 16383 AND CASE WHEN @type LIKE '%#AL#%' OR @type LIKE '%#SC#%' THEN 1 ELSE 0 END = 1
		UNION ALL
		SELECT
            'Type' obj_type,
			SCHEMA_NAME(UT.schema_id) obj_schema,
			UT.name obj_name,
			'' modify_date,
			'CREATE TYPE ' + QUOTENAME(SCHEMA_NAME(UT.schema_id)) + '.' + QUOTENAME(UT.name) + ' FROM ' + QUOTENAME(ST.name) +
			CASE
				WHEN UT.max_length = -1 AND ST.name NOT IN ('geography','geometry') THEN '(max)'
				WHEN ST.name IN ('binary','char','varbinary','varchar') THEN '(' + CONVERT(VARCHAR, UT.max_length) + ')'
				WHEN ST.name IN ('nchar','nvarchar') THEN '(' + CONVERT(VARCHAR, UT.max_length/2) + ')'
				WHEN ST.name IN ('decimal','numeric') THEN '(' + CONVERT(VARCHAR, UT.precision) + ',' + CONVERT(VARCHAR, UT.scale) + ')'
				WHEN ST.name IN ('datetime2','datetimeoffset','time') THEN '(' + CONVERT(VARCHAR, UT.scale) + ')'
				ELSE ''
			END +
			CASE
				WHEN UT.is_nullable = 1 THEN ' NULL'
				ELSE ' NOT NULL'
			END code,
			UT.user_type_id ord_id,
			1 grp_id
		FROM
			sys.types UT
				INNER JOIN
			sys.types ST ON
				ST.user_type_id = UT.system_type_id AND
				ST.is_user_defined = 0
		WHERE
			UT.is_user_defined = 1 AND CASE WHEN @type LIKE '%#AL#%' OR @type LIKE '%#UD#%' THEN 1 ELSE 0 END = 1
	) X
WHERE
	CASE
		WHEN @contains = '' THEN 1
		WHEN code LIKE '%' + @contains + '%' THEN 1
		ELSE 0
	END = 1
ORDER BY
	obj_type,
	obj_schema,
	obj_name
/*sda*/
";
            }
            else if (queryId == QueryId.ScriptSelect || queryId == QueryId.ScriptViewSelect)
            {
                string select = @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--DECLARE @schema NVARCHAR(128)
--DECLARE @name NVARCHAR(128)
--SET @schema = ''
--SET @name = ''

DECLARE @code NVARCHAR(MAX)

SET @code = ''

SELECT
	@code = @code +
	CASE
		WHEN SC.row_num = 1 THEN ''
		ELSE ','
	END +
	CHAR(13) + CHAR(10) + CHAR(9) + QUOTENAME(SC.cl_name)
FROM
	(
		SELECT
			ROW_NUMBER() OVER (ORDER BY CL.column_id) row_num,
			CL.name cl_name,
			CL.is_identity,
			CL.is_rowguidcol,
			CL.is_computed
		FROM
			sys.{0}s TB
				INNER JOIN
			sys.columns CL ON
			CL.object_id = TB.object_id
		WHERE
			TB.schema_id = SCHEMA_ID(@schema) AND
			TB.name = @name
	) SC
ORDER BY
	SC.row_num
/*sda*/

SET @code = 'SELECT' + @code + '
FROM
' + CHAR(9) + QUOTENAME(@schema) + '.' + QUOTENAME(@name) + '
'

SELECT @code code
";
                if (queryId == QueryId.ScriptSelect)
                {
                    return string.Format(select, "table");
                }
                else
                {
                    return string.Format(select, "view");
                }
            }
            else if (queryId == QueryId.ScriptCountDistinct)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--DECLARE @schema NVARCHAR(128)
--DECLARE @name NVARCHAR(128)
--SET @schema = ''
--SET @name = ''

DECLARE @code NVARCHAR(MAX)

SET @code = ''

SELECT
	@code = @code +
	CASE
		WHEN SC.row_num = 1 THEN 'SELECT '
		ELSE CHAR(13) + CHAR(10) + 'UNION ALL SELECT '
	END +
	CASE
		WHEN SC.is_computed = 1 OR SC.max_length = -1 OR SC.cl_type IN ('image','text','ntext','xml') THEN 'NULL'
		ELSE 'COUNT_BIG(DISTINCT ' + QUOTENAME(SC.cl_name) + ')'
	END +
	' distinct_values,N''' + REPLACE(SC.cl_name, '''', '''''') + ''' column_name,' +
	'''' + SC.cl_type + ''' column_type,' +
	CONVERT(VARCHAR, SC.max_length) + ' max_length,' +
	CONVERT(VARCHAR, SC.precision) + ' precision,' +
	CONVERT(VARCHAR, SC.scale) + ' scale,' +
	CONVERT(VARCHAR, SC.is_nullable) + ' is_nullable,' +
	CONVERT(VARCHAR, SC.is_identity) + ' is_identity,' +
	CONVERT(VARCHAR, SC.is_computed) + ' is_computed' +
	CASE
		WHEN SC.is_computed = 1 OR SC.max_length = -1 OR SC.cl_type IN ('image','text','ntext','xml') THEN ''
		ELSE ' FROM ' + QUOTENAME(@schema) + '.' + QUOTENAME(@name)
	END
FROM
	(
		SELECT
			ROW_NUMBER() OVER (ORDER BY CL.column_id) row_num,
			CL.name cl_name,
			TY.name cl_type,
            CL.max_length,
            CL.precision,
            CL.scale,
            CL.is_nullable,
            CL.is_identity,
            CL.is_computed
		FROM
			sys.tables TB
				INNER JOIN
			sys.columns CL ON
				CL.object_id = TB.object_id
				INNER JOIN
			sys.types TY ON
				TY.user_type_id = CL.user_type_id
		WHERE
			TB.schema_id = SCHEMA_ID(@schema) AND
			TB.name = @name
	) SC
ORDER BY
SC.row_num
/*sda*/

SET @code = 'SELECT
CASE
	WHEN ISNULL(Y.cnt, 0) = 0 THEN NULL
	ELSE CONVERT(DECIMAL(6,2), 100. * X.distinct_values / Y.cnt)
END selectivity,
X.*
FROM (' + @code + ') X CROSS JOIN (SELECT COUNT_BIG(*) cnt FROM ' + QUOTENAME(@schema) + '.' + QUOTENAME(@name) + ') Y
ORDER BY CASE WHEN distinct_values IS NULL THEN -1 ELSE distinct_values END DESC, is_identity DESC, max_length, precision, scale, is_nullable, column_name
/*sda*/
'

SELECT @code code
";
            }
            else if (queryId == QueryId.ScriptInsert)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--DECLARE @schema NVARCHAR(128)
--DECLARE @name NVARCHAR(128)
--SET @schema = ''
--SET @name = ''

DECLARE @code NVARCHAR(MAX)
DECLARE @code1 NVARCHAR(MAX)

SET @code = ''
SET @code1 = ''

SELECT
    @code = @code +
    CASE
	    WHEN SC.row_num = 1 THEN ''
	    ELSE ','
    END +
    CHAR(13) + CHAR(10) + CHAR(9) + QUOTENAME(SC.cl_name),

    @code1 = @code1 +
    CASE
	    WHEN SC.row_num = 1 THEN ''
	    ELSE ','
    END +
    CHAR(13) + CHAR(10) + CHAR(9) + '<' + SC.cl_name + ',,>'
FROM
    (
	    SELECT
		    ROW_NUMBER() OVER (ORDER BY CL.column_id) row_num,
		    CL.name cl_name,
		    CL.is_identity,
		    CL.is_rowguidcol,
		    CL.is_computed
	    FROM
		    sys.tables TB
			    INNER JOIN
		    sys.columns CL ON
			    CL.object_id = TB.object_id
	    WHERE
		    CL.is_identity = 0 AND
		    CL.is_rowguidcol = 0 AND
		    CL.is_computed = 0 AND
		    TB.schema_id = SCHEMA_ID(@schema) AND
		    TB.name = @name
    ) SC
ORDER BY
    SC.row_num
/*sda*/

SET @code = 'INSERT INTO ' + QUOTENAME(@schema) + '.' + QUOTENAME(@name) + '
(' + @code + '
)
VALUES
(' + @code1 + '
)
'

SELECT @code code
";
            }
            else if (queryId == QueryId.ScriptUpdate)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--DECLARE @schema NVARCHAR(128)
--DECLARE @name NVARCHAR(128)
--SET @schema = ''
--SET @name = ''

DECLARE @code NVARCHAR(MAX)

SET @code = ''

SELECT
    @code = @code +
    CASE
	    WHEN SC.row_num = 1 THEN ''
	    ELSE ','
    END +
    CHAR(13) + CHAR(10) + CHAR(9) + QUOTENAME(SC.cl_name) + ' = <' + SC.cl_name + ',,>'
FROM
    (
	    SELECT
		    ROW_NUMBER() OVER (ORDER BY CL.column_id) row_num,
		    CL.name cl_name,
		    CL.is_identity,
		    CL.is_rowguidcol,
		    CL.is_computed
	    FROM
		    sys.tables TB
			    INNER JOIN
		    sys.columns CL ON
			    CL.object_id = TB.object_id
	    WHERE
		    CL.is_identity = 0 AND
		    CL.is_rowguidcol = 0 AND
		    CL.is_computed = 0 AND
		    TB.schema_id = SCHEMA_ID(@schema) AND
		    TB.name = @name
    ) SC
ORDER BY
    SC.row_num
/*sda*/

SET @code = 'UPDATE X SET' + @code + '
FROM
' + CHAR(9) + QUOTENAME(@schema) + '.' + QUOTENAME(@name) + ' X
WHERE
    (0=1)
'

SELECT @code code
";
            }
            else if (queryId == QueryId.ColumnInconsistency)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
;with
cte as
(
	select
		c.name cl_name,
		schema_name(t.SCHEMA_ID) + '.' + t.name tb_name,
		p.name [type],
		c.max_length,
		c.scale,
		c.precision
	from
		sys.tables t
			inner join
		sys.columns c on
			c.object_id = t.object_id
			inner join
		sys.types p on
			p.user_type_id = c.user_type_id
			and p.system_type_id = c.system_type_id
)
select distinct x.*
from 
	cte x
		inner join
	cte y on
		y.tb_name <> x.tb_name
		and y.cl_name = x.cl_name
		and
		(
			y.max_length <> x.max_length
			or y.precision <> x.precision
			or y.scale <> x.scale
			or y.[type] <> x.[type]
		)
order by 1,2
/*sda*/
";
            }
            else if (queryId == QueryId.ExportData)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--DECLARE @schema NVARCHAR(128)
--DECLARE @name NVARCHAR(128)
--SET @schema = ''
--SET @name = ''

SELECT
	CL.name cl_name,
	ISNULL(TY.name, '') cl_type,
	CASE
		WHEN TY.name = 'xml' THEN ''
		WHEN CL.max_length = -1 AND TY.name NOT IN ('geography','geometry') THEN 'max'
		WHEN TY.name IN ('binary','char','varbinary','varchar') THEN CONVERT(VARCHAR, CL.max_length)
		WHEN TY.name IN ('nchar','nvarchar') THEN CONVERT(VARCHAR, CL.max_length/2)
		WHEN TY.name IN ('decimal','numeric') THEN CONVERT(VARCHAR, CL.precision) + ',' + CONVERT(VARCHAR, CL.scale)
		WHEN TY.name IN ('datetime2','datetimeoffset','time') THEN CONVERT(VARCHAR, CL.scale)
		ELSE ''
	END cl_size,
	CASE
		WHEN CL.is_computed = 1 THEN 'computed'
		WHEN CL.is_identity = 1 THEN 'identity'
		WHEN CL.is_rowguidcol = 1 THEN 'rowguidcol'
		ELSE ''
	END cl_info,
    ISNULL(CL.collation_name, '') cl_collation
FROM
	sys.{0}s TB
		INNER JOIN
	sys.columns CL ON
		CL.object_id = TB.object_id
		INNER JOIN
	sys.types TY ON
		TY.user_type_id = CL.user_type_id
WHERE
    TB.schema_id = SCHEMA_ID(@schema) AND
    TB.name = @name
ORDER BY
	CL.column_id
/*sda*/
";
            }
            else if (queryId == QueryId.RunnableCheck)
            {
                //Product Database      Engine  Cmp. Lev.   Supported Cmp. Lev.
                //SQL Server 2017       14      140         140, 130, 120, 110, 100
                //Azure SQL Database    12      130         140, 130, 120, 110, 100
                //SQL Server 2016       13      130         130, 120, 110, 100
                //SQL Server 2014       12      120         120, 110, 100
                //SQL Server 2012       11      110         110, 100, 90
                //SQL Server 2008 R2    10.5    100         100, 90, 80
                //SQL Server 2008       10      100         100, 90, 80
                //SQL Server 2005       9       90          90, 80
                //SQL Server 2000       8       80          80
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CONVERT(INT, compatibility_level) compatibility_level,
	HAS_PERMS_BY_NAME(NULL, NULL, 'VIEW SERVER STATE') view_server_state,
	HAS_PERMS_BY_NAME(name, 'DATABASE', 'VIEW DATABASE STATE') view_database_state
FROM
	sys.databases
WHERE
	name = ISNULL(NULLIF('{0}', ''), DB_NAME())
/*sda*/
";
            }
            else if (queryId == QueryId.FeatureTest)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	ao.object_id
FROM
	sys.all_objects ao
		INNER JOIN
	sys.all_columns ac ON
		ac.object_id = ao.object_id	
WHERE
	ao.name = '{0}' AND
	ao.is_ms_shipped = 1 AND
	ac.name = '{1}'
/*sda*/
";
            }
            else if (queryId == QueryId.MissingFkIndexes)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
IF NOT OBJECT_ID('tempdb..#FK') IS NULL DROP TABLE #FK
IF NOT OBJECT_ID('tempdb..#IX') IS NULL DROP TABLE #IX
IF NOT OBJECT_ID('tempdb..#RC') IS NULL DROP TABLE #RC

--Foreign keys first column
SELECT
	SCHEMA_NAME(FK.schema_id) FK_TABLE_SCHEMA,
	OBJECT_NAME(FK.parent_object_id) FK_TABLE_NAME, 
	CFK.name FK_COLUMN_NAME, 
	SCHEMA_NAME(PK.schema_id) PK_TABLE_SCHEMA,
	OBJECT_NAME(FK.referenced_object_id) PK_TABLE_NAME, 
	CPK.name PK_COLUMN_NAME, 
	FK.name FK_CONSTRAINT_NAME
INTO #FK
FROM
	sys.foreign_keys FK
		INNER JOIN
	sys.foreign_key_columns RL ON
		RL.constraint_object_id = FK.object_id
		INNER JOIN
	sys.columns CFK ON
		CFK.object_id = RL.parent_object_id AND
		CFK.column_id = RL.parent_column_id
		INNER JOIN
	sys.columns CPK ON
		CPK.object_id = RL.referenced_object_id AND
		CPK.column_id = RL.referenced_column_id
		INNER JOIN
	sys.tables PK ON
		PK.object_id = FK.referenced_object_id
WHERE
	RL.constraint_column_id = 1
ORDER BY 
	1, 2, 3, 4, 5, 6
/*sda*/

--Index first column
SELECT DISTINCT
	SCHEMA_NAME(O.schema_id) TABLE_SCHEMA,
	O.name TABLE_NAME,
	C.name COLUMN_NAME
INTO #IX
FROM
	sys.tables O
		INNER JOIN
	sys.indexes I ON
		O.object_id = I.object_id  
		INNER JOIN
	sys.index_columns IC ON
		IC.object_id = I.object_id AND 
		IC.index_id = I.index_id  
		INNER JOIN
	sys.columns C ON
		C.object_id = IC.object_id AND
		C.column_id = IC.column_id  
WHERE
	IC.key_ordinal = 1
ORDER BY
	1, 2, 3
/*sda*/

--Row count
SELECT
	SCHEMA_NAME(O.schema_id) TABLE_SCHEMA,
	O.name TABLE_NAME,
	P.rows TABLE_ROWS,
	I.name INDEX_NAME
INTO #RC
FROM
	sys.tables O
		INNER JOIN
	sys.indexes I ON
		O.object_id = I.object_id  
		INNER JOIN
	sys.partitions P ON
		P.object_id = I.object_id AND
		P.index_id = I.index_id 
WHERE
	I.index_id < 2
ORDER BY
	1, 2
/*sda*/

--Missing indexes
SELECT
	QUOTENAME(FK.FK_TABLE_SCHEMA) + '.' + QUOTENAME(FK.FK_TABLE_NAME) fk_table,
	QUOTENAME(FK.FK_COLUMN_NAME) fk_column,
	FKRC.TABLE_ROWS fk_rows,
	QUOTENAME(FK.PK_TABLE_SCHEMA) + '.' + QUOTENAME(FK.PK_TABLE_NAME) pk_table,
	QUOTENAME(FK.PK_COLUMN_NAME) pk_column,
	PKRC.TABLE_ROWS pk_rows,
	'--' + FK.FK_TABLE_SCHEMA + '.' +
	FK.FK_TABLE_NAME + '.' +
	FK.FK_COLUMN_NAME + ' (' +
	CONVERT(VARCHAR, FKRC.TABLE_ROWS) + ' FK Rows) --> ' +
	FK.PK_TABLE_SCHEMA + '.' +
	FK.PK_TABLE_NAME + '.' +
	FK.PK_COLUMN_NAME + ' (' +
	CONVERT(VARCHAR, PKRC.TABLE_ROWS) + ' PK Rows)' + CHAR(13) + CHAR(10) +
	'CREATE NONCLUSTERED INDEX ' + QUOTENAME('IX_' + FK.FK_CONSTRAINT_NAME) + ' ON ' + QUOTENAME(FK.FK_TABLE_SCHEMA) + '.' + QUOTENAME(FK.FK_TABLE_NAME) + ' (' + QUOTENAME(FK.FK_COLUMN_NAME) + ')' + CHAR(13) + CHAR(10) script
FROM
	#FK FK
		LEFT JOIN
	#RC FKRC ON
		FKRC.TABLE_SCHEMA = FK.FK_TABLE_SCHEMA AND
		FKRC.TABLE_NAME = FK.FK_TABLE_NAME
		LEFT JOIN
	#RC PKRC ON
		PKRC.TABLE_SCHEMA = FK.PK_TABLE_SCHEMA AND
		PKRC.TABLE_NAME = FK.PK_TABLE_NAME
		LEFT JOIN
	#IX IX ON
		IX.TABLE_SCHEMA = FK.FK_TABLE_SCHEMA AND
		IX.TABLE_NAME = FK.FK_TABLE_NAME AND
		IX.COLUMN_NAME = FK.FK_COLUMN_NAME
WHERE
	IX.TABLE_NAME IS NULL
ORDER BY
	FKRC.TABLE_ROWS DESC, FK.FK_TABLE_NAME, PKRC.TABLE_ROWS DESC
/*sda*/
";
            }
            else if (queryId == QueryId.MissingIndexes)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @object_id INT
SELECT @object_id = object_id FROM sys.objects WHERE name = '{0}' AND schema_id = SCHEMA_ID('{1}')
/*sda*/

SELECT
	index_impact,
	user_seeks,
	avg_total_user_cost,
	avg_user_impact,
	QUOTENAME(obj_schema) + '.' + QUOTENAME(obj_name) [object_name],
	'--' + obj_schema + '.' + obj_name + ' (index impact: ' + CONVERT(VARCHAR, index_impact) + ')' + CHAR(13) + CHAR(10) +
	'CREATE NONCLUSTERED INDEX ' + QUOTENAME('IX_AT_' + obj_schema + '_' + obj_name + '_' + REPLACE(CONVERT(VARCHAR,CHECKSUM(idx_columns + ISNULL(idx_included_columns, ''))),'-','0')) + ' ' +
	'ON ' + QUOTENAME(obj_schema) + '.' + QUOTENAME(obj_name) + ' ' +
	'(' + idx_columns + ')' +
	ISNULL(' INCLUDE (' +  idx_included_columns + ')', '') + CHAR(13) + CHAR(10) script
FROM
	(
		SELECT
			CONVERT(DECIMAL(18,2),MAX(index_impact)) index_impact,
			MAX(user_seeks) user_seeks,
			CONVERT(DECIMAL(18,2),MAX(avg_total_user_cost)) avg_total_user_cost,
			MAX(avg_user_impact) avg_user_impact,
			obj_schema,
			obj_name,
			idx_columns,
			idx_included_columns
		FROM
			(
				SELECT
					migs.user_seeks * migs.avg_total_user_cost * migs.avg_user_impact / 100. index_impact,
					migs.user_seeks,
					migs.avg_total_user_cost,
					migs.avg_user_impact,
					SCHEMA_NAME(obj.schema_id) COLLATE database_default obj_schema,
					obj.name COLLATE database_default obj_name,
					ISNULL(mid.equality_columns, '') + CASE WHEN mid.equality_columns IS NOT NULL AND mid.inequality_columns IS NOT NULL THEN ', ' ELSE '' END + ISNULL(mid.inequality_columns, '') COLLATE database_default idx_columns,
					mid.included_columns COLLATE database_default idx_included_columns
				FROM
					sys.dm_db_missing_index_details mid
						INNER JOIN
					sys.dm_db_missing_index_groups mig ON
						mig.index_handle = mid.index_handle
						INNER JOIN
					sys.dm_db_missing_index_group_stats migs ON
						migs.group_handle = mig.index_group_handle
						INNER JOIN
					sys.objects obj ON
						obj.object_id = mid.object_id
				WHERE
					obj.object_id = ISNULL(@object_id, obj.object_id) AND
					mid.database_id = DB_ID() AND
					NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = obj.object_id AND ep.name = 'microsoft_database_tools_support') AND
					obj.is_ms_shipped = 0
			) X
		GROUP BY
			obj_schema,
			obj_name,
			idx_columns,
			idx_included_columns
	) Y
ORDER BY
	index_impact DESC
/*sda*/
";
            }
            else if (queryId == QueryId.IndexesStatus)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @object_id INT
DECLARE @db_id INT = DB_ID()
SELECT @object_id = object_id FROM sys.objects WHERE name = '{1}' AND schema_id = SCHEMA_ID('{2}')
SELECT * INTO #IUS FROM sys.dm_db_index_usage_stats
SELECT * INTO #IOS FROM sys.dm_db_index_operational_stats(@db_id, @object_id, NULL, NULL)
{3}SELECT CONVERT(INT, NULL) object_id, CONVERT(INT, NULL) index_id, CONVERT(INT, NULL) partition_number, CONVERT(NVARCHAR, NULL) alloc_unit_type_desc, CONVERT(BIGINT, NULL) record_count, CONVERT(FLOAT, NULL) avg_fragmentation_in_percent, CONVERT(TINYINT, NULL) index_level INTO #IPS
{4}SELECT object_id, index_id, partition_number, alloc_unit_type_desc, record_count, avg_fragmentation_in_percent, index_level INTO #IPS FROM sys.dm_db_index_physical_stats(@db_id, @object_id, NULL, NULL, '{0}')
SELECT
	QUOTENAME(SCHEMA_NAME(O.schema_id)) + '.' + QUOTENAME(O.name) [object_name],
	object_type = O.type_desc,
	I.name index_name,
	I.type_desc index_type,
	PS.index_level,
	AU.type_desc allocation_type,
	AU.used_pages * 8.0 / 1024.0 used_MB,
	ISNULL(PS.record_count, P.[rows]) record_count,
	CONVERT(DECIMAL(6, 2), CASE WHEN I.type = 0 AND '{0}' <> 'DETAILED' THEN NULL ELSE PS.avg_fragmentation_in_percent END) fragment_percent,
	ISNULL(US.user_seeks, 0) + ISNULL(US.user_scans, 0) + ISNULL(US.user_lookups, 0) - ISNULL(US.user_updates, 0) read_vs_write,
	ISNULL(US.user_seeks, 0) + ISNULL(US.user_scans, 0) + ISNULL(US.user_lookups, 0) total_reads,
	(SELECT CONVERT(VARCHAR, MAX(c),120) FROM (VALUES(US.last_user_seek),(US.last_user_scan),(US.last_user_lookup)) T (c)) last_read,
	US.user_seeks,
	US.user_scans,
	US.user_lookups,
	US.user_updates,
	CONVERT(VARCHAR, US.last_user_seek, 120) last_user_seek,
	CONVERT(VARCHAR, US.last_user_scan, 120) last_user_scan,
	CONVERT(VARCHAR, US.last_user_lookup, 120) last_user_lookup,
	CONVERT(VARCHAR, US.last_user_update, 120) last_user_update,
	CONVERT(VARCHAR, STATS_DATE(I.object_id, I.index_id), 120) statistics_date,
	{5}P.data_compression_desc data_compression,
	FG.name [filegroup_name],	
	P.partition_number,
	DS.type_desc data_space_type,
	DS.name data_space_name,
	PF.name partition_function,
	PR.value partition_value,
	OS.leaf_allocation_count,
	OS.nonleaf_allocation_count,
	OS.row_lock_count,
	OS.row_lock_wait_count,
	OS.page_lock_count,
	OS.page_lock_wait_count,
	CASE
		WHEN I.type_desc <> 'HEAP' AND AU.type_desc = 'IN_ROW_DATA' AND ISNULL(PS.index_level, 0) = 0 THEN
'ALTER INDEX ' + QUOTENAME(I.name) + ' ON ' + QUOTENAME(DB_NAME()) + '.' + QUOTENAME(SCHEMA_NAME(O.schema_id)) + '.' + QUOTENAME(O.name) + ' REORGANIZE --PARTITION = ' + CONVERT(VARCHAR, P.partition_number)
		WHEN I.type_desc = 'HEAP' AND AU.type_desc = 'IN_ROW_DATA' THEN
'CREATE CLUSTERED INDEX ' + QUOTENAME('IH_' + O.name) + ' ON ' + QUOTENAME(DB_NAME()) + '.' + QUOTENAME(SCHEMA_NAME(O.schema_id)) + '.' + QUOTENAME(O.name) + ' (' + QUOTENAME(C.name) + ')' + CHAR(13) + CHAR(10)
		ELSE ''
	END script,
	CASE
		WHEN I.type_desc <> 'HEAP' AND AU.type_desc = 'IN_ROW_DATA' AND ISNULL(PS.index_level, 0) = 0 THEN
'--' + SCHEMA_NAME(O.schema_id) + '.' + O.name + ' fragment impact (record_count * fragment_percent): ' + CONVERT(VARCHAR, CONVERT(BIGINT, ISNULL(PS.record_count, P.[rows]) * ISNULL(PS.avg_fragmentation_in_percent, 0) / 100))
		WHEN I.type_desc = 'HEAP' AND AU.type_desc = 'IN_ROW_DATA' THEN
'--Check HEAP fragmentation, eventually defrag creating a clustered index (most selective column could be appropriate): SELECT partition_number,avg_fragmentation_in_percent FROM sys.dm_db_index_physical_stats(@db_id, OBJECT_ID(N''[' + DB_NAME() + '].[' + SCHEMA_NAME(O.schema_id) + '].[' + O.name + ']''), NULL, NULL, ''DETAILED'') WHERE index_type_desc = ''HEAP'' AND alloc_unit_type_desc = ''IN_ROW_DATA'''
		ELSE ''
	END comment
FROM
	sys.objects O
		INNER JOIN
	(SELECT object_id, MIN(column_id) column_id FROM sys.columns GROUP BY object_id) FC ON
		FC.object_id = O.object_id
		INNER JOIN
	sys.columns C ON
		C.object_id = O.object_id AND
		C.column_id = FC.column_id
		INNER JOIN
	sys.indexes I ON
		I.object_id = O.object_id
		INNER JOIN
	sys.partitions P ON
		P.object_id = I.object_id AND
		P.index_id = I.index_id
		INNER JOIN
	sys.allocation_units AU ON
		AU.container_id = P.partition_id
		LEFT JOIN
	#IPS PS ON
		PS.object_id = O.object_id AND
		PS.index_id = I.index_id AND
		PS.partition_number = P.partition_number AND
		PS.alloc_unit_type_desc = AU.type_desc COLLATE database_default
		LEFT JOIN
	#IUS US ON
		US.database_id = @db_id AND
		US.object_id = O.object_id AND
		US.index_id = I.index_id
		LEFT JOIN
	#IOS OS ON
		OS.object_id = O.object_id AND
		OS.index_id = I.index_id AND
		OS.partition_number = P.partition_number
		LEFT JOIN
	sys.data_spaces DS on
		DS.data_space_id = I.data_space_id
		LEFT JOIN
	sys.partition_schemes SC ON
		SC.data_space_id = I.data_space_id
		LEFT JOIN
	sys.partition_functions PF ON
		PF.function_id = SC.function_id
		LEFT JOIN
	sys.partition_range_values PR ON
		PR.function_id = SC.function_id AND
		PR.boundary_id = P.partition_number
		LEFT JOIN
	sys.destination_data_spaces SP ON
		SP.partition_scheme_id = SC.data_space_id AND
		SP.destination_id = P.partition_number
		LEFT JOIN
	sys.filegroups FG ON
		FG.data_space_id = CASE WHEN DS.[type] <> 'PS' THEN DS.data_space_id ELSE SP.data_space_id END
WHERE
	O.object_id = ISNULL(@object_id, O.object_id) AND
	O.is_ms_shipped = 0 AND
	NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = O.object_id AND ep.name = 'microsoft_database_tools_support')
ORDER BY
	[object_name],
	index_name,
	P.partition_number,
	PS.index_level
OPTION(RECOMPILE, MAXDOP 1)
/*sda*/
";
            }
            else if (queryId == QueryId.ExistingIndexes)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET ARITHABORT ON

SELECT
	object_schema,
	[object_name],
	index_name,
	'CREATE' + cn_type + ix_type + ' INDEX ' + QUOTENAME(index_name) + ' ON ' + QUOTENAME(object_schema) + '.' + QUOTENAME([object_name]) + ' (' +
	CASE
		WHEN IX.type IN (5, 6) THEN CONVERT(XML,'<r>' + SUBSTRING(inc_columns, 1, LEN(inc_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)')
		ELSE CONVERT(XML,'<r>' + SUBSTRING(idx_columns, 1, LEN(idx_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)')
	END
	+ ')' +
	CASE
		WHEN ISNULL(inc_columns, '') = '' OR IX.type NOT IN (1, 2) THEN ''
		ELSE ' INCLUDE (' + CONVERT(XML,'<r>' + SUBSTRING(inc_columns, 1, LEN(inc_columns)-1) + '</r>').value('r[1]','NVARCHAR(max)') + ')'
	END
	{0}+ CASE WHEN IX.filter_definition IS NOT NULL THEN ' WHERE ' + IX.filter_definition ELSE '' END
	+ CASE
		WHEN idk + flf + pad <> '' THEN	REPLACE(' WITH' + idk + flf + pad, 'WITH,', 'WITH')
		ELSE ''
	END script
FROM
	(
		SELECT
			O.object_id,
			SCHEMA_NAME(O.schema_id) object_schema,
			O.name [object_name],
			I.name index_name,
			(
				SELECT
					QUOTENAME(C.name) + CASE WHEN IC.is_descending_key = 1 THEN ' DESC,' ELSE ',' END
				FROM
					sys.index_columns IC
						inner join
					sys.columns C on
						C.object_id = IC.object_id and
						C.column_id = IC.column_id
				WHERE
					IC.is_included_column = 0 AND
					IC.object_id = I.object_id and
					IC.index_id = I.index_id
				ORDER BY IC.key_ordinal
				FOR XML PATH('')
			) idx_columns,
			(
				SELECT QUOTENAME(C.name) + ','
				FROM
					sys.index_columns IC
						inner join
					sys.columns C on
						C.object_id = IC.object_id and
						C.column_id = IC.column_id
				WHERE
					IC.is_included_column = 1 AND
					IC.object_id = I.object_id and
					IC.index_id = I.index_id
				ORDER BY IC.key_ordinal
				FOR XML PATH('')
			) inc_columns,
			CASE I.is_unique WHEN 0 THEN '' ELSE ' UNIQUE' END cn_type,
			' ' + I.type_desc COLLATE DATABASE_DEFAULT ix_type,
			CASE I.ignore_dup_key WHEN 0 THEN '' ELSE ', IGNORE_DUP_KEY' END idk,
			CASE I.fill_factor WHEN 0 THEN '' ELSE ', FILLFACTOR = ' + CONVERT(VARCHAR, I.fill_factor) END flf,
			CASE I.is_padded WHEN 0 THEN '' ELSE ', PAD_INDEX' END pad,
			{0}I.filter_definition,
			I.type
		FROM
			sys.objects O
				inner join
			sys.indexes I on
				I.object_id = O.object_id
		WHERE
			NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = O.object_id AND ep.name = 'microsoft_database_tools_support') AND
			O.is_ms_shipped = 0 AND
			I.is_hypothetical = 0 AND
			I.type IN (1, 2, 5, 6) AND
			I.is_primary_key = 0 AND
            I.is_unique_constraint = 0
	) IX
ORDER BY
	object_schema,
	[object_name],
	ix_type,
	index_name
/*sda*/
";
            }
            else if (queryId == QueryId.DropTemplate)
            {
                return "IF NOT OBJECT_ID(N'[{0}].[{1}]') IS NULL\r\n\tDROP {2} [{3}].[{4}]";
            }
            else if (queryId == QueryId.DropSchemaTemplate)
            {
                return "IF EXISTS(SELECT * FROM sys.schemas WHERE name = N'{0}')\r\n\tDROP {1} [{2}]";
            }
            else if (queryId == QueryId.DropTypeTemplate)
            {
                return "IF EXISTS(SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'{0}' AND ss.name = N'{1}')\r\n\tDROP {2} [{3}].[{4}]";
            }
            else if (queryId == QueryId.TableStatistics)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @object_id INT
SELECT @object_id = object_id FROM sys.objects WHERE name = '{1}' AND schema_id = SCHEMA_ID('{2}')
/*sda*/

SELECT 
	QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' +	QUOTENAME(t.name) table_name,
	i.name index_name,
	s.name stat_name,
	CONVERT(VARCHAR, STATS_DATE(s.object_id, s.stats_id), 120) stat_date,
	CONVERT(BIT, CASE WHEN i.index_id is null THEN 0 ELSE 1 END) index_stat,
	s.auto_created,
	s.user_created,	
	{0}s.has_filter,
	'CREATE STATISTICS ' + QUOTENAME(s.name) + ' ON ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name) +	' (' +
	SUBSTRING(
		(
			SELECT
				',' + QUOTENAME(c.name)
			FROM
				sys.stats_columns sc
					INNER JOIN
				sys.columns c ON
					c.object_id = sc.object_id AND
					c.column_id = sc.column_id
			WHERE
				sc.object_id = s.object_id AND
				sc.stats_id = s.stats_id
			ORDER BY
				sc.stats_column_id
			FOR XML PATH('')
		),
		2,
		4096
	) + ')'
	{0}+ CASE s.has_filter WHEN 1 THEN ' WHERE ' + s.filter_definition ELSE '' END
	script
FROM
	sys.tables t
		inner join
	sys.stats s on
		s.object_id = t.object_id
		left join
	sys.indexes i on
		i.object_id = s.object_id and
		i.index_id = s.stats_id
WHERE
	t.object_id = ISNULL(@object_id, t.object_id) AND
	t.is_ms_shipped = 0 AND
	NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = t.object_id AND ep.name = 'microsoft_database_tools_support')
ORDER BY
	1, 2, 3
/*sda*/
";
            }
            else if (queryId == QueryId.TableMBytes)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @TMP AS TABLE
(
	table_schema NVARCHAR(128),
	table_name NVARCHAR(256),
	record_count VARCHAR(50),
	reserved VARCHAR(50),
	data VARCHAR(50),
	index_size VARCHAR(50),
	unused VARCHAR(50)
)

DECLARE @table_name NVARCHAR(256)
DECLARE @name NVARCHAR(128)
DECLARE @object_id INT
DECLARE @schema_id INT

SELECT @object_id = -1
WHILE 1 = 1
BEGIN
	SELECT TOP 1
		@name = T.name,
		@object_id = T.object_id,
		@schema_id = T.schema_id
	FROM
		sys.tables T
	WHERE
		NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties ep WHERE ep.major_id = T.object_id AND ep.name = 'microsoft_database_tools_support') AND
		T.is_ms_shipped = 0 AND
		T.object_id > @object_id
	ORDER BY
		object_id
/*sda*/

	IF @@ROWCOUNT = 0
		BREAK

	SET @table_name = QUOTENAME(SCHEMA_NAME(@schema_id)) + '.' + QUOTENAME(@name)

	INSERT INTO @TMP
	(
		table_name,
		record_count,
		reserved,
		data,
		index_size,
		unused
	)
	EXEC sys.sp_spaceused @table_name
/*sda*/

	UPDATE @TMP SET
		table_name = @table_name,
		table_schema = SCHEMA_NAME(@schema_id),
		reserved = REPLACE(reserved, ' KB', ''),
		data = REPLACE(data, ' KB', ''),
		index_size = REPLACE(index_size, ' KB', ''),
		unused = REPLACE(unused, ' KB', '')
	WHERE
		table_schema IS NULL
/*sda*/
END

SELECT
	T.table_name,
	CASE
		WHEN CONVERT(FLOAT, S.reserved) = 0 THEN CONVERT(DECIMAL(5, 2), 0)
		ELSE CONVERT(DECIMAL(5, 2), 100. * CONVERT(FLOAT, T.reserved) / CONVERT(FLOAT, S.reserved))
	END [weight %],
	CONVERT(bigint, T.record_count) record_count,
	CONVERT(DECIMAL(10, 4), CONVERT(FLOAT, T.reserved) / 1024.) reserved,
	CONVERT(DECIMAL(10, 4), CONVERT(FLOAT, T.data) / 1024.) data,
	CONVERT(DECIMAL(10, 4), CONVERT(FLOAT, T.index_size) / 1024.) index_size,
	CONVERT(DECIMAL(10, 4), CONVERT(FLOAT, T.unused) / 1024.) unused,
	'UPDATE STATISTICS ' + T.table_name script
FROM
	@TMP T
		CROSS JOIN
	(SELECT SUM(CONVERT(FLOAT, reserved)) reserved FROM @TMP) S
ORDER BY
	reserved DESC,
	record_count DESC
/*sda*/
";
            }
            else if (queryId == QueryId.DatabaseFiles)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
CREATE TABLE #DF
(
	database_name sysname,
	group_name sysname,
	group_type nvarchar(60),
	default_group nvarchar(1),
	file_id int,
	[file_name] sysname,
	file_type nvarchar(60),
	size_MB int,
	free_MB int,
	max_size_MB int,
	growth_value int,
	growth_type nvarchar(60),
	[state] nvarchar(60),
	[read_only] nvarchar(1),
	physical_name nvarchar(260),
	script nvarchar(max)
)
BEGIN TRY
INSERT INTO #DF
EXEC sp_MSforeachdb 'USE [?]
SELECT
	''?'' database_name,
	ISNULL(DS.name, '''') group_name,
	ISNULL(DS.type_desc, '''') group_type,
	CASE
		WHEN DS.is_default = 0 THEN ''N''
		WHEN DS.is_default = 1 THEN ''Y''
		ELSE ''''
	END default_group,
	DF.file_id,
	DF.name [file_name],
	DF.type_desc file_type,
	DF.size / 128 size_MB,
	DF.size / 128 - CONVERT(INT, FILEPROPERTY(DF.name, ''SpaceUsed'')) / 128 free_MB,
	CASE DF.max_size 
		WHEN 0 THEN DF.size / 128
		WHEN -1 THEN NULL
		ELSE DF.max_size / 128
	END max_size_MB,
	CASE DF.is_percent_growth
		WHEN 0 THEN DF.growth / 128
		ELSE DF.growth
	END growth_value,
	CASE 
		WHEN DF.growth = 0 THEN ''No growth''
		WHEN DF.growth > 0 AND DF.is_percent_growth = 0 THEN ''MBytes''
		ELSE ''%''
	END growth_type,
	DF.state_desc [state],
	CASE WHEN DF.is_read_only = 0 AND DB.is_read_only = 0 THEN ''N'' ELSE ''Y'' END [read_only],
	DF.physical_name,
	CASE WHEN DF.[state] = 0 AND DF.is_read_only = 0 AND DB.is_read_only = 0 AND DF.file_id = 1 THEN ''DBCC CHECKDB (N'''''' + DB.name + '''''', NOINDEX) WITH NO_INFOMSGS, PHYSICAL_ONLY'' ELSE '''' END check_script
FROM
	sys.database_files DF
		LEFT JOIN
	sys.data_spaces DS ON
		DS.data_space_id = DF.data_space_id
		LEFT JOIN
	sys.databases DB ON
		DB.name = ''?''
WHERE
	(DF.[type] = 0 OR DB.source_database_id IS NULL)
/*sda*/
'
END TRY
BEGIN CATCH
END CATCH
SELECT * FROM #DF WHERE database_name = ISNULL(NULLIF(CONVERT(NVARCHAR(160), N'{0}'), N''), database_name) ORDER BY 1, 7 DESC, 6, 2
";
            }
            else if (queryId == QueryId.ServerConfigurations)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	*
FROM
	sys.configurations
/*sda*/
";
            }
            else if (queryId == QueryId.ServerProperties)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @PR AS TABLE(category VARCHAR(50), property_name VARCHAR(100), property_value NVARCHAR(256))
BEGIN TRY
INSERT INTO @PR
SELECT
	category,
	property_name,
	ISNULL(CONVERT(NVARCHAR(256),
		CASE
			WHEN property_name = 'FullVersion' THEN @@VERSION
			WHEN property_name = 'EngineEdition' THEN
				CASE SERVERPROPERTY(property_name)
					WHEN 1 THEN 'Personal'
					WHEN 2 THEN 'Standard'
					WHEN 3 THEN 'Enterprise'
					WHEN 4 THEN 'Express'
					WHEN 5 THEN 'SQL Azure'
				END
			WHEN property_name = 'HadrManagerStatus' THEN
				CASE SERVERPROPERTY(property_name)
					WHEN 0 THEN 'Not started, pending communication'
					WHEN 1 THEN 'Started and running'
					WHEN 2 THEN 'Not started and failed'
				END
			WHEN property_name IN ('FilestreamConfiguredLevel','FilestreamEffectiveLevel') THEN
				CASE SERVERPROPERTY(property_name)
					WHEN 0 THEN 'Disabled'
					WHEN 1 THEN 'Enabled for T-SQL'
					WHEN 2 THEN 'Enabled for T-SQL and Win32'
				END
			ELSE SERVERPROPERTY(property_name)
		END
	), '') property_value
FROM
	(
		--SERVERPROPERTY
		SELECT 'BuildClrVersion' property_name, 'Version' category
		UNION ALL SELECT 'Collation', 'Configuration'
		UNION ALL SELECT 'ComputerNamePhysicalNetBIOS', 'Name'
		UNION ALL SELECT 'Edition', 'Version'
		UNION ALL SELECT 'EngineEdition', 'Version'
		UNION ALL SELECT 'HadrManagerStatus', 'Status'
		UNION ALL SELECT 'InstanceName', 'Name'
		UNION ALL SELECT 'IsClustered', 'Configuration'
		UNION ALL SELECT 'IsFullTextInstalled', 'Configuration'
		UNION ALL SELECT 'IsHadrEnabled', 'Configuration'
		UNION ALL SELECT 'IsIntegratedSecurityOnly', 'Configuration'
		UNION ALL SELECT 'IsLocalDB', 'Configuration'
		UNION ALL SELECT 'IsSingleUser', 'Status'
		UNION ALL SELECT 'MachineName', 'Configuration'
		UNION ALL SELECT 'ProcessID', 'Status'
		UNION ALL SELECT 'ProductVersion', 'Version'
		UNION ALL SELECT 'ProductLevel', 'Version'
		UNION ALL SELECT 'ServerName', 'Name'
		UNION ALL SELECT 'FilestreamShareName', 'Configuration'
		UNION ALL SELECT 'FilestreamConfiguredLevel', 'Configuration'
		UNION ALL SELECT 'FilestreamEffectiveLevel', 'Status'
		--global variable
		UNION ALL SELECT 'FullVersion', 'Version'
	) SP
END TRY
BEGIN CATCH END CATCH
BEGIN TRY
INSERT INTO @PR
SELECT
   'SystemInfo' category, 
   property_name, 
   property_value 
FROM 
   (
		SELECT
			[logical_cpu] = CONVERT(NVARCHAR(256), cpu_count), 
			[hyperthread_ratio] = CONVERT(NVARCHAR(256), hyperthread_ratio), 
			[physical_cpu] = CONVERT(NVARCHAR(256), cpu_count/hyperthread_ratio), 
			{0}[physical_memory_MB] = CONVERT(NVARCHAR(256), physical_memory_in_bytes/1048576),
			{1}[physical_memory_MB] = CONVERT(NVARCHAR(256), physical_memory_kb/1024),
			[sqlserver_start_time] = CONVERT(NVARCHAR(256), sqlserver_start_time, 120)
		FROM sys.dm_os_sys_info
   ) OI
UNPIVOT
	(
		property_value 
		FOR property_name IN ([logical_cpu], [hyperthread_ratio], [physical_cpu], [physical_memory_MB])
	) unpvt
END TRY
BEGIN CATCH END CATCH
SELECT * FROM @PR ORDER BY 1,2
/*sda*/
";
            }
            else if (queryId == QueryId.ServerRoles)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	RL.name role_name,
	MB.name member_name,
	'EXEC master..sp_addsrvrolemember @loginame = N''' + MB.name + ''', @rolename = N''' + RL.name + '''' script
FROM
	sys.server_role_members SR
		INNER JOIN
	sys.server_principals RL ON
		RL.principal_id = SR.role_principal_id
		INNER JOIN
	sys.server_principals MB ON
		MB.principal_id = SR.member_principal_id
ORDER BY 1,2
/*sda*/
";
            }
            else if (queryId == QueryId.Processes)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CASE
		WHEN ER.er_total_elapsed_time > 0 THEN
			RIGHT('00000' + CONVERT(VARCHAR, (ER.er_total_elapsed_time - ER.er_total_elapsed_time % 3600000) / 3600000), 5) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (ER.er_total_elapsed_time - ER.er_total_elapsed_time % 60000) % 3600000 / 60000), 2) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (ER.er_total_elapsed_time - ER.er_total_elapsed_time % 1000) % 60000 / 1000), 2) + '.' +
			RIGHT('000' + CONVERT(VARCHAR, ER.er_total_elapsed_time % 1000), 3)
		WHEN SP.sp_total_elapsed_time > 0 THEN
			RIGHT('00000' + CONVERT(VARCHAR, (SP.sp_total_elapsed_time - SP.sp_total_elapsed_time % 3600000) / 3600000), 5) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (SP.sp_total_elapsed_time - SP.sp_total_elapsed_time % 60000) % 3600000 / 60000), 2) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (SP.sp_total_elapsed_time - SP.sp_total_elapsed_time % 1000) % 60000 / 1000), 2) + '.' +
			RIGHT('000' + CONVERT(VARCHAR, SP.sp_total_elapsed_time % 1000), 3)
		ELSE '00000:00:00.000'
	END prc_h_m_s_ms,
	CASE
		WHEN AT.at_total_elapsed_time > 0 THEN
			RIGHT('00000' + CONVERT(VARCHAR, (AT.at_total_elapsed_time - AT.at_total_elapsed_time % 3600000) / 3600000), 5) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (AT.at_total_elapsed_time - AT.at_total_elapsed_time % 60000) % 3600000 / 60000), 2) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (AT.at_total_elapsed_time - AT.at_total_elapsed_time % 1000) % 60000 / 1000), 2) + '.' +
			RIGHT('000' + CONVERT(VARCHAR, AT.at_total_elapsed_time % 1000), 3)
	END trn_h_m_s_ms,
	SP.spid session_id,
	NULLIF(ER.blocking_session_id, 0) blocking_session_id,
	CASE
		WHEN ER.[status] = 'background' THEN ER.[status]
		ELSE ES.[status]
	END [status],
	DB_NAME(SP.dbid) database_name,
	ER.command,
	ET.[text] sql_task,
	CASE
		WHEN ER.statement_end_offset - ER.statement_start_offset > 0 THEN
			SUBSTRING
			(
				ET.text,
				(ER.statement_start_offset / 2) + 1, 
				(CASE ER.statement_end_offset WHEN - 1 THEN DATALENGTH(ET.text) ELSE ER.statement_end_offset END - ER.statement_start_offset) / 2 + 1
			)
		ELSE ET.text
	END sql_statement,
	[program_name] = CASE WHEN JN.[program_name] IS NOT NULL THEN JN.[program_name] ELSE ES.[program_name] END,
	ES.[host_name],
	ISNULL(NULLIF(ES.login_name, ''), ES.original_login_name) login_name,
	ER.cpu_time,
	ER.reads,
	ER.writes,
	ER.logical_reads,
	ER.wait_time,
	ER.wait_type,
	ER.last_wait_type,
	ER.wait_resource,
	ES.total_elapsed_time,
	ES.cpu_time total_cpu_time,
	ES.reads total_reads,
	ES.writes total_writes,
	ES.logical_reads total_logical_reads,
	SP.open_tran open_transaction_count,
	CASE ISNULL(ER.transaction_isolation_level, ES.transaction_isolation_level)
		WHEN 0 THEN 'Unspecified'
		WHEN 1 THEN 'ReadUncomitted'
		WHEN 2 THEN 'ReadCommitted'
		WHEN 3 THEN 'Repeatable'
		WHEN 4 THEN 'Serializable'
		WHEN 5 THEN 'Snapshot'
	END transaction_isolation,
	AT.name transaction_name,
	CASE AT.transaction_type
		WHEN 1 THEN 'read/write'
		WHEN 2 THEN 'read-only'
		WHEN 3 THEN 'system'
		WHEN 4 THEN 'distributed'
	END transaction_type,
	CASE AT.transaction_state
		WHEN 0 THEN 'not completely initialized'
		WHEN 1 THEN 'initialized but not started'
		WHEN 2 THEN 'active'
		WHEN 3 THEN 'read-only transaction has ended'
		WHEN 4 THEN 'commit initiated'
		WHEN 5 THEN 'prepared and waiting resolution'
		WHEN 6 THEN 'committed'
		WHEN 7 THEN 'being rolled back'
		WHEN 8 THEN 'been rolled back'
	END transaction_state,
	ES.last_request_start_time,
	ES.last_request_end_time,
	ES.login_time,
	ES.[language],
	ES.date_format,
	ES.date_first
FROM
	(
		SELECT
			*,
			CASE
				WHEN DATEDIFF(D, last_batch, GETDATE()) > 24 THEN DATEDIFF(S, last_batch, GETDATE()) * CONVERT(BIGINT, 1000)
				ELSE DATEDIFF(MS, last_batch, GETDATE()) * CONVERT(BIGINT, 1)
			END sp_total_elapsed_time,
			RN = ROW_NUMBER() OVER (PARTITION BY spid, request_id ORDER BY sql_handle DESC)
		FROM
			sys.sysprocesses
		WHERE
			hostprocess > '' AND
			(
				status <> 'sleeping' OR
				open_tran > 0
			)
	) SP
		LEFT LOOP JOIN
	sys.dm_exec_sessions ES ON
		ES.session_id = SP.spid
		LEFT LOOP JOIN
	(
		SELECT
			*,
			CASE
				WHEN DATEDIFF(D, start_time, GETDATE()) > 24 THEN DATEDIFF(S, start_time, GETDATE()) * CONVERT(BIGINT, 1000)
				ELSE DATEDIFF(MS, start_time, GETDATE()) * CONVERT(BIGINT, 1)
			END er_total_elapsed_time --fix total_elapsed_time bug
		FROM
			sys.dm_exec_requests
	) ER ON
		ER.session_id = SP.spid
		AND ER.request_id = SP.request_id
		LEFT LOOP JOIN 
	(
		SELECT
			*,
			CASE
				WHEN DATEDIFF(D, transaction_begin_time, GETDATE()) > 24 THEN DATEDIFF(S, transaction_begin_time, GETDATE()) * CONVERT(BIGINT, 1000)
				ELSE DATEDIFF(MS, transaction_begin_time, GETDATE()) * CONVERT(BIGINT, 1)
			END at_total_elapsed_time
		FROM
			sys.dm_tran_active_transactions
	) AT ON
		AT.transaction_id = ER.transaction_id
		OUTER APPLY
	sys.dm_exec_sql_text(COALESCE(ER.sql_handle, NULLIF(SP.sql_handle, 0x00))) ET
		OUTER APPLY
	(
		SELECT TOP 1
			[program_name] = 'SQLAgent - TSQL JobStep (Job [' + j.name + '] - ' + SUBSTRING(ES.[program_name], 67, 500)
		FROM
			msdb..sysjobs j
		WHERE
			ES.[program_name] LIKE '%' + sys.fn_varbintohexstr(j.job_id) + '%' AND
			ES.[program_name] LIKE 'SQLAgent - TSQL JobStep (Job 0x%)'
	) JN
WHERE
	SP.RN = 1
	{0}AND (ET.[text] NOT LIKE '%/*sda*/%' OR ET.[text] IS NULL)
	{1}AND DB_NAME(SP.dbid) = '{2}'
ORDER BY
	1 DESC,
	2
";
            }
            else if (queryId == QueryId.DatabasePermissions)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CASE
		WHEN P.class IN (0, 3) THEN P.class_desc
		ELSE O.type_desc
	END type_desc,
	CASE
		WHEN P.class IN (0, 3) THEN ''
		ELSE SCHEMA_NAME(O.schema_id)
	END object_schema,
	CASE
		WHEN P.class = 3 THEN SCHEMA_NAME(P.major_id)
		WHEN P.class = 1 THEN OBJECT_NAME(P.major_id)
		ELSE ''
	END object_name,
	ISNULL(C.name, '') column_name,
	P.class_desc,
	USER_NAME(P.grantee_principal_id) grantee,
	P.permission_name,
	P.state_desc,
	USER_NAME(P.grantor_principal_id) grantor,
	CASE P.state
		WHEN 'W' THEN 'GRANT'
		ELSE P.state_desc
	END + ' ' +
	CASE
		WHEN P.class = 0 THEN P.permission_name + ' TO ' + QUOTENAME(USER_NAME(P.grantee_principal_id)) COLLATE database_default
		WHEN P.class = 3 THEN P.permission_name + ' ON SCHEMA::' + QUOTENAME(SCHEMA_NAME(P.major_id)) + ' TO ' + QUOTENAME(USER_NAME(P.grantee_principal_id))
		WHEN P.minor_id <> 0 THEN P.permission_name + ' ON ' + QUOTENAME(SCHEMA_NAME(O.schema_id)) + '.' + QUOTENAME(O.name) + ' (' + QUOTENAME(C.name) + ') TO ' + QUOTENAME(USER_NAME(P.grantee_principal_id))
		ELSE P.permission_name + ' ON ' + QUOTENAME(SCHEMA_NAME(O.schema_id)) + '.' + QUOTENAME(O.name) + ' TO ' + QUOTENAME(USER_NAME(P.grantee_principal_id))
	END +
	CASE P.state
		WHEN 'W' THEN ' WITH GRANT OPTION'
		ELSE ''
	END script
FROM
	sys.database_permissions P
		LEFT JOIN
	sys.all_objects O ON
		O.object_id = P.major_id
		LEFT JOIN
	sys.all_columns C ON
		C.object_id = P.major_id AND
		C.column_id = P.minor_id
WHERE
	NOT
	(
		P.class = 1 AND
		(
			EXISTS (SELECT TOP 1 1 FROM sys.extended_properties EP WHERE EP.major_id = O.object_id AND EP.name = 'microsoft_database_tools_support') OR
			O.is_ms_shipped = 1
		)
	)
ORDER BY
	1,2,3,4,5,6,7,8
/*sda*/
";
            }
            else if (queryId == QueryId.DatabaseRoles)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	RL.name role_name,
	MB.name member_name,
	'EXEC ' + QUOTENAME(DB_NAME()) + '..sp_addrolemember @membername = N''' + MB.name + ''', @rolename = N''' + RL.name + '''' script
FROM
	sys.database_role_members SR
		INNER JOIN
	sys.database_principals RL ON
		RL.principal_id = SR.role_principal_id
		INNER JOIN
	sys.database_principals MB ON
	MB.principal_id = SR.member_principal_id
ORDER BY 1,2
/*sda*/
";
            }
            else if (queryId == QueryId.Jobs)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @xp_results TABLE
(
	job_id UNIQUEIDENTIFIER NOT NULL,
	last_run_date INT,
	last_run_time INT,
	next_run_date INT,
	next_run_time INT,
	next_run_schedule_id INT,
	requested_to_run INT, --BOOL
	request_source INT,
	request_source_id SYSNAME COLLATE database_default NULL,
	running INT, --BOOL
	current_step INT,
	current_retry_attempt INT,
	job_state INT
)
DECLARE @agent_reader INT
DECLARE @user_name SYSNAME
SET @agent_reader = ISNULL(IS_SRVROLEMEMBER(N'sysadmin'), 0)
IF @agent_reader = 0
BEGIN
	SET @agent_reader = ISNULL(IS_MEMBER(N'SQLAgentReaderRole'), 0)
END
SET @user_name = SUSER_SNAME()
INSERT INTO @xp_results EXEC msdb.sys.xp_sqlagent_enum_jobs @agent_reader, @user_name
/*sda*/

SELECT
	j.name AS job_name,
	c.name AS job_category,
	CASE j.[enabled] WHEN 1 THEN 'Y' ELSE 'N' END AS [enabled],
	CASE x.running
		WHEN 1 THEN 'Running'
		ELSE
			CASE h.run_status
				WHEN 2 THEN 'Inactive'
				WHEN 4 THEN 'Inactive'
				ELSE 'Completed'
			END
	END AS [status],
	CASE
		WHEN ja.run_duration > 0 AND x.running = 1 THEN
			RIGHT('000' + CONVERT(VARCHAR, (ja.run_duration - ja.run_duration % 3600) / 3600), 3) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (ja.run_duration - ja.run_duration % 60) % 3600 / 60), 2) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (ja.run_duration) % 60), 2)
	END AS duration,
	COALESCE(x.current_step, 0) AS step_id,
	s.step_name,
	CASE
		WHEN x.last_run_date > 0 THEN
			CONVERT(VARCHAR,CONVERT(DATETIME,
				  CONVERT(VARCHAR(4),x.last_run_date / 10000) + '-' +
				  CONVERT(VARCHAR(2),(x.last_run_date % 10000)/100)  + '-' +
				  CONVERT(VARCHAR(2),x.last_run_date % 100) + ' ' +       
				  CONVERT(VARCHAR(2),x.last_run_time / 10000) + ':' +       
				  CONVERT(VARCHAR(2),(x.last_run_time % 10000)/100) + ':' +       
				  CONVERT(VARCHAR(2),x.last_run_time % 100),
			120),120)
	END AS last_run_time,
	CASE h.run_status
		WHEN 0 THEN 'Failed'
		WHEN 1 THEN 'Succeeded'
		WHEN 2 THEN 'Retry'
		WHEN 3 THEN 'Canceled'
		WHEN 4 THEN 'Running'
	END AS last_run_status,
	CASE
		WHEN h.run_duration > 0 THEN
			RIGHT('000' + CONVERT(VARCHAR, (h.run_duration / 1000000) * 24 + (h.run_duration / 10000 % 100)), 3) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (h.run_duration / 100 % 100)), 2) + ':' +
			RIGHT('00' + CONVERT(VARCHAR, (h.run_duration % 100)), 2)
	END AS last_run_duration,
	h.[message] AS last_run_message,
	ls.step_id AS last_step_id,
	ls.step_name AS last_step_name,
	CONVERT(VARCHAR, ja.next_scheduled_run_date, 120) AS next_run_time,
	j.job_id AS job_id
FROM
	@xp_results x
		LEFT JOIN
	msdb.dbo.sysjobs j ON
		j.job_id = x.job_id
		LEFT JOIN
	msdb.dbo.syscategories c ON
		c.category_id = j.category_id
		LEFT JOIN
	msdb.dbo.sysjobsteps s ON
		s.job_id = x.job_id AND
		s.step_id = x.current_step
		LEFT JOIN
	(
		SELECT
			*,
			DATEDIFF(S, start_execution_date, ISNULL(stop_execution_date, GETDATE())) AS run_duration
		FROM
			msdb.dbo.sysjobactivity
		WHERE
			session_id = (SELECT TOP 1 session_id FROM msdb.dbo.syssessions ORDER BY agent_start_date DESC)
	) ja ON
		ja.job_id = j.job_id
		LEFT JOIN
	msdb.dbo.sysjobhistory h ON
		h.instance_id = ja.job_history_id
		LEFT JOIN
	msdb.dbo.sysjobsteps ls ON
		ls.job_id = x.job_id AND
		ls.step_id = ja.last_executed_step_id
ORDER BY
	x.running DESC, duration DESC, j.name
OPTION(RECOMPILE)
/*sda*/
";
            }
            else if (queryId == QueryId.Deadlocks)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
;WITH cte AS
(
	SELECT TOP 1
		target_file_name = CONVERT(XML, t.target_data).value('EventFileTarget[1]/File[1]/@name', 'NVARCHAR(256)')
	FROM
		sys.dm_xe_sessions s
			INNER JOIN
		sys.dm_xe_session_targets t ON
			t.event_session_address = s.address
	WHERE
		s.NAME = 'system_health' AND
		t.target_name = 'event_file'
	ORDER BY
		s.create_time DESC
)
SELECT
	event_id = ROW_NUMBER() OVER (ORDER BY ev.file_name, ev.file_offset, CONVERT(xml, ev.event_data).value('(event[@name=''xml_deadlock_report'']/@timestamp)[1]', 'datetime')),
	event_data = CONVERT(XML, ev.event_data),
	event_time = CONVERT(DATETIME, SWITCHOFFSET(CONVERT(DATETIMEOFFSET, CONVERT(XML, ev.event_data).value('(event[@name=''xml_deadlock_report'']/@timestamp)[1]', 'datetime')), DATENAME(TZOFFSET, SYSDATETIMEOFFSET())))
INTO #event
FROM
	cte c
		CROSS APPLY
	sys.fn_xe_file_target_read_file(LEFT(c.target_file_name, LEN(c.target_file_name) - CHARINDEX('\', REVERSE(c.target_file_name), 1)) + '\system_health*.xel', null, null, null) ev
WHERE
	ev.object_name like 'xml_deadlock_report'
/*sda*/

;WITH
deadlock_victims AS
(
	SELECT
		e.event_id,
		victim_id = victims.victim.value('@id', 'varchar(50)')
	FROM
		#event e
			CROSS APPLY
		e.event_data.nodes('/event/data/value/deadlock/victim-list/victimProcess') victims(victim)
),
deadlock_objects AS
(
	SELECT DISTINCT
		e.event_id,
		[object_name] = resources.[resource].value('@objectname', 'nvarchar(256)')
	FROM
		#event e
			CROSS APPLY
		e.event_data.nodes('/event/data/value/deadlock/resource-list/*') resources([resource])
),
deadlocks AS
(
	SELECT
		e.event_id,
		e.event_time,
		transaction_time = processes.process.value('@lasttranstarted', 'datetime'),
		e.event_data,
		objects_list = STUFF((SELECT ', ' + o.[object_name] FROM deadlock_objects o WHERE o.event_id = e.event_id ORDER BY o.[object_name] FOR XML PATH('')), 1, 2, ''),
		victim = CASE WHEN v.victim_id IS NOT NULL THEN 1 ELSE 0 END,
		spid = processes.Process.value('@spid', 'int'),
		[procedure_name] = processes.process.value('executionStack[1]/frame[1]/@procname[1]', 'varchar(200)'),
		lock_mode = processes.process.value('@lockMode', 'varchar(10)'),
		process_id = processes.process.value('@id', 'varchar(50)'),
		process_code = processes.process.value('executionStack[1]/frame[1]', 'varchar(max)'),
		client_app = processes.process.value('@clientapp', 'varchar(500)'),
		[host_name] = processes.process.value('@hostname', 'varchar(20)'),
		login_name = processes.process.value('@loginname', 'varchar(20)'),
		input_buffer = processes.process.value('inputbuf[1]', 'varchar(max)'),
		RN = ROW_NUMBER() OVER (PARTITION BY e.event_id ORDER BY e.event_id DESC, CASE WHEN v.victim_id IS NOT NULL THEN 0 ELSE 1 END, processes.process.value('@lasttranstarted', 'datetime') DESC)
	FROM
		#event e
			CROSS APPLY
		e.event_data.nodes('/event/data/value/deadlock/process-list/process') processes(process)
			LEFT JOIN
		deadlock_victims v ON
			v.event_id = e.event_id AND
			v.victim_id = processes.process.value('@id', 'varchar(50)')
)
SELECT
	dl.event_id,
	event_time = CONVERT(VARCHAR, dl.event_time, 120),
	transaction_time = CONVERT(VARCHAR, dl.transaction_time, 120),
	deadlock_graph = CASE WHEN RN = 1 THEN dl.event_data.query('/event/data/value/deadlock') END,
	dl.objects_list,
	dl.victim,
	dl.spid,
	dl.lock_mode,
	dl.process_id,
	dl.process_code,
	dl.[procedure_name],
	[program_name] = ISNULL(jn.client_app, dl.client_app),
	dl.[host_name],
	dl.login_name,
	dl.input_buffer
FROM
	deadlocks dl
		OUTER APPLY
	(
		SELECT TOP 1
			client_app = 'SQLAgent - TSQL JobStep (Job [' + j.name + '] - ' + SUBSTRING(dl.client_app, 67, 500)
		FROM
			msdb..sysjobs j
		WHERE
			dl.client_app LIKE '%' + sys.fn_varbintohexstr(j.job_id) + '%' AND
			dl.client_app LIKE 'SQLAgent - TSQL JobStep (Job 0x%)'
	) jn
ORDER BY
	dl.event_id DESC,
	dl.RN
/*sda*/
";
            }
            else if (queryId == QueryId.Locks)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	session_id,
	blocking_session_id,
	database_name,
	resource_name,
	resource_type,
	resource_subtype,
	resource_count,
	request_mode,
	request_status,
	[host_name],
	login_name,
	[program_name],
	sql_statement
FROM
	(
		SELECT
			TL.request_session_id session_id,
			TL.blocking_session_id,
			DB_NAME(TL.resource_database_id) database_name,
			CASE
				WHEN TL.resource_type = 'OBJECT' AND TL.request_mode IN ('Sch-M') THEN '*Unavailable'
				WHEN TL.resource_type = 'OBJECT' AND TL.request_mode IN ('Sch-S') AND TL.request_status = 'WAIT' THEN '*Unavailable'
				WHEN TL.resource_type = 'OBJECT' AND 1 = {3} THEN OBJECT_NAME(TL.resource_associated_entity_id, TL.resource_database_id)
				WHEN TL.resource_type = 'OBJECT' AND 0 = {3} THEN 'object_id = ' + CONVERT(NVARCHAR, TL.resource_associated_entity_id)
				WHEN TL.resource_type IN ('DATABASE', 'FILE', 'EXTENT', 'APPLICATION', 'METADATA') THEN '*Not applicable'
				WHEN TL.resource_type IN ('KEY', 'PAGE', 'RID', 'HOBT') THEN (SELECT TOP 1 OBJECT_NAME(PR.object_id, TL.resource_database_id) FROM sys.partitions PR WHERE PR.hobt_id = TL.resource_associated_entity_id)
				WHEN TL.resource_type = 'ALLOCATION_UNIT' THEN
					(
						SELECT
							CASE
								WHEN AU.[type] IN (1, 3) THEN (SELECT TOP 1 OBJECT_NAME(PR.object_id, TL.resource_database_id) FROM sys.partitions PR WHERE PR.hobt_id = AU.allocation_unit_id)
								WHEN AU.[type] = 2 THEN (SELECT TOP 1 OBJECT_NAME(PR.object_id, TL.resource_database_id) FROM sys.partitions PR WHERE PR.partition_id = AU.allocation_unit_id)
								ELSE NULL
							END
						FROM
							sys.allocation_units AU
						WHERE
							AU.allocation_unit_id = TL.resource_associated_entity_id		
					)
				ELSE '*Undefined'
			END resource_name,
			TL.resource_type,
			TL.resource_subtype,
			TL.resource_count,
			TL.request_mode,
			TL.request_status,
			ES.[host_name],
			ES.login_name,
			ES.[program_name],
			CASE
				WHEN TL.resource_type = 'DATABASE' AND TL.resource_subtype = '' THEN SC.[text]
				ELSE ''
			END sql_statement
		FROM
			(
				SELECT
					T.request_session_id,
					T.resource_database_id,
					T.resource_type,
					T.resource_subtype,
					T.request_mode,
					T.request_status,
					W.blocking_session_id,
					MAX(T.resource_associated_entity_id) resource_associated_entity_id,
					COUNT(*) resource_count
				FROM
					sys.dm_tran_locks T
						LEFT JOIN
					sys.dm_os_waiting_tasks W ON
						W.resource_address = T.lock_owner_address
				GROUP BY
					CASE
						WHEN T.resource_type IN ('KEY', 'PAGE', 'RID', 'HOBT', 'ALLOCATION_UNIT') THEN -1
						ELSE T.resource_associated_entity_id
					END,		
					T.request_session_id,
					T.resource_database_id,
					T.resource_type,
					T.resource_subtype,
					T.request_mode,
					T.request_status,
					W.blocking_session_id
			) TL
				INNER JOIN
			sys.dm_exec_sessions ES ON
				ES.session_id = TL.request_session_id
				OUTER APPLY
			(
				SELECT TOP 1
					EC.most_recent_sql_handle
				FROM
					sys.dm_exec_connections EC
				WHERE
					EC.session_id = TL.request_session_id
				ORDER BY
					EC.most_recent_sql_handle DESC
			) EC
				OUTER APPLY
			sys.dm_exec_sql_text(EC.most_recent_sql_handle) SC
	) X
WHERE
	(1=1)
	{0}AND sql_statement NOT LIKE '%/*sda*/%'
	{1}AND database_name = '{2}'
ORDER BY
	session_id,
	database_name,
	resource_name,
	CASE
		WHEN resource_type = 'DATABASE' THEN 0
		WHEN resource_type IN ('FILE', 'EXTENT', 'APPLICATION', 'METADATA') THEN 1
		WHEN resource_type = 'OBJECT' THEN 2
		WHEN resource_type = 'PAGE' THEN 3
		WHEN resource_type IN ('KEY', 'RID') THEN 4
		WHEN resource_type IN ('HOBT', 'ALLOCATION_UNIT') THEN 5
		ELSE 10
	END,
	resource_subtype
";
            }
            else if (queryId == QueryId.TopQueries)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
WITH
QRYSTS AS
(
	SELECT
		CONVERT(SMALLINT, pa.value) pa_dbid,
		qs.*
	FROM
		sys.dm_exec_query_stats qs
			OUTER APPLY
		sys.dm_exec_plan_attributes(qs.plan_handle) pa
	WHERE
		pa.attribute = 'dbid'
),
TOPQRY AS
(
	SELECT TOP 20
		qt.text,
		ISNULL(DB_NAME(ISNULL(qt.dbid, pa_dbid)), '') database_name,
		qs.*
	FROM
		QRYSTS qs
			CROSS APPLY
		sys.dm_exec_sql_text(qs.sql_handle) qt
	WHERE
		qt.encrypted = 0
		{0}AND qt.text NOT LIKE '%/*sda*/%'
		{1}AND ISNULL(DB_NAME(ISNULL(qt.dbid, pa_dbid)), '') = '{2}'
		{3}AND DATEDIFF(MI, qs.last_execution_time, GETDATE()) <= {4}
	ORDER BY
		{5}total_worker_time DESC
		{6}total_logical_reads DESC
)
SELECT
	SUBSTRING
	(
		q.text,
		q.statement_start_offset / 2 + 1, 
		(
			CASE q.statement_end_offset
				WHEN -1 THEN DATALENGTH(q.text)
				ELSE q.statement_end_offset
			END - q.statement_start_offset
		) / 2 + 1
	) sql_statement,
	database_name,
	CONVERT(VARCHAR, q.creation_time, 120) creation_time,
	CONVERT(VARCHAR, q.last_execution_time, 120) last_execution_time,
	q.execution_count,
	q.total_worker_time,
	q.last_worker_time,
	q.min_worker_time,
	q.max_worker_time,
	q.total_physical_reads,
	q.last_physical_reads,
	q.min_physical_reads,
	q.max_physical_reads,
	q.total_logical_writes,
	q.last_logical_writes,
	q.min_logical_writes,
	q.max_logical_writes,
	q.total_logical_reads,
	q.last_logical_reads,
	q.min_logical_reads,
	q.max_logical_reads,
	q.total_clr_time,
	q.last_clr_time,
	q.min_clr_time,
	q.max_clr_time,
	q.total_elapsed_time,
	q.last_elapsed_time,
	q.min_elapsed_time,
	q.max_elapsed_time
FROM
	TOPQRY q
";
            }
            else if (queryId == QueryId.Cpu)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT
	CU.sql_server, 
	100 - CU.system_idle - CU.sql_server other_processes,
	CU.system_idle,
	CONVERT(VARCHAR(16), DATEADD(ss, (CU.[timestamp] - TM.current_ts) / 1000, GETDATE()), 120) collection_time
FROM
	(
		SELECT cpu_ticks / CONVERT(FLOAT, cpu_ticks / ms_ticks) current_ts FROM sys.dm_os_sys_info
	) TM
		CROSS JOIN
	(
		SELECT
			RB.[record].value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 'int') sql_server,
			RB.[record].value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]', 'int') system_idle,
			RB.[timestamp]
		FROM
			(
				SELECT TOP 20
					[timestamp],
					CONVERT(XML, record) [record]
				FROM
					sys.dm_os_ring_buffers
				WHERE
					ring_buffer_type = 'RING_BUFFER_SCHEDULER_MONITOR' AND
					[record] LIKE '%<SystemHealth>%'
				ORDER BY
					[timestamp] DESC
			) RB
	) CU
/*sda*/
";
            }
            else if (queryId == QueryId.PlansCache)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	database_name,
	cache_object_type,
	object_type,
	SUM(CONVERT(BIGINT, size_in_bytes)) / 1024 size_kb,
	COUNT(*) object_count
FROM
	(
		SELECT
			DB_NAME(ISNULL(st.dbid, CONVERT(SMALLINT, pa.value))) database_name,
			cp.cacheobjtype cache_object_type,
			cp.objtype object_type,
			cp.size_in_bytes
		FROM
			sys.dm_exec_cached_plans cp
				OUTER APPLY
			sys.dm_exec_plan_attributes(cp.plan_handle) pa
				OUTER APPLY
			sys.dm_exec_sql_text(cp.plan_handle) st
		WHERE
			pa.attribute = 'dbid'
			{0}AND st.text not like '%/*sda*/%'
			{1}AND DB_NAME(ISNULL(st.dbid, CONVERT(SMALLINT, pa.value))) = '{2}'
	) C
GROUP BY
	database_name,
	cache_object_type,
	object_type
ORDER BY 4 DESC
";
            }
            else if (queryId == QueryId.DatabasesCache)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
    DB_NAME([database_id]) [database_name],
    COUNT(1) / 128 used_MB
FROM sys.dm_os_buffer_descriptors
WHERE [database_id] <> 32767
{0}AND DB_NAME([database_id]) = '{1}'
GROUP BY [database_id]
ORDER BY used_MB DESC
/*sda*/
";
            }
            else if (queryId == QueryId.ObjectsCache)
            {
                return @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
    QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(OBJECT_NAME(object_id)) [object_name],
	COUNT(1) / 128 AS cached_MB  
FROM
	sys.dm_os_buffer_descriptors AS bd
	    INNER JOIN
    (
        SELECT
			p.object_id,
			au.allocation_unit_id
        FROM
			sys.allocation_units au  
				INNER JOIN
			sys.partitions p ON
				p.hobt_id = au.container_id AND
				au.type IN (1, 3)  
        UNION ALL  
        SELECT
			p.object_id,
			au.allocation_unit_id
        FROM
			sys.allocation_units au
				INNER JOIN
			sys.partitions p ON
				p.partition_id = au.container_id AND
				au.type = 2  
    ) obj ON
		obj.allocation_unit_id = bd.allocation_unit_id
WHERE
	database_id = DB_ID()  
GROUP BY
	object_id
HAVING COUNT(1) / 128 > 0
ORDER BY cached_MB DESC
/*sda*/
";
            }
            else if (queryId == QueryId.ViewData)
            {
                return @"SELECT{0} * FROM [{1}].[{2}].[{3}]{4}";
            }
            else if (queryId == QueryId.DataSearch)
            {
                return @"
SELECT 'DECLARE @search NVARCHAR(100)' cmd, '--' cm, 0 sn, 1 rn
UNION ALL
SELECT 'SET @search = N''%search this%''' cmd, '--' cm, 0 sn, 2 rn
UNION ALL
SELECT 'DECLARE @FOUND INT' cmd, '--' cm, 0 sn, 3 rn
UNION ALL
SELECT 'SET @FOUND = 0' cmd, '--' cm, 0 sn, 4 rn
UNION ALL
SELECT
	'IF EXISTS(SELECT TOP 1 1 FROM ' + QUOTENAME(SCHEMA_NAME(T.schema_id)) + '.' + QUOTENAME(T.name) + ' WITH(NOLOCK) WHERE ' +
	CASE
		WHEN C.collation_name IS NOT NULL THEN QUOTENAME(C.name)
		WHEN CT.name IN ('image') THEN 'CONVERT(NVARCHAR(MAX), CONVERT(VARBINARY(MAX), ' + QUOTENAME(C.name) + '))'
		ELSE 'CONVERT(NVARCHAR(MAX), ' + QUOTENAME(C.name) + ')'
	END +
	' LIKE @search) BEGIN PRINT ''' + REPLACE(QUOTENAME(SCHEMA_NAME(T.schema_id)) + '.' + QUOTENAME(T.name) + '.' + QUOTENAME(C.name), '''', '''''') + '''; SET @FOUND = @FOUND + 1 END' cmd,
	'--' cm,
	1 sn,
	ROW_NUMBER() OVER (ORDER BY SCHEMA_NAME(T.schema_id), T.name, C.name) rn
FROM
	sys.tables T
		INNER JOIN
	sys.columns C ON
		C.object_id = T.object_id
		INNER JOIN
	sys.types CT ON
		CT.user_type_id = C.user_type_id
WHERE
	NOT EXISTS (SELECT TOP 1 1 FROM sys.extended_properties EP WHERE EP.major_id = T.object_id AND EP.name = 'microsoft_database_tools_support') AND
	T.is_ms_shipped = 0 AND
	(C.collation_name IS NOT NULL OR CT.name IN ('image','varbinary','binary','xml')) AND
	C.is_computed = 0
UNION ALL
SELECT 'SELECT CONVERT(VARCHAR, @FOUND) + '' columns found'' search_result' cmd, '--' cm, 2 sn, 1 rn
ORDER BY
	3, 4
/*sda*/
";
            }
            else
            {
                return "";
            }
        }
    }
}
