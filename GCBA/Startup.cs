using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GCBA.Startup))]
namespace GCBA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
