using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSerializers.Tests.Entity;

namespace WebApiSerializers.Tests.Serializers
{
    public class EmployeeSerializer : Serializer<Employee>
    {
        public EmployeeSerializer()
        {
            Attribute(employee => employee.FirstName);
            Attribute(employee => employee.LastName);
        }
    }
}
