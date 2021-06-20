using System;
using System.Data;

namespace PoEAA_TableDataGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            RunReaderVersionExample();
            RunDataTableVersionExample();
        }

        private static void RunDataTableVersionExample()
        {
            InitializeData();

            Console.WriteLine("Start RunDataTableVersionExample");
            Console.WriteLine("Function: Get all persons");
            var gateway = new DataTableVersion.PersonGateway();
            gateway.LoadAll();
            var allPersons = gateway.Table.Rows;
            PrintPersonsRowData(allPersons);

            Console.WriteLine("Function: Get person by id = 2");
            var onePerson = gateway[2];
            PrintPersonRowData(onePerson);

            Console.WriteLine("Function: Update person by id = 2");
            onePerson["lastname"] = "Jackson";
            onePerson["firstname"] = "Michael";
            onePerson["numberOfDependents"] = 100;
            gateway.Holder.Update();
            Console.WriteLine("Function: Get person by id = 2");
            var updatedPerson = gateway[2];
            PrintPersonRowData(updatedPerson);

            Console.WriteLine("Function: Insert a person");
            gateway.Insert("Skinner", "Neil", 3);
            gateway.Holder.Update();
            Console.WriteLine("Function: Get all persons");
            allPersons = gateway.Table.Rows;
            PrintPersonsRowData(allPersons);

            Console.WriteLine("Function: Get persons by numberOfDependents > 10");
            gateway = new DataTableVersion.PersonGateway();
            gateway.LoadWhere("numberOfDependents > 10");
            var findPersons = gateway.Table.Rows;
            PrintPersonsRowData(findPersons);

            Console.WriteLine("Function: Delete person by id = 1");
            gateway = new DataTableVersion.PersonGateway();
            gateway.LoadAll();
            var deletedRow = gateway[1];
            deletedRow.Delete();
            gateway.Holder.Update();

            Console.WriteLine("Function: Get all persons");
            allPersons = gateway.Table.Rows;
            PrintPersonsRowData(allPersons);

            Console.WriteLine("End RunDataTableVersionExample");
        }

        private static void RunReaderVersionExample()
        {
            InitializeData();

            Console.WriteLine("Start RunReaderVersionExample");
            Console.WriteLine("Function: Get all persons");
            var gateway = new ReaderVersion.PersonGateway();
            var allPersons = gateway.FindAll();
            PrintPersonsRowData(allPersons);
            allPersons.Close();

            Console.WriteLine("Function: Get person by id = 2");
            var onePerson = gateway.FindRow(2);
            PrintPersonRowData(onePerson);

            Console.WriteLine("Function: Update person by id = 2");
            gateway.Update(2, "Jackson", "Michael", 100);
            Console.WriteLine("Function: Get person by id = 2");
            var updatedPerson = gateway.FindRow(2);
            PrintPersonRowData(updatedPerson);

            Console.WriteLine("Function: Insert a person");
            gateway.Insert("Skinner", "Neil", 3);

            Console.WriteLine("Function: Get all persons");
            allPersons = gateway.FindAll();
            PrintPersonsRowData(allPersons);
            allPersons.Close();

            Console.WriteLine("Function: Get persons by numberOfDependents > 10");
            var findPersons = gateway.FindWhere("numberOfDependents > 10");
            PrintPersonsRowData(findPersons);
            findPersons.Close();

            Console.WriteLine("Function: Delete person by id = 1");
            gateway.Delete(1);

            Console.WriteLine("Function: Get all persons");
            allPersons = gateway.FindAll();
            PrintPersonsRowData(allPersons);
            allPersons.Close();

            Console.WriteLine("End RunReaderVersionExample");
        }

        private static void PrintPersonRowData(object[] columns)
        {
            Console.WriteLine($"id: {columns[0]}, lastname: {columns[1]}, firstname: {columns[2]}, numberOfDependents: {columns[3]}");
        }

        private static void PrintPersonRowData(DataRow columns)
        {
            Console.WriteLine($"id: {columns[0]}, lastname: {columns[1]}, firstname: {columns[2]}, numberOfDependents: {columns[3]}");
        }

        private static void PrintPersonsRowData(DataRowCollection dataRows)
        {
            foreach (DataRow row in dataRows)
            {
                Console.WriteLine($"id: {row["id"]}, lastname: {row["lastname"]}, firstname: {row["firstname"]}, numberOfDependents: {row["numberOfDependents"]}");
            }
        }

        private static void PrintPersonsRowData(IDataReader reader)
        {
            while (reader.Read())
            {
                Console.WriteLine($"id: {reader["id"]}, lastname: {reader["lastname"]}, firstname: {reader["firstname"]}, numberOfDependents: {reader["numberOfDependents"]}");
            }
        }

        private static void InitializeData()
        {
            using (var connection = DbManager.CreateConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                            @"
                        DROP TABLE IF EXISTS person;
                    ";
                        command.ExecuteNonQuery();


                        command.CommandText =
                            @"
                        CREATE TABLE person (Id int primary key, lastname TEXT, firstname TEXT, numberOfDependents int);
                    ";
                        command.ExecuteNonQuery();

                        command.CommandText =
                            @"
                       
                    INSERT INTO person
                        VALUES (1, 'Sean', 'Reid', 5);

                    INSERT INTO person
                        VALUES (2, 'Madeleine', 'Lyman', 13);

                    INSERT INTO person
                        VALUES (3, 'Oliver', 'Wright', 66);
                    ";
                    command.ExecuteNonQuery();
                }
                    
            }
        }
    }
}
