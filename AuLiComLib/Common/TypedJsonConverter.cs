using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public abstract class TypedJsonConverter<T> : Newtonsoft.Json.JsonConverter<T>
    {
        protected TypedJsonConverter(params object[] parameters)
        {
            _constructorByTypeName = new();
            _parameters = parameters;
        }

        protected void Register<TConcrete>() where TConcrete : T => _constructorByTypeName[typeof(TConcrete).Name] = typeof(TConcrete);

        private readonly Dictionary<string, Type> _constructorByTypeName;
        private readonly object[] _parameters;

        // JSON Converter

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            string? typeName = jsonObject.Value<string>("$type");
            T result = Create(typeName);
            serializer.Populate(jsonObject.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        // Core functionality for use even without JSON

        public T Create(string? typeName)
        {
            T result;
            if (typeName != null
                && _constructorByTypeName.TryGetValue(typeName, out Type concreteType))
            {
                result = (T)Activator.CreateInstance(concreteType, _parameters);
            }
            else
            {
                throw new JsonSerializationException($"Invalid type '{typeName}'");
            }
            return result;
        }
    }
}
