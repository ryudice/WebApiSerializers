using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using Newtonsoft.Json;
using WebApiSerializers.Tests.Entity;

namespace WebApiSerializers.Tests
{
    public class when_serializing_an_entity_with_standard_attributes
    {

        private Establish conext = () =>
        {
            WebApiSerializers.Configure(configuration =>
            {
                configuration.UseSerializersFromAssembly(Assembly.GetExecutingAssembly());
            });

            Subject = new WebApiSerializersMediaTypeFormatter();
            Employee = new Employee();
        };

        public static Employee Employee { get; set; }

        private Because of = () =>
        {
            stream= new MemoryStream();
            Task writeToStreamAsync = Subject.WriteToStreamAsync(typeof(Employee),Employee, stream,null,null);
           
        };

        private It serializes_the_attributes = () =>
        {
            var json = Encoding.UTF8.GetString(stream.ToArray());

            json.ShouldContain("FirstName");
        };

        private It serializes_attributes_with_custom_name = () =>
        {
            var json = Encoding.UTF8.GetString(stream.ToArray());

            json.ShouldContain("FullName");
        };


        private It should_use_default_values = () =>
        {
            var json = Encoding.UTF8.GetString(stream.ToArray());

            dynamic deserialized = JsonConvert.DeserializeObject(json);

            ((string)deserialized.LastName).ShouldEqual("Martinez");

        };

        private static MemoryStream stream;


        public static WebApiSerializersMediaTypeFormatter Subject { get; set; }
    }

    


}
