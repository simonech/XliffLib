using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XliffLib.Model;

namespace XliffLib.Utils
{
    public static class JsonExtensions
    {
        public static string ToJson(this Bundle bundle)
        {

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(bundle, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore                
            });
        }

        public static Bundle ToBundle(this string jsonString)
        {
            return JsonConvert.DeserializeObject<Bundle>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
