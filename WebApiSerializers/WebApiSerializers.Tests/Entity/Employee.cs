using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSerializers.Tests.Entity
{
    public class Employee
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public IList<Company> Companies{ get; set; }

        

    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
