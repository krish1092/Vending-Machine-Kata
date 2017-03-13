using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VendingMachine.Startup))]
namespace VendingMachine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
