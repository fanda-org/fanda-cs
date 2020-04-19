using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FandaTabler.Converters
{
    public class JsonStringTrimConverter : JsonConverter<string>
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetString()?.Trim();

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    if (reader.TokenType == JsonToken.String)
        //        if (reader.Value != null)
        //            return (reader.Value as string).Trim();

        //    return reader.Value;
        //}

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            => writer.WriteStringValue(value?.Trim());

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    var text = (string)value;
        //    if (text == null)
        //        writer.WriteNull();
        //    else
        //        writer.WriteValue(text.Trim());
        //}
    }
}
