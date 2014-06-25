using System;
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
            cache.PopulateFromAssembly(configuration.Assembly);



        }





    }
}
