using System;
using System.Reflection;

namespace WebApiSerializers
{
    public interface ISerializersCache
    {
        void PopulateFromAssembly(Assembly assembly);

        void SerializerFactory(Func<ISerializer> factory);

        ISerializer GetSerializerForClass(Type type);

    }
}