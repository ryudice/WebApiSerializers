using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace WebApiSerializers
{
    internal interface ISerializer
    {
        IContractResolver GetContractResolver();
    }

    public class Serializer<T> : ISerializer where T: class
    {
        private List<string> _attributes = new List<string>(); 

        public void Attribute(Func<T, object> property )
        {
            if (property == null)
                throw new ArgumentNullException("property");



        }


        public IContractResolver GetContractResolver()
        {
            return new FluentContractResolver(_attributes);

        }

    }
}
