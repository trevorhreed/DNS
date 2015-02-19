using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace angular.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Phone> Phones { get; set; }
        public Address HomeAddress { get; set; }
        public Address WorkAddress { get; set; }
    }
}