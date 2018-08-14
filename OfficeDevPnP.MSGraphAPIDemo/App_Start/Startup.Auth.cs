using Owin;
using OfficeDevPnP.MSGraphAPI.Infrastructure.Components;

namespace OfficeDevPnP.MSGraphAPIDemo
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            StartupHelper.ConfigureAuth(app);
            
        }
    }
}
