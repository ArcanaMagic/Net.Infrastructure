using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Net.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private static readonly JsonSerializerOptions IgnoreNullJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };

        private static readonly JsonSerializerSettings CurrentJsonSerializerSettings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string ToJson(this object obj, bool ignoreNullValues = true)
        {
            return ignoreNullValues 
                ? JsonSerializer.Serialize(obj, IgnoreNullJsonOptions) 
                : JsonSerializer.Serialize(obj);
        }

        public static string ToJson(this string text, bool ignoreNullValues = true)
        {
            return ignoreNullValues
                ? JsonSerializer.Serialize(text, IgnoreNullJsonOptions)
                : JsonSerializer.Serialize(text);
        }

        public static string ToJsonByNewtonsoft(this object obj, bool ignoreNullValues = true)
        {
            return ignoreNullValues
                ? JsonSerializer.Serialize(obj, IgnoreNullJsonOptions)
                : JsonSerializer.Serialize(obj);
        }

        public static string ToJsonByNewtonsoft(this string text, bool ignoreNullValues = true)
        {
            return ignoreNullValues
                ? JsonConvert.SerializeObject(text, CurrentJsonSerializerSettings)
                : JsonConvert.SerializeObject(text);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}