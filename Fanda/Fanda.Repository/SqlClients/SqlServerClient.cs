namespace Fanda.Repository.SqlClients
{
    public class SqlServerClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }
        public SqlServerClient(string connectionString)
        {
            Connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        }
        public void Dispose() => Connection.Dispose();
    }
}
