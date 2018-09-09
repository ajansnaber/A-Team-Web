using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ANPositive.Startup))]
namespace ANPositive
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
