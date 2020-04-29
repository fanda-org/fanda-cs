namespace Fanda.Shared
{
    public class JsGridResult<T>
        where T : class
    {
        #region JsGrid information
        public T Data { get; set; } = null;
        public int? ItemsCount { get; set; } = -1;
        #endregion

        #region Additional information
        public string Error { get; set; } = null;
        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public int? FirstRowOnPage { get; set; }
        public int? LastRowOnPage { get; set; }
        #endregion
    }
}
