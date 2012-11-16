using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace TypeSql.Tests.Templates
{
    public class ExampleSelectTemplateWithWhereClause
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;
        private const string _sqlStatement = @"SELECT Name:string, DOB:DateTime, LastModified:DateTimeOffset FROM customers WHERE Name = @name AND DOB < @dob";
        private SqlTransaction _transaction;

        private readonly string _name;
        private readonly DateTime _dob;

        public ExampleSelectTemplateWithWhereClause(string name, DateTime dob) : this(name, dob, "default connection string")
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString;
        }

        public ExampleSelectTemplateWithWhereClause(string name, DateTime dob, SqlConnection connection, SqlTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
            _name = name;
            _dob = dob;
        }

        public ExampleSelectTemplateWithWhereClause(string name, DateTime dob, string connectionString, SqlTransaction transaction = null)
        {
            _connectionString = connectionString;
            _transaction = transaction;
            _name = name;
            _dob = dob;
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
                {
                    cmd.Parameters.AddWithValue("@Name", _name);
                    cmd.Parameters.AddWithValue("@DOB", _dob);

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