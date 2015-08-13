using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
			string DIR_PATH = Path.Combine(Request.PhysicalApplicationPath, "App_Data/people");

			Stopwatch watch = Stopwatch.StartNew();
			var content1 = ReadAllFilesSync(DIR_PATH);
			var time1 = (float)watch.ElapsedMilliseconds / (float)1000;
			watch.Restart();
			var content2 = ReadAllFilesAsync(DIR_PATH);
			var time2 = (float)watch.ElapsedMilliseconds / (float)1000;

			Dictionary<string, dynamic> json = new Dictionary<string, dynamic>();
			json.Add("Sync", time1);
			json.Add("Async", time2);
			return new JavaScriptSerializer().Serialize(json);
		}

		private string ReadAllFilesSync(string directoryPath)
		{
			List<string> contents = new List<string>();
			string[] files = Directory.GetFiles(directoryPath);
			for (var i = 0; i < files.Length; i++)
			{
				contents.Add(System.IO.File.ReadAllText(files[i]));
			}
			return "[" + string.Join(",", contents) + "]";
		}

		private async Task<string> ReadFileAsync(string file)
		{
			string content = "";
			using (FileStream stream = System.IO.File.Open(file, FileMode.Open))
			{
				byte[] result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
				content = System.Text.Encoding.UTF8.GetString(result);
			}
			return content;
		}

		private async Task<string> ReadAllFilesAsync(string directoryPath)
		{
			List<string> contents = new List<string>();
			string[] files = Directory.GetFiles(directoryPath);
			Task<string>[] tasks = new Task<string>[files.Length];
			for(var i=0; i < files.Length; i++)
			{
				tasks[i] = ReadFileAsync(files[i]);
			}
			for (var i = 0; i < tasks.Length; i++)
			{
				await tasks[i];
				contents.Add(tasks[i].Result);
			}
			return "[" + string.Join(",", contents) + "]";
		}

    }
}