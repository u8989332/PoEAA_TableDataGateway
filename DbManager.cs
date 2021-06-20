using System.Data.SQLite;

namespace PoEAA_TableDataGateway
{
    public static class DbManager
    {
        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source=poeaa_tabledatagateway.db");
        }
    }
}