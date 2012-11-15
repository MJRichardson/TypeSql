using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TypeSql
{
    public class ExampleSelectTemplate
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;
        private const string _sqlStatement = @"SELECT Name:string, DOB:DateTime, LastModified:DateTimeOffset FROM customers";
        private SqlTransaction _transaction;

        public ExampleSelectTemplate()
        {
            _connectionString = "pull this from some web.config";
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
                                             Name = reader.GetString(reader.GetOrdinal("Name")),
                                             DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                             LastModified = reader.GetDateTimeOffset(reader.GetOrdinal("LastModified")),
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
            public DateTime DOB { get; set; }
            public DateTimeOffset LastModified { get; set; }
        }
    }
}