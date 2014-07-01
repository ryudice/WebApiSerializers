using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using WebApiSerializers.Helpers;

namespace WebApiSerializers
{
    public interface ISerializer
    {
        IContractResolver GetContractResolver();
    }

    public class Serializer<T> : ISerializer where T : class
    {
        private List<MappedAttributeBase> _attributes = new List<MappedAttributeBase>();
        //private List<HasManyAttributeBase> _hasManyAttributes= new List<HasManyAttributeBase>();

        public IAttributeBuilder Attribute(Expression<Func<T, object>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var propertyName = Utils.GetPropertyNameFromExpression(property);

            var jsonAttribute = new JsonAttribute()
            {
                Name = propertyName
            };
            _attributes.Add(jsonAttribute);
            return new AttributeBuilder(jsonAttribute);
        }


        public IRelationShipBuilder<TProperty> HasMany<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> property)
            where TProperty : class
        {
            var hasManyAttribute = new HasManyAttribute<TProperty>()
            {
                Name = Utils.GetPropertyNameFromExpression(property)
            };
            _attributes.Add(hasManyAttribute);

            return new RelationShipBuilder<TProperty>(hasManyAttribute);
        }

        public IContractResolver GetContractResolver()
        {
            return new FluentContractResolver(_attributes, typeof(T));
        }
    }

    internal class HasManyAttributeBase : MappedAttributeBase
    {
        
    }

    internal class HasManyAttribute<T> : HasManyAttributeBase
    {
        public Func<T, object>  Map { get; set; }

    }


    public interface IAttributeBuilder
    {
        IAttributeBuilder As(string name);

        IAttributeBuilder DefaultValue(object value);
    }


    public class MappedAttributeBase
    {
        public string Name { get; set; }
        public string As { get; set; }
    }

    public class JsonAttribute : MappedAttributeBase
    {
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