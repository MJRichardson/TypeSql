using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;

namespace TypeSql
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

        protected IEnumerable<TResult> Execute(object parameters =null, bool buffered=true)
        {
            if (_connectionStringName != null)
            {
                var enumerable = CreateConnectionAndEnumerate(_connectionStringName, parameters);
                return buffered ? enumerable.ToList() : enumerable;
            }

            return _connection.Query<TResult>(Sql, transaction: _transaction, param: parameters, buffered: buffered);
        }

        private IEnumerable<TResult> CreateConnectionAndEnumerate(string connectionStringName, object parameters)
        {
                var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

                //todo: default to sql server
                var factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

                using (var connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionStringSettings.ConnectionString;
                    connection.Open();

                    foreach (var result in connection.Query<TResult>(Sql, parameters))
                        yield return result;
                }
        }

        protected abstract string Sql { get; }



        private readonly string _connectionStringName;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
    }
}