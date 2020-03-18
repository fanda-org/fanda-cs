using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FandaTabler.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value) where T : class
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            string val = session.GetString(key);
            return val == null ? null : JsonConvert.DeserializeObject<T>(val);
        }
    }
}
