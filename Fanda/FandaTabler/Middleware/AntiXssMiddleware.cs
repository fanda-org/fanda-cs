using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Middleware
{
    public static class AntiXssMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiXss(
            this IApplicationBuilder builder)
        {
            var options = new AntiXssMiddlewareOptions();
            return builder.UseMiddleware<AntiXssMiddleware>(options);
        }

        public static IApplicationBuilder UseAntiXss(
            this IApplicationBuilder builder, AntiXssMiddlewareOptions options)
        {
            if (options == null)
            {
                options = new AntiXssMiddlewareOptions();
            }

            return builder.UseMiddleware<AntiXssMiddleware>(options);
        }
    }

    public class CrossSiteScriptingException : Exception
    {
        public CrossSiteScriptingException(string message) : base(message) { }
    }

    public class AntiXssMiddlewareOptions
    {
        private string _errorMessage;
        public bool ThrowExceptionIfRequestContainsCrossSiteScripting { get; set; }
        public string ErrorMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_errorMessage))
                {
                    if (ThrowExceptionIfRequestContainsCrossSiteScripting)
                    {
                        return "DANGER: Cross site scripting found";
                    }
                    else
                    {
                        return "<h1><b style='color:red;'>DANGER</b>: Cross site scripting found!</h1><h2>Application handled it safely. Now your are <b style='color:green'>SAFE!!</b></h2><a href='/'>Home</a>";
                    }
                }
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }
    }

    public class AntiXssMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AntiXssMiddlewareOptions _options;

        public AntiXssMiddleware(RequestDelegate next, AntiXssMiddlewareOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check XSS in URL
            if (!string.IsNullOrWhiteSpace(context.Request.Path.Value))
            {
                var url = context.Request.Path.Value;

                if (CrossSiteScriptingValidation.IsDangerousString(url, out int _))
                {
                    if (_options.ThrowExceptionIfRequestContainsCrossSiteScripting)
                    {
                        throw new CrossSiteScriptingException(_options.ErrorMessage);
                    }

                    context.Response.Clear();
                    await context.Response.WriteAsync(_options.ErrorMessage);
                    return;
                }
            }

            // Check XSS in query string
            if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
            {
                var queryString = HttpUtility.UrlDecode(context.Request.QueryString.Value);

                if (CrossSiteScriptingValidation.IsDangerousString(queryString, out int _))
                {
                    if (_options.ThrowExceptionIfRequestContainsCrossSiteScripting)
                    {
                        throw new CrossSiteScriptingException(_options.ErrorMessage);
                    }

                    context.Response.Clear();
                    await context.Response.WriteAsync(_options.ErrorMessage);
                    //var encoded = HttpUtility.UrlEncode(queryString);
                    //context.Request.QueryString = new QueryString(encoded);
                    return;
                }
            }

            // Check XSS in request form
            if (context.Request.HasFormContentType)
            {
                //bool dangerousValueFound = false;
                //var formDataAsString = FormDataAsString(await context.Request.ReadFormAsync());
                var formData = await context.Request.ReadFormAsync();
                //var nv = new Dictionary<string, StringValues>();
                foreach (var key in formData.Keys)
                {
                    if (formData.TryGetValue(key, out var newValue))
                    {
                        if (CrossSiteScriptingValidation.IsDangerousString(newValue, out int _))
                        {
                            //dangerousValueFound = true;
                            if (_options.ThrowExceptionIfRequestContainsCrossSiteScripting)
                            {
                                throw new CrossSiteScriptingException(_options.ErrorMessage);
                            }
                            context.Response.Clear();
                            await context.Response.WriteAsync(_options.ErrorMessage);
                            return;
                            //newValue = HttpUtility.JavaScriptStringEncode(newValue);
                        }
                    }
                    //nv.Add(key, newValue);
                }
                //context.Request.Form = new FormCollection(nv, context.Request.Form.Files);
                //if (dangerousValueFound)
                //    return;
            }

            // Check XSS in request content
            var originalBody = context.Request.Body;
            try
            {
                var content = await ReadRequestBody(context);

                if (CrossSiteScriptingValidation.IsDangerousString(content, out int _))
                {
                    if (_options.ThrowExceptionIfRequestContainsCrossSiteScripting)
                    {
                        throw new CrossSiteScriptingException(_options.ErrorMessage);
                    }

                    context.Response.Clear();
                    await context.Response.WriteAsync(_options.ErrorMessage);
                    return;
                }

                await _next(context);
            }
            finally
            {
                context.Request.Body = originalBody;
            }
        }

        private string FormDataAsString(IFormCollection formCollection)
        {
            var sb = new StringBuilder();
            foreach (var key in formCollection.Keys)
            {
                sb.Append(formCollection.TryGetValue(key, out var value) ? value.ToString() : "");
            }
            return sb.ToString();
        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            using (var buffer = new MemoryStream())
            {
                await context.Request.Body.CopyToAsync(buffer);
                context.Request.Body = buffer;
                buffer.Position = 0;

                var encoding = Encoding.UTF8;
                var contentType = context.Request.GetTypedHeaders().ContentType;
                if (contentType?.Charset != null && !string.IsNullOrEmpty(contentType?.Charset.Value))
                {
                    encoding = Encoding.GetEncoding(contentType.Charset.Value);
                }
                else if (contentType?.Encoding != null)
                {
                    encoding = Encoding.GetEncoding(contentType.Encoding.CodePage);
                }

                var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
                context.Request.Body.Position = 0;

                return requestContent;
            }
        }
    }
}
