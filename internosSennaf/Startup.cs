using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(internosSennaf.Startup))]
namespace internosSennaf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
