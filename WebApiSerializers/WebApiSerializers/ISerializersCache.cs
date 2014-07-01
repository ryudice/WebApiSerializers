using System;
using System.Reflection;

namespace WebApiSerializers
{
    public interface ISerializersCache
    {
        void PopulateFromAssembly(Assembly assembly);

        

        ISerializer GetSerializerForClass(Type type);

    }
}