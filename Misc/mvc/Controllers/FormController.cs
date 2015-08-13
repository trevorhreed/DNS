using log4net;
using mvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace mvc.Controllers
{
    public class FormController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(FormController));

        public ActionResult Index()
        {
            log.Debug("Enter Index");

            FormModel model = new FormModel();
            model.Name = "Harrison";
			
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormModel model)
        {
            log.Debug("Enter Index Post");

            if (ModelState.IsValid)
            {
                if (model.Image != null && model.Image.ContentLength > 0)
                {
                    model.Image.SaveAs(@"C:\dump\" + model.Name + "_" + model.Image.FileName);
                }
				if (model.ImageThin != null && model.ImageThin.ContentLength > 0)
				{
					model.ImageThin.SaveAs(@"C:\dump\" + model.Name + "_thin_" + model.ImageThin.FileName);
				}

				StringBuilder sb = new StringBuilder();
                sb.AppendLine("Name: " + model.Name);
                sb.AppendLine();
                using (StreamWriter outfile = new StreamWriter(@"C:\dump\" + model.Name + "_info.txt"))
                {
                    outfile.Write(sb.ToString());
                }
            }

            return RedirectToAction("Index");
        }

		public ActionResult NewSocialLink()
		{
			return PartialView("_SocialLinkForm");
		}
    }
}