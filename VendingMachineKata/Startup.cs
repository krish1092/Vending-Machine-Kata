using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VendingMachineKata.Startup))]
namespace VendingMachineKata
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
