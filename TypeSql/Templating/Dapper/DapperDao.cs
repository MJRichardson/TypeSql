using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Dapper;

namespace TypeSql.Templating.Dapper
{
    public abstract class DapperDao<TResult>
    {
        protected DapperDao(string connectionStringName)
        {
            if (connectionStringName==null)
                throw new ArgumentNullException("connectionStringName");

            _connectionStringName = connectionStringName;
        }

        protected DapperDao(IDbConnection connection, IDbTransaction transaction=null)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<TResult> Execute()
        {
            return _connectionStringName != null 
                ? Enumerable(_connectionStringName) 
                : _connection.Query<TResult>(Sql, transaction: _transaction);
        }

        private IEnumerable<TResult> Enumerable(string connectionName)
        {
                var connectionStringSettings = ConfigurationManager.ConnectionStrings[_connectionStringName];

                //todo: default to sql server
                var factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

                using (var connection = factory.CreateConnection())
                {
                    connection.Open();

                    foreach (var result in connection.Query<TResult>(Sql))
                        yield return result;
                }
        }

        protected abstract string Sql { get; }



        private readonly string _connectionStringName;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
    }
}