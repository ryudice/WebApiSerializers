using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using WebApiSerializers.Tests.Entity;

namespace WebApiSerializers.Tests
{
    public class when_serializing_an_entity
    {

        private Establish conext = () =>
        {
            WebApiSerializers.

            Subject = new WebApiSerializersMediaTypeFormatter();
            Employee = new Employee();
        };

        public static Employee Employee { get; set; }

        private Because of = () =>
        {
            stream= new MemoryStream();
            Subject.WriteToStreamAsync(typeof(Employee),Employee, stream,null,)
        };

        private static MemoryStream stream;


        public static WebApiSerializersMediaTypeFormatter Subject { get; set; }
    }
}
