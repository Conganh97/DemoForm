using System;
using System.Data;
using Contracts.Interfaces;
using Dapper;

namespace Infrastructure.Implements
{
    public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
    {
        private readonly ISqlConnectionFactory context;
        private readonly IDbConnection _connection;
        public ApplicationWriteDbConnection(ISqlConnectionFactory context)
        {
            this.context = context;
            this._connection = context.GetOpenConnection();
        }
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _connection.ExecuteAsync(sql, param, transaction);
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
    }
}
