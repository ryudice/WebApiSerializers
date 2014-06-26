using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using WebApiSerializers.Tests.Entity;

namespace WebApiSerializers.Tests
{
    public class when_populating_from_assembly
    {

        private Establish context = () =>
        {
            Subject = new SerializersCache();

        };

        private Because of = () =>
        {
           Subject.PopulateFromAssembly(Assembly.GetExecutingAssembly());
        };

        private It should_return_a_serializer_for_type = () =>
        {
            Subject.GetSerializerForClass(typeof (Employee)).ShouldNotBeNull();
        };


        private static SerializersCache Subject;

    }
}
