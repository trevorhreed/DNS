using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class FormModel
    {
        public String Name { get; set; }
		public HttpPostedFileBase Image { get; set; }
		public HttpPostedFileBase ImageThin { get; set; }
	}
}