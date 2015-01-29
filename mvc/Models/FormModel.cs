using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class FormModel
    {
        public String Name { get; set; }
        public List<PhoneNumber> Contacts { get; set; }

        public FormModel()
        {
            Contacts = new List<PhoneNumber>();
        }
    }

    public class PhoneNumber
    {
        public String Number { get; set; }
        public String Extension { get; set; }
    }
}