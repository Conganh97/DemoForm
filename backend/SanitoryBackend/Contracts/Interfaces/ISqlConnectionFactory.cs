using System;
using System.Data;

namespace Contracts.Interfaces
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
