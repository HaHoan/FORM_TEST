using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UMCFormTest.Startup))]
namespace UMCFormTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
