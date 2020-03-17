using DataTables.Queryable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FandaNg.Extensions
{
    public static class JsonPagedResult
    {
        /// <summary>
        /// Helper method that converts <see cref="IPagedList{T}"/> collection to the JSON-serialized object in datatables-friendly format.
        /// </summary>
        /// <param name="model"><see cref="IPagedList{T}"/> collection of items</param>
        /// <param name="draw">Draw counter (optional).</param>
        /// <returns>JsonNetResult instance to be sent to datatables</returns>
        public static JsonResult JsonDataTable<T>(this IPagedList<T> model, int draw = 0)
        {
            var result = new JsonResult(new DataTablePagedResult
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
        //protected class JsonNetResult : JsonResult
        //{
        //    public JsonNetResult(object value) : base(value)
        //    {
        //        SerializerSettings = new JsonSerializerSettings();
        //        SerializerSettings.Converters.Add(new StringEnumConverter());
        //        SerializerSettings.NullValueHandling = NullValueHandling.Include;
        //    }

        //    //public override Task ExecuteResultAsync(ActionContext context)
        //    //{
        //    //    if (context == null)
        //    //        throw new ArgumentNullException("context");

        //    //    var response = context.HttpContext.Response;

        //    //    response.ContentType = !string.IsNullOrEmpty(ContentType)
        //    //        ? ContentType
        //    //        : "application/json";

        //    //    //if (ContentEncoding != null)
        //    //    //    response.ContentEncoding = ContentEncoding;

        //    //    var serializedObject = JsonConvert.SerializeObject(Value, Formatting.Indented, SerializerSettings);
        //    //    return response.WriteAsync(serializedObject);
        //    //}
        //}
    }

    public class DataTablePagedResult
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IPagedList data { get; set; }
    }
}