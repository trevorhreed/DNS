using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace jdb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public object People()
		{
			Stopwatch watch = Stopwatch.StartNew();
			var json = "[";
			var DIR = Path.Combine(Request.PhysicalApplicationPath, "App_Data/people");
			foreach (string file in Directory.EnumerateFiles(DIR))
			{
				if (json != "[") json += ",";
				json += System.IO.File.ReadAllText(file);
				
			}
			json += "]";
			var time = (float)watch.ElapsedMilliseconds / (float)1000;
			json = "{ \"time\": " + time + ", \"data\": " + json + " }";
			return json;
		}
    }
}