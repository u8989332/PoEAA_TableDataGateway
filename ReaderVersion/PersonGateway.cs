using System.Data;
using System.Data.SQLite;

namespace PoEAA_TableDataGateway.ReaderVersion
{
    class PersonGateway
    {
        public IDataReader FindAll()
        {
            string sql = "select * from person";
            var conn = DbManager.CreateConnection();
            conn.Open();
            return new SQLiteCommand(sql, conn).ExecuteReader();
        }

        public IDataReader FindWithLastName(string lastName)
        {
            string sql = "select * from person where lastname = $lastname";
            var conn = DbManager.CreateConnection();
            conn.Open();
            IDbCommand comm = new SQLiteCommand(sql, conn);
            comm.Parameters.Add(new SQLiteParameter("$lastname", lastName));

            return comm.ExecuteReader();
        }

        public IDataReader FindWhere(string whereClause)
        {
            string sql = $"select * from person where {whereClause}";
            var conn = DbManager.CreateConnection();
            conn.Open();
            return new SQLiteCommand(sql, conn).ExecuteReader();
        }

        public object[] FindRow(long key)
        {
            string sql = "select * from person where id = $id";
            using var conn = DbManager.CreateConnection();
            conn.Open();
            using IDbCommand comm = new SQLiteCommand(sql, conn);
            comm.Parameters.Add(new SQLiteParameter("$id", key));
            using IDataReader reader = comm.ExecuteReader();
            reader.Read();
            object[] result = new object[reader.FieldCount];
            reader.GetValues(result);
            return result;
        }

        public void Update(long key, string lastName, string firstName, int numberOfDependents)
        {
            string sql =
                @"Update person SET lastname = $lastname, firstname = $firstname, numberOfDependents = $numberOfDependents
                            WHERE id = $id";
            using var conn = DbManager.CreateConnection();
            conn.Open();
            using IDbCommand comm = new SQLiteCommand(sql, conn);
            comm.Parameters.Add(new SQLiteParameter("$lastname", lastName));
            comm.Parameters.Add(new SQLiteParameter("$firstname", firstName));
            comm.Parameters.Add(new SQLiteParameter("$numberOfDependents", numberOfDependents));
            comm.Parameters.Add(new SQLiteParameter("$id", key));
            comm.ExecuteNonQuery();
        }

        public long Insert(string lastName, string firstName, int numberOfDependents)
        {
            string sql =
                @"INSERT INTO person VALUES ($id, $lastname, $firstname, $numberOfDependents)";
            long key = GetNextId();
            using var conn = DbManager.CreateConnection();
            conn.Open();
            using IDbCommand comm = new SQLiteCommand(sql, conn);
            comm.Parameters.Add(new SQLiteParameter("$id", key));
            comm.Parameters.Add(new SQLiteParameter("$lastname", lastName));
            comm.Parameters.Add(new SQLiteParameter("$firstname", firstName));
            comm.Parameters.Add(new SQLiteParameter("$numberOfDependents", numberOfDependents));
            comm.ExecuteNonQuery();
            return key;
        }

        public void Delete(long key)
        {
            string sql = "DELETE FROM person WHERE id = $id";
            using var conn = DbManager.CreateConnection();
            conn.Open();
            IDbCommand comm = new SQLiteCommand(sql, conn);
            comm.Parameters.Add(new SQLiteParameter("$id", key));
            comm.ExecuteNonQuery();
        }

        private long GetNextId()
        {
            string sql = "SELECT max(id) as curId from person";
            using var conn = DbManager.CreateConnection();
            conn.Open();
            using IDbCommand comm = new SQLiteCommand(sql, conn);
            using IDataReader reader = comm.ExecuteReader();
            bool hasResult = reader.Read();
            if (hasResult)
            {
                return ((long)reader["curId"] + 1);
            }
            else
            {
                return 1;
            }
        }
    }
}
