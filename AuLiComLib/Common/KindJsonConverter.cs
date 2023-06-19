using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public abstract class KindJsonConverter<T> : Newtonsoft.Json.JsonConverter<T>
    {
        protected KindJsonConverter(params object[] parameters)
        {
            _constructorByKind = new();
            _parameters = parameters;
        }

        protected void Register<TConcrete>() where TConcrete : T => _constructorByKind[typeof(TConcrete).Name] = typeof(TConcrete);

        private readonly Dictionary<string, Type> _constructorByKind;
        private readonly object[] _parameters;

        // JSON Converter

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            string? kind = jsonObject.Value<string>("Kind");
            T result;
            if (kind != null
                && _constructorByKind.TryGetValue(kind, out Type concreteType))
            {
                result = (T)Activator.CreateInstance(concreteType, _parameters);
            }
            else
            {
                throw new NotSupportedException($"Invalid kind '{kind}'");
            }
            serializer.Populate(jsonObject.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
