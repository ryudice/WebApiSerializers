using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace WebApiSerializers
{
    public interface IConfiguration
    {
        void UseSerializersFromAssembly(Assembly assembly);

        void AddSerializer<T>() where T : ISerializer;
    }

    class Configuration : IConfiguration
    {
        internal Assembly Assembly { get; set; }

        internal IList<Type> Serializers { get; set; }

        public Configuration()
        {
            Serializers= new List<Type>();
        }


        public void UseSerializersFromAssembly(Assembly assembly)
        {
            Assembly = assembly;
            
        }

        public void AddSerializer<T>() where T : ISerializer
        {
            Serializers.Add(typeof(T));
        }
    }
}