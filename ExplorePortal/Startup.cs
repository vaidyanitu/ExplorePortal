using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExplorePortal.Startup))]
namespace ExplorePortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
