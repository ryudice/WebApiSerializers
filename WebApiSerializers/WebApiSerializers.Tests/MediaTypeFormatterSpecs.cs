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
using WebApiSerializers.Tests.Serializers;

namespace WebApiSerializers.Tests
{
    public class when_serializing_relationships
    {
        private Establish context = () =>
        {
            WebApiSerializers.Configure(configuration =>
            {
                configuration.AddSerializer<EmployeeRelationshipSerializer>();
            });

            Subject = new WebApiSerializersMediaTypeFormatter();
            Employee = new Employee()
            {
                FirstName = "Mario",
                LastName = "Martinez",
                Name = "Mario Martinez",
                Companies =  new []{ new Company()
                {
                    Id=1,
                    Name = "Facebook"
                }, new Company()
                {
                    Id = 2,
                    Name = "Google"
                } }
            };
        };

        private Because of = () =>
        {
            stream = new MemoryStream();
            Task writeToStreamAsync = Subject.WriteToStreamAsync(typeof(Employee), Employee, stream, null, null);

            json = Encoding.UTF8.GetString(stream.ToArray());
            
        };

        private It should_serialize_the_relation_ship = () =>
        {
            Console.WriteLine("Employee JSON: {0}", json);
            json.ShouldContain("Companies");
        } ;

        private It should_use_custom_mapping;
        private static MemoryStream stream;
        private static string json;

        public static WebApiSerializersMediaTypeFormatter Subject { get; set; }

        public static Employee Employee { get; set; }
    }

    public class when_deserializing_an_entity
    {

        private Establish conext = () =>
        {
            WebApiSerializers.Configure(configuration =>
            {
                configuration.UseSerializersFromAssembly(Assembly.GetExecutingAssembly());
            });

            Subject = new WebApiSerializersMediaTypeFormatter();
            Employee = new Employee()
            {
                FirstName = "Mario",
                LastName = "Martinez",
                Name = "Mario Martinez" 
            };
        };

        private Because of = () =>
        {
            stream = new MemoryStream();
            Task writeToStreamAsync = Subject.WriteToStreamAsync(typeof(Employee), Employee, stream, null, null);

            json = Encoding.UTF8.GetString(stream.ToArray());

        };

        private It should_deserialize_back_from_json = () =>
        {
            Console.WriteLine("Employee JSON: {0}", json);

            var bytes = Encoding.UTF8.GetBytes(json);

            var result = Subject.ReadFromStreamAsync(typeof(Employee), new MemoryStream(bytes),null,null  );

            var employee = result.Result as Employee;

            employee.FirstName.ShouldEqual("Mario");
            employee.LastName.ShouldEqual("Martinez");
            employee.Name.ShouldEqual("Mario Martinez");

        };



        private static MemoryStream stream;
        private static string json;
        public static Employee Employee { get; set; }
        public static WebApiSerializersMediaTypeFormatter Subject { get; set; }

    }

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
        public static Employee Employee { get; set; }
        public static WebApiSerializersMediaTypeFormatter Subject { get; set; }
    }

    


}
