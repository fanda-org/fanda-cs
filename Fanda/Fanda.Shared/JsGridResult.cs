namespace Fanda.Shared
{
    public class JsGridResult<T> : PagedListBase
        where T : class
    {
        #region JsGrid information
        public T Data { get; set; } = null;
        //public int? ItemsCount { get; set; } = 0;
        #endregion

        #region Additional information
        public string Error { get; set; } = null;
        #endregion
    }
}
