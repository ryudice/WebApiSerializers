using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSerializers
{
    public class WebApiSerializers
    {
        internal static IConfiguration Configuration;


        public static void Configure(Action<IConfiguration> builder)
        {
            var configuration = new Configuration();

            builder(configuration);

            Configuration = configuration;

            var cache = new SerializersCache();
            if (configuration.Assembly != null)
                cache.PopulateFromAssembly(configuration.Assembly);

            cache.AddSerializers(configuration.Serializers);


        }





    }
}
