namespace Fanda.Shared
{
    public class AppSettings
    {
        public string DatabaseType { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public FandaSettings FandaSettings { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string MsSqlConnection { get; set; }
        public string MySqlConnection { get; set; }
        public string PgSqlConnection { get; set; }
    }

    public class FandaSettings
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Secret { get; set; }
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
