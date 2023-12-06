using System;
using System.Data;
using System.Data.SqlClient;
using Contracts.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Implements
{
    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly ISqlConnectionFactory context;
        private readonly IDbConnection _connection;
 
        
        public ApplicationReadDbConnection(ISqlConnectionFactory context)
        {
            this.context = context;
            this._connection = context.GetOpenConnection();
        }
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await _connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _connection.QuerySingleAsync<T>(sql, param, transaction);
        }
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
