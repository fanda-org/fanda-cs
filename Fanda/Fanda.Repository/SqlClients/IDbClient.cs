using System;

namespace Fanda.Repository.SqlClients
{
    public interface IDbClient : IDisposable
    {
        public System.Data.Common.DbConnection Connection { get; }
    }
}
