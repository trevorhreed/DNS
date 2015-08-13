using jdb.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace jdb.Controllers
{
	[RoutePrefix("data")]
    public class DataController : ApiController
    {
		private static readonly string DATA_FILE = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data/data.json");

		[HttpGet]
		[Route("{*path?}")]
		public object Get(string path = "")
		{
			return new JsonNode(DATA_FILE).Get(path).Value();
		}
		[HttpPost]
		[Route("{*path?}")]
		public object Post(string path = "")
		{
			return new JsonNode(DATA_FILE).Post(path, getBody()).Value();
		}
		[HttpPut]
		[Route("{*path?}")]
		public object Put(string path = "")
		{
			return new JsonNode(DATA_FILE).Put(path, getBody()).Value();
		}
		[HttpDelete]
		[Route("{*path?}")]
		public object Delete(string path = "")
		{
			return new JsonNode(DATA_FILE).Delete(path).Value();
		}

		private string getBody(){
			return new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
		}
    }
}
