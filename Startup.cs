using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShallvaMVC.Startup))]
namespace ShallvaMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
