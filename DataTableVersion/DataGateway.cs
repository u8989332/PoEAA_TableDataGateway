using System.Data;

namespace PoEAA_TableDataGateway.DataTableVersion
{
    abstract class DataGateway
    {
        public abstract string TableName { get; }
        public DataSetHolder Holder;

        public DataSet Data => Holder.Data;

        public abstract DataTable Table { get; }

        protected DataGateway()
        {
            Holder = new DataSetHolder();
        }

        protected DataGateway(DataSetHolder holder)
        {
            this.Holder = holder;
        }

        public void LoadAll()
        {
            string commandString = $"select * from {TableName}";
            Holder.FillData(commandString, TableName);
        }

        public void LoadWhere(string whereClause)
        {
            string commandString = $"select * from {TableName} where {whereClause}";
            Holder.FillData(commandString, TableName);
        }
    }
}
