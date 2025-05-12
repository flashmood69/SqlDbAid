using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace SqlDbAid
{
    class DataExporter : BackgroundWorker
    {
        public enum OutputType
        {
            Text,
            Script,
            Table
        }

        OutputType mExportType = OutputType.Text;

        int mExecutionTimeout = 30;
        int mMaxRows = 0;

        string mConnectionString = "";
        string mCommandText = "";

        string mFileName = "";
        string mFieldDelimiter = "";
        string mRowDelimiter = "";
        string mTextQualifier = "";
        bool mIncludeColumnNames = true;

        bool mUseLocalSettings = true;
        bool mUnicode = false;

        string mTableSchema = "";
        string mTableName = "";

        string[] mColumnTypes;

        bool mCollectSqlMessages = false;
        string mMessage;

        DataTable mResultTable;

        int mRowCount = 0;

        public string[] ColumnTypes
        {
            get { return mColumnTypes; }
        }

        public DataTable ResultTable
        {
            get { return mResultTable; }
        }

        public OutputType ExportType
        {
            get { return mExportType; }
            set { mExportType = value; }
        }

        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }

        public string CommandText
        {
            get { return mCommandText; }
            set { mCommandText = value; }
        }

        public string FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }

        public string FieldDelimiter
        {
            get { return mFieldDelimiter; }
            set { mFieldDelimiter = value; }
        }

        public string RowDelimiter
        {
            get { return mRowDelimiter; }
            set { mRowDelimiter = value; }
        }

        public string TextQualifier
        {
            get { return mTextQualifier; }
            set { mTextQualifier = value; }
        }

        public bool ExportColumnNames
        {
            get { return mIncludeColumnNames; }
            set { mIncludeColumnNames = value; }
        }

        public bool UseLocalSettings
        {
            get { return mUseLocalSettings; }
            set { mUseLocalSettings = value; }
        }

        public bool Unicode
        {
            get { return mUnicode; }
            set { mUnicode = value; }
        }

        public string TableSchema
        {
            get { return mTableSchema; }
            set { mTableSchema = value; }
        }

        public string TableName
        {
            get { return mTableName; }
            set { mTableName = value; }
        }

        public int ExecutionTimeout
        {
            get { return mExecutionTimeout; }
            set { mExecutionTimeout = value; }
        }

        public int MaxRows
        {
            get { return mMaxRows; }
            set { mMaxRows = value; }
        }

        public bool CollectSqlMessages
        {
            get { return mCollectSqlMessages; }
            set { mCollectSqlMessages = value; }
        }

        public string Message
        {
            get { return mMessage; }
        }

        public void ClearResult()
        {
            mResultTable = null;
        }

        private string BinaryToString(byte[] binary)
        {
            return "0x" + BitConverter.ToString(binary).Replace("-", "");
        }

        private void TxtExport()
        {
            mRowCount = 0;

            using (SqlConnection cnn = new SqlConnection(mConnectionString))
            {
                SqlCommand cmd = new SqlCommand(mCommandText, cnn);
                cmd.CommandTimeout = mExecutionTimeout;

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Encoding flEnc;

                if (mUnicode)
                {
                    flEnc = Encoding.Unicode;
                }
                else
                {
                    flEnc = Encoding.Default;
                }

                using (StreamWriter sw = new System.IO.StreamWriter(mFileName.ToString(), false, flEnc))
                {
                    object[] items = new object[dr.FieldCount];
                    List<int> byteArrayIdx = new List<int>();

                    StringBuilder rowFormat = new StringBuilder();

                    mColumnTypes = new string[dr.FieldCount];

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        mColumnTypes[i] = dr.GetFieldType(i).Name;

                        if (mIncludeColumnNames)
                        {
                            sw.Write(dr.GetName(i));
                        }

                        if (mColumnTypes[i].ToLower() == "string" || dr.GetDataTypeName(i).ToLower() == "sql_variant")
                        {
                            rowFormat.AppendFormat("{0}{1}{2}{3}{4}", mTextQualifier, "{", i, "}", mTextQualifier);
                        }
                        else
                        {
                            rowFormat.AppendFormat("{0}{1}{2}", "{", i, "}");

                            if (mColumnTypes[i].ToLower() == "byte[]")
                            {
                                byteArrayIdx.Add(i);
                            }
                        }

                        if (i == dr.FieldCount - 1)
                        {
                            if (mIncludeColumnNames)
                            {
                                sw.Write(mRowDelimiter);
                            }

                            rowFormat.Append(mRowDelimiter);
                        }
                        else
                        {
                            if (mIncludeColumnNames)
                            {
                                sw.Write(mFieldDelimiter);
                            }

                            rowFormat.Append(mFieldDelimiter);
                        }
                    }

                    if (byteArrayIdx.Count > 0)
                    {
                        while (dr.Read() && !this.CancellationPending)
                        {
                            mRowCount++;
                            if (mRowCount % 1000 == 0)
                            {
                                this.ReportProgress(mRowCount);
                            }

                            dr.GetValues(items);

                            foreach (int i in byteArrayIdx)
                            {
                                if (items[i] != System.DBNull.Value)
                                {
                                    items[i] = BinaryToString((byte[])items[i]);
                                }
                            }

                            sw.Write(rowFormat.ToString(), items);
                        }
                    }
                    else
                    {
                        while (dr.Read() && !this.CancellationPending)
                        {
                            mRowCount++;
                            if (mRowCount % 1000 == 0)
                            {
                                this.ReportProgress(mRowCount);
                            }
                            dr.GetValues(items);
                            sw.Write(rowFormat.ToString(), items);
                        }
                    }

                    sw.Flush();
                    sw.Close();
                }

                dr.Close();
            }
        }

        private void InsertExport()
        {
            mRowCount = 0;

            using (SqlConnection cnn = new SqlConnection(mConnectionString))
            {
                SqlCommand cmd = new SqlCommand(mCommandText, cnn);
                cmd.CommandTimeout = mExecutionTimeout;

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Encoding flEnc;

                if (mUnicode)
                {
                    flEnc = Encoding.Unicode;
                }
                else
                {
                    flEnc = Encoding.UTF8;
                }

                using (StreamWriter sw = new System.IO.StreamWriter(mFileName.ToString(), false, flEnc))
                {
                    object[] items = new object[dr.FieldCount];
                    object[] fields = new object[dr.FieldCount];
                    int[] typeArray = new int[dr.FieldCount];

                    StringBuilder rowFormat = new StringBuilder();
                    StringBuilder valueFormat = new StringBuilder();

                    int fieldCount = dr.FieldCount;
                    mColumnTypes = new string[fieldCount];

                    rowFormat.AppendFormat("INSERT INTO [{0}].[{1}] (", mTableSchema, mTableName);
                    valueFormat.Append(" VALUES (");

                    for (int i = 0; i < fieldCount; i++)
                    {
                        mColumnTypes[i] = dr.GetFieldType(i).Name;

                        if (i > 0)
                        {
                            rowFormat.Append(",");
                            valueFormat.Append(",");
                        }

                        rowFormat.AppendFormat("[{0}]", dr.GetName(i));
                        valueFormat.AppendFormat("{0}{1}{2}", "{", i, "}");

                        if (dr.GetDataTypeName(i).ToLower() == "timestamp")
                        {
                            typeArray[i] = 3;
                        }
                        else if (mColumnTypes[i].ToLower().Contains("string") || dr.GetDataTypeName(i).ToLower() == "sql_variant")
                        {
                            if ("#nchar#ntext#nvarchar#sysname#".Contains(string.Format("#{0}#", dr.GetDataTypeName(i).ToLower())))
                            {
                                typeArray[i] = -1;
                            }
                            else
                            {
                                typeArray[i] = 0;
                            }
                        }
                        else if (mColumnTypes[i].ToLower().Contains("byte[]"))
                        {
                            typeArray[i] = 2;
                        }
                        else
                        {
                            typeArray[i] = 1;
                        }
                    }

                    rowFormat.Append(")");
                    valueFormat.Append(")\r\nGO\r\n");
                    rowFormat.Append(valueFormat.ToString());

                    while (dr.Read() && !this.CancellationPending)
                    {
                        mRowCount++;
                        if (mRowCount % 1000 == 0)
                        {
                            this.ReportProgress(mRowCount);
                        }
                        dr.GetValues(items);

                        for (int i = 0; i < fieldCount; i++)
                        {
                            if (items[i] == System.DBNull.Value)
                            {
                                fields[i] = "NULL";
                            }
                            else if (typeArray[i] == -1)
                            {
                                fields[i] = string.Format("N'{0}'", items[i].ToString().Replace("'", "''"));
                            }
                            else if (typeArray[i] == 0)
                            {
                                fields[i] = string.Format("'{0}'", items[i].ToString().Replace("'", "''"));
                            }
                            else if (typeArray[i] == 1)
                            {
                                fields[i] = items[i];
                            }
                            else if (typeArray[i] == 2)
                            {
                                fields[i] = BinaryToString((byte[])items[i]);
                            }
                            else
                            {
                                fields[i] = "DEFAULT";
                            }
                        }

                        sw.Write(rowFormat.ToString(), fields);
                    }

                    sw.Flush();
                    sw.Close();
                }

                dr.Close();
            }
        }

        private void TableExport()
        {
            mRowCount = 0;

            mMessage = "";

            using (SqlConnection cnn = new SqlConnection(mConnectionString))
            {
                bool nullType = false;

                SqlCommand cmd = new SqlCommand(mCommandText, cnn);
                cmd.CommandTimeout = mExecutionTimeout;

                if (mCollectSqlMessages)
                {
                    cmd.StatementCompleted += new StatementCompletedEventHandler(OnStatementCompleted);
                    cnn.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                    cnn.FireInfoMessageEventOnUserErrors = true;
                }

                cnn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                mResultTable = new DataTable();

                if (dr != null && dr.FieldCount > 0)
                {
                    mColumnTypes = new string[dr.FieldCount];

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string colName = dr.GetName(i);
                        string newName = colName;
                        int colCount = 0;
                        while (mResultTable.Columns.Contains(newName))
                        {
                            colCount++;
                            newName = string.Format("{0}({1})", colName, colCount);
                        }

                        if (dr.GetFieldType(i) == null)
                        {
                            nullType = true;
                            mColumnTypes[i] = "";
                        }
                        else
                        {
                            mColumnTypes[i] = dr.GetFieldType(i).Name;
                        }

                        if ("Boolean#Byte[]".Contains(mColumnTypes[i]) || mColumnTypes[i] == "")
                        {
                            mResultTable.Columns.Add(newName);
                        }
                        else
                        {
                            mResultTable.Columns.Add(newName, dr.GetFieldType(i));
                        }
                    }
                }

                if (dr.HasRows)
                {
                    int maxRows = mMaxRows;
                    if (maxRows <= 0)
                    {
                        maxRows = -1;
                    }

                    object[] items = new object[dr.FieldCount];

                    if (nullType)
                    {
                        while (dr.Read() && !this.CancellationPending && mRowCount != maxRows)
                        {
                            mRowCount++;
                            if (mRowCount % 1000 == 0)
                            {
                                this.ReportProgress(mRowCount);
                            }

                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                if (mColumnTypes[i] == "")
                                {
                                    items[i] = dr.GetSqlBytes(i).Buffer;
                                }
                                else
                                {
                                    items[i] = dr[i];
                                }
                            }
                            mResultTable.LoadDataRow(items, false);
                        }
                    }
                    else
                    {
                        while (dr.Read() && !this.CancellationPending && mRowCount != maxRows)
                        {
                            mRowCount++;
                            if (mRowCount % 1000 == 0)
                            {
                                this.ReportProgress(mRowCount);
                            }
                            dr.GetValues(items);
                            mResultTable.LoadDataRow(items, false);
                        }
                    }
                }

                for (int i = 0; i < dr.FieldCount; i++)
                {
                    if (mColumnTypes[i] == "")
                    {
                        mColumnTypes[i] = "Byte[]";
                    }
                }

                dr.Close();
            }
        }

        private void OnStatementCompleted(Object sender, StatementCompletedEventArgs args)
        {
            mMessage += string.Format("{0} - {1} row(s) affected\r\n", string.Format("{0:s}", DateTime.Now).Replace("T", " "), args.RecordCount);
        }

        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            foreach (SqlError err in args.Errors)
            {
                mMessage += string.Format("{0} - Line: {1} Message: {2}\r\n", string.Format("{0:s}", DateTime.Now).Replace("T", " "), err.LineNumber, err.Message);
            }
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (mExportType == OutputType.Script)
            {
                CultureInfo enCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = enCulture;

                InsertExport();
            }
            else if (mExportType == OutputType.Text)
            {
                if (!mUseLocalSettings)
                {
                    CultureInfo enCulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = enCulture;
                }

                TxtExport();
            }
            else
            {
                TableExport();
            }

            e.Result = mRowCount;

            if (this.CancellationPending)
            {
                e.Cancel = true;
            }
        }
    }
}
