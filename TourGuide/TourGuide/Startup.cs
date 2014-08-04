using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TourGuide.Startup))]
namespace TourGuide
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
