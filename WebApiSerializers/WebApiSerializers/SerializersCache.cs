using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiSerializers
{
    public class SerializersCache : ISerializersCache
    {

        internal static SerializersCache Current { get; private set; }

        Dictionary<Type, ISerializer> serializers = new Dictionary<Type, ISerializer>();

        public SerializersCache()
        {
            Current = this;
        }

        public void PopulateFromAssembly(Assembly assembly)
        {
            serializers = assembly.GetExportedTypes().Where(type => type.GetGenericTypeDefinition() == typeof(Serializer<>)).ToDictionary(type => type.GenericTypeArguments[0], type => (ISerializer) Activator.CreateInstance(type) );
        }

        public void SerializerFactory(Func<ISerializer> factory)
        {
            throw new NotImplementedException();
        }

        public ISerializer GetSerializerForClass(Type type)
        {
            ISerializer value;
            serializers.TryGetValue(type, out value);
            return value;

        }
    }
}