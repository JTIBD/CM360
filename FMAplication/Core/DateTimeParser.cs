using Newtonsoft.Json;
using System;

namespace FMAplication.Core
{
    public class DateTimeParser : Newtonsoft.Json.JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is DateTime value)
            {
                return value.ToUniversalTime();
            }

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
