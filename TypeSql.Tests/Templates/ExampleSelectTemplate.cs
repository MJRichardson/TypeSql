using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace TypeSql.Tests.Templates
{
    public class ExampleSelectTemplate : DbTemplate
    {
        private const string _typedSqlStatement = @"SELECT FirstName:String, ModifiedDate:DateTime FROM SalesLT.customers";
        private const string _sqlStatement = @"SELECT FirstName, ModifiedDate FROM SalesLT.Customer";

        public ExampleSelectTemplate() : base("AdventureWorks") {}

        public ExampleSelectTemplate(DbConnection connection, DbTransaction transaction = null) : base(connection, transaction) {}

        public ExampleSelectTemplate(string connectionStringKey, DbTransaction transaction = null) : base(connectionStringKey, transaction) {}

    
        public IEnumerable<Result> Execute()
        {
            return ReadResults();
        }

        public Task<IEnumerable<Result>> ExecuteAsync()
        {
            var task = new Task<IEnumerable<Result>>(ReadResults);
            task.Start();
            return task;
        }

        private IEnumerable<Result> ReadResults()
        {
            DbConnection connection = _connection ?? GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            DbCommand command = connection.CreateCommand();
            command.CommandText = _sqlStatement;
            command.Transaction = _transaction;
            return ReadResults(command);
        }

        private IEnumerable<Result> ReadResults(DbCommand command)
        {
            try
            {
                using (command)
                {
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Result
                                         {
                                             Name = reader.GetFieldValue<String>(reader.GetOrdinal("FirstName")),
                                             ModifiedDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("ModifiedDate")),
                                         };
                        }
                    }
                }
            }
            finally
            {
                if (_connection == null) // if not user supplied connection.
                    command.Connection.Dispose();
            }
        }

        public class Result
        {
            public string Name { get; set; }
            public DateTime ModifiedDate { get; set; }
        }
    }
}