using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Controllers
{
	public class ContentImageHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write("Hello World!");
			context.Response.End();
		}
	}
}