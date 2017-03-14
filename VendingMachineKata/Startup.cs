using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendingMachineKata
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}