using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FXPrototype.Startup))]
namespace FXPrototype
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
