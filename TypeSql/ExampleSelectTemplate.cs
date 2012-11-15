using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace TypeSql
{
    public class ExampleSelectTemplate
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;
        private const string _typedSqlStatement = @"SELECT FirstName:String, ModifiedDate:DateTime FROM SalesLT.customers";
        private const string _sqlStatement = @"SELECT FirstName, ModifiedDate FROM SalesLT.Customer";
        private SqlTransaction _transaction;

        public ExampleSelectTemplate()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
        }

        public ExampleSelectTemplate(SqlConnection connection, SqlTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public ExampleSelectTemplate(string connectionString, SqlTransaction transaction = null)
        {
            _connectionString = connectionString;
            _transaction = transaction;
        }

        public IList<Record> Records
        {
            get { return Execute(); }
        }

        protected virtual IList<Record> Execute()
        {
            List<Record> records = new List<Record>();
            using (SqlConnection connection = _connection ?? new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(_sqlStatement, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new Record
                                         {
                                             Name = reader.GetString(reader.GetOrdinal("FirstName")),
                                             ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                                         };
                            records.Add(record);
                        }
                    }
            }
            return records;
        }

        public class Record
        {
            public string Name { get; set; }
            public DateTime ModifiedDate { get; set; }
        }
    }
}