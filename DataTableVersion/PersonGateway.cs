using System.Data;

namespace PoEAA_TableDataGateway.DataTableVersion
{
    class PersonGateway : DataGateway
    {
        public override string TableName => "person";

        public override DataTable Table => Data.Tables[TableName];

        public PersonGateway() : base()
        {

        }

        public PersonGateway(DataSetHolder holder) : base(holder)
        {

        }

        public DataRow this[long key]
        {
            get
            {
                string filter = $"id = {key}";
                return Table.Select(filter)[0];
            }
        }

        public long Insert(string lastName, string firstName, int numberOfDependents)
        {
            long key = GetNextId();
            DataRow newRow = Table.NewRow();
            newRow["id"] = key;
            newRow["lastname"] = lastName;
            newRow["firstname"] = firstName;
            newRow["numberOfDependents"] = numberOfDependents;
            Table.Rows.Add(newRow);

            return key;
        }

        private long GetNextId()
        {
            var result = Table.Compute("max([id])", string.Empty);
            if (result != System.DBNull.Value)
            {
                return ((int)result + 1);
            }
            else
            {
                return 1;
            }
        }
    }
}
