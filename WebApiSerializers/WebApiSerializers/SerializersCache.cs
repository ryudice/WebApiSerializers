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
            var filterAssemblies = assembly.GetExportedTypes().Where(type => (type.BaseType !=null && type.BaseType.IsGenericType) && type.BaseType.GetGenericTypeDefinition() == typeof(Serializer<>) );
            serializers = filterAssemblies.ToDictionary(type => type.BaseType.GenericTypeArguments[0], type => (ISerializer) Activator.CreateInstance(type) );
        }

 

        public ISerializer GetSerializerForClass(Type type)
        {
            ISerializer value;
            serializers.TryGetValue(type, out value);
            return value;

        }

        public void AddSerializers(IEnumerable<Type> list)
        {
            var dictionary = list.ToDictionary(type => type.BaseType.GenericTypeArguments[0], type => (ISerializer)Activator.CreateInstance(type));

            foreach (var serializer in dictionary)
                serializers.Add(serializer.Key, serializer.Value);
        }
    }
}