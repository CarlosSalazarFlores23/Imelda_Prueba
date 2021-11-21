using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TDV_Carlos.Startup))]
namespace TDV_Carlos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
