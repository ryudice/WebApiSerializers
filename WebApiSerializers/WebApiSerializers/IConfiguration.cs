using System.Reflection;

namespace WebApiSerializers
{
    public interface IConfiguration
    {
        void UseSerializersFromAssembly(Assembly assembly);

    }

    class Configuration : IConfiguration
    {
        internal Assembly Assembly { get; set; }

        public void UseSerializersFromAssembly(Assembly assembly)
        {
            Assembly = assembly;
            
        }
    }
}