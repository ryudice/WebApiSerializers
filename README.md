WebApiSerializers
=================

Fluent and extensible webapi serializers. Based on rails serializers, inspired by fluent nhibernate.

Still under development, very basic functionality works. Tests are there, you are very welcome to contribute if you like the concept.

Configuration
======
Either in global.asax or wherever you bootstrap your application add this:

```cs
 WebApiSerializers.Configure(configuration =>
            {
                configuration.UseSerializersFromAssembly(Assembly.GetExecutingAssembly());
            });

```


Creating a serializer for a class
-------------------

The following is an example of a serializer. It has to be public so that it gets picked up by the assembly scanner.

```cs
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
```
