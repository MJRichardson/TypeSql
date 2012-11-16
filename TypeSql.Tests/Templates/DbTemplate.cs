using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace TypeSql.Tests.Templates
{
    abstract public class DbTemplate {
        
        protected readonly DbConnection _connection;
        protected readonly DbTransaction _transaction;
        protected readonly string _connectionStringKey;

        protected DbTemplate(string connectionStringKey)
        {
            _connectionStringKey = connectionStringKey;
        }

        protected DbTemplate(DbConnection connection, DbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }

        protected DbTemplate(string connectionStringKey, DbTransaction transaction = null)
        {
            _connectionStringKey = connectionStringKey;
            _transaction = transaction;
        }
        
        // todo: exception handling if the provider/connection string info is screwy
        protected DbConnection GetConnection()
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[_connectionStringKey];
            string providerName = connectionStringSetting.ProviderName;
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionStringSetting.ConnectionString;
            return connection;
        }
    }
}