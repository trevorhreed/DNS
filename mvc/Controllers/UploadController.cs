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
    public class UploadController : Controller
    {
        private ILog log = LogManager.GetLogger(typeof(UploadController));

        public ActionResult Index()
        {
            log.Debug("Enter Index");

            FormModel model = new FormModel();
            model.Name = "Harrison";
            PhoneNumber phone = new PhoneNumber();
            phone.Number = "801.836.3202";
            phone.Extension = "123";
            model.Contacts.Add(phone);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormModel model, HttpPostedFileBase profile, HttpPostedFileBase profileThin)
        {
            log.Debug("Enter Index Post");

            if (ModelState.IsValid)
            {
                if (profile != null && profile.ContentLength > 0)
                {
                    profile.SaveAs(@"C:\dump\" + model.Name + "_" + profile.FileName);
                }
                if (profileThin != null && profileThin.ContentLength > 0)
                {
                    profileThin.SaveAs(@"C:\dump\" + model.Name + "_" + profileThin.FileName);
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Name: " + model.Name);
                sb.AppendLine();
                sb.AppendLine("Contacts:");
                sb.AppendLine("=========");
                foreach (PhoneNumber phone in model.Contacts)
                {
                    sb.Append("Phone Number: ");
                    sb.Append(phone.Number);
                    sb.Append(" ext: ");
                    sb.AppendLine(phone.Extension);
                }
                sb.AppendLine();
                using (StreamWriter outfile = new StreamWriter(@"C:\dump\" + model.Name + "_info.txt"))
                {
                    outfile.Write(sb.ToString());
                }
            }

            return RedirectToAction("Index");
        }
    }
}