using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace angular.Models
{
    public class Person : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<String> Phones { get; set; }
        public Address Address { get; set; }
    }
}