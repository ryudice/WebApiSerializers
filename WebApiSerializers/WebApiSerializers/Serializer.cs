using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace WebApiSerializers
{
    public interface ISerializer
    {
        IContractResolver GetContractResolver();
    }

    public class Serializer<T> : ISerializer where T: class
    {
        private List<JsonAttribute> _attributes = new List<JsonAttribute>();

        public IAttributeBuilder Attribute(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            //TODO mover to utils later
            var memberExpression = property.Body as MemberExpression;
            if (memberExpression != null)
            {
                var jsonAttribute = new JsonAttribute()
                {
                    Name = memberExpression.Member.Name
                };
                _attributes.Add(jsonAttribute);
                return new AttributeBuilder(jsonAttribute);
                
            }
            throw new ArgumentException("Not a valid member expression", "property");
        }


        public IContractResolver GetContractResolver()
        {
            return new FluentContractResolver(_attributes);

        }

    }

    public interface IAttributeBuilder
    {
        IAttributeBuilder As(string name);

        IAttributeBuilder DefaultValue(object value);

    }


    public class JsonAttribute
    {

        public string Name { get; set; }
        public string As { get; set; }
        public object DefaultValue { get; set; }
    }

    internal class AttributeBuilder : IAttributeBuilder
    {
        private readonly JsonAttribute _jsonAttribute;

        public AttributeBuilder(JsonAttribute jsonAttribute)
        {
            _jsonAttribute = jsonAttribute;
        }

        public JsonAttribute Attribute
        {
            get { return _jsonAttribute; }
        }


        public IAttributeBuilder As(string name)
        {
            Attribute.As = name;
            return this;
        }

        public IAttributeBuilder DefaultValue(object value)
        {
            Attribute.DefaultValue = value;
            return this;
        }
    }
}
