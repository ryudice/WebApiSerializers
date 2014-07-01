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
            Attribute(employee => employee.LastName).DefaultValue("Martinez");
            Attribute(employee => employee.Name).As("FullName");

        }
    }

    internal class EmployeeRelationshipSerializer : Serializer<Employee>
    {
        public EmployeeRelationshipSerializer()
        {
            Attribute(employee => employee.FirstName);
            Attribute(employee => employee.LastName).DefaultValue("Martinez");
            Attribute(employee => employee.Name).As("FullName");
            HasMany(employee => employee.Companies).Map(company => new {  name = company.Name });

        }
    }

}
