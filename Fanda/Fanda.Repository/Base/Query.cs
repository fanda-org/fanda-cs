namespace Fanda.Repository.Base
{
    public class Query
    {
        public string Search { get; set; }
        public string Filter { get; set; }
        public string[] FilterArgs { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 100;
    }
}
