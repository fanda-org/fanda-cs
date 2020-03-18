using DataTables.Queryable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FandaCoreUI.Extensions
{
    public static class JsonPagedResult
    {
        /// <summary>
        /// Helper method that converts <see cref="IPagedList{T}"/> collection to the JSON-serialized object in datatables-friendly format.
        /// </summary>
        /// <param name="model"><see cref="IPagedList{T}"/> collection of items</param>
        /// <param name="draw">Draw counter (optional).</param>
        /// <returns>JsonNetResult instance to be sent to datatables</returns>
        public static JsonNetResult JsonDataTable<T>(this IPagedList<T> model, int draw = 0)
        {
            var result = new JsonNetResult(new DataTablePagedResult
            {
                draw = draw,
                recordsTotal = model.TotalCount,
                recordsFiltered = model.TotalCount,
                data = model
            });
            return result;
        }

        /// <summary>
        /// JsonNet-based JsonResult
        /// </summary>
        public class JsonNetResult : JsonResult
        {
            public JsonNetResult(object value) : base(value) { }
            //{
            //SerializerSettings = new JsonSerializerSettings();
            //SerializerSettings.Converters.Add(new StringEnumConverter());
            //SerializerSettings.NullValueHandling = NullValueHandling.Include;
            //}

            public override async Task ExecuteResultAsync(ActionContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                var response = context.HttpContext.Response;

                response.ContentType = !string.IsNullOrEmpty(ContentType)
                    ? ContentType
                    : "application/json";

                //if (ContentEncoding != null)
                //    response.ContentEncoding = ContentEncoding;

                string serializedObject = JsonConvert.SerializeObject(Value);
                // Formatting.Indented);
                await response.WriteAsync(serializedObject);
                return;
            }
        }
    }

    public class DataTablePagedResult
    {
#pragma warning disable IDE1006 // Naming Styles
        public int draw { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int recordsTotal { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int recordsFiltered { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public IPagedList data { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}