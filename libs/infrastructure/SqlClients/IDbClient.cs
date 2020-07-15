using System;

namespace Fanda.Infrastructure.SqlClients
{
    public interface IDbClient : IDisposable
    {
        public System.Data.Common.DbConnection Connection { get; }
    }
}
