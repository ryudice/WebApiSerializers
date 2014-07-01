using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiSerializers
{
    public class FluentContractResolver : DefaultContractResolver
    {
        private readonly List<MappedAttributeBase> _attributes;
        private readonly Type _type;

        

        public FluentContractResolver(List<MappedAttributeBase> attributes, Type type)
        {
            _attributes = attributes;
            _type = type;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            
            if (member.DeclaringType != _type)
                return jsonProperty;

            var jsonAttribute = _attributes.SingleOrDefault(attribute => attribute.Name == jsonProperty.PropertyName );
            
            if (jsonAttribute == null)
            {
                jsonProperty.Ignored = true;
                return jsonProperty;
            }

            if (jsonAttribute.As != null)
            {
                jsonProperty.PropertyName = jsonAttribute.As;
            }

            if (jsonAttribute is HasManyAttributeBase)
            {
                var genericTypeDefinition = (member as PropertyInfo).PropertyType.GenericTypeArguments[0];
                var makeGenericType = typeof(HasManyValueProvider<>).MakeGenericType(genericTypeDefinition);
                var valueProvider = (IValueProvider) Activator.CreateInstance(makeGenericType, member, jsonAttribute);
                jsonProperty.ValueProvider = valueProvider;

            }

            //if (jsonAttribute.DefaultValue != null)
            //{            
            //    jsonProperty.DefaultValue = jsonAttribute.DefaultValue;
            //    jsonProperty.NullValueHandling = NullValueHandling.Include;   
            //}
                

            

            return jsonProperty;

        }
    }


    class HasManyValueProvider<T> : IValueProvider
    {
        private readonly HasManyAttribute<T> _hasManyAttribute;
        private ReflectionValueProvider _reflectedValueProvider;

        public HasManyValueProvider(MemberInfo memberInfo, HasManyAttribute<T> hasManyAttribute) 
        {
            _hasManyAttribute = hasManyAttribute;
            _reflectedValueProvider = new ReflectionValueProvider(memberInfo);
        }

        public new void SetValue(object target, object value)
        {
            _reflectedValueProvider.SetValue(target, value);
        }

        public new object GetValue(object target)
        {
            if (_hasManyAttribute.Map !=null)
            {
                var value = _reflectedValueProvider.GetValue(target);
                return (value as IEnumerable<T>).Select(_hasManyAttribute.Map);
            }

            return _reflectedValueProvider.GetValue(target);
        }
    }



}