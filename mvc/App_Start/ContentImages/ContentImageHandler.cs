using System.Web;

namespace mvc.App_Start
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