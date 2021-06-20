using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace PoEAA_TableDataGateway.DataTableVersion
{
    class DataSetHolder
    {
        public DataSet Data = new DataSet();
        private readonly Dictionary<string, SQLiteDataAdapter> _dataAdapters = new Dictionary<string, SQLiteDataAdapter>();

        public void FillData(string query, string tableName)
        {
            if (_dataAdapters.ContainsKey(tableName))
            {
                throw new MultipleLoadException();
            }

            var conn = DbManager.CreateConnection();
            conn.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(da);
            da.Fill(Data, tableName);
            _dataAdapters.Add(tableName, da);
        }

        public void Update()
        {
            foreach (string table in _dataAdapters.Keys)
            {
                (_dataAdapters[table]).Update(Data, table);
            }
        }

        public DataTable this[string tableName] => Data.Tables[tableName];
    }

    public class MultipleLoadException : Exception
    {
    }
}
