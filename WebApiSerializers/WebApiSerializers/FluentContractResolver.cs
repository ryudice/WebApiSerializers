using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiSerializers
{
    public class FluentContractResolver : DefaultContractResolver
    {
        private readonly List<string> _attributes;

        public FluentContractResolver(List<string> attributes)
        {
            _attributes = attributes;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            jsonProperty.Ignored = !_attributes.Contains(jsonProperty.PropertyName);

            return jsonProperty;

        }
    }
}