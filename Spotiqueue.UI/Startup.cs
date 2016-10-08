using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Spotiqueue.UI.Startup))]
namespace Spotiqueue.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
