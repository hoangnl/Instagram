using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Instagram.Startup))]
namespace Instagram
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
