using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiSerializers
{
    public class FluentContractResolver : DefaultContractResolver
    {
        private readonly List<JsonAttribute> _attributes;

        public FluentContractResolver(List<JsonAttribute> attributes)
        {
            _attributes = attributes;
        }


    

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
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

            if (jsonAttribute.DefaultValue != null)
            {            
                jsonProperty.DefaultValue = jsonAttribute.DefaultValue;
                jsonProperty.NullValueHandling = NullValueHandling.Include;   
            }
                

            

            return jsonProperty;

        }
    }
}