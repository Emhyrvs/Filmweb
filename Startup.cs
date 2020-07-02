using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Filmweb.Startup))]
namespace Filmweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
