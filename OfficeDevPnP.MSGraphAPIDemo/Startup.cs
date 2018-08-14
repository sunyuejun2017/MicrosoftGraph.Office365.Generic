using Owin;

namespace OfficeDevPnP.MSGraphAPIDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
