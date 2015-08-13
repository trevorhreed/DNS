using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(angular.Startup))]
namespace angular
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ConfigureAuth(app);
        }
    }
}
