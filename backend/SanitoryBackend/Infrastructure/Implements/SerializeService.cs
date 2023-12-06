﻿using Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Implements
{
    public class SerializeService : ISerializeService
    {
        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert
                .SerializeObject(obj,
                new JsonSerializerSettings {
                    ContractResolver =
                        new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters =
                        new List<JsonConverter> {
                            new StringEnumConverter {
                                NamingStrategy = new CamelCaseNamingStrategy()
                            }
                        },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        public string Serialize<T>(T obj, Type type)
        {
            return JsonConvert
                .SerializeObject(obj, type, new JsonSerializerSettings());
        }
    }
}