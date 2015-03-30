using System;
using System.Web;
using System.Web.Routing;

namespace mvc.App_Start
{
	class ContentImageRouter : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new ContentImageHandler();
		}
	}
}
