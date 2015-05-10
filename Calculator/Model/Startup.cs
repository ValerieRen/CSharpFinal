using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Model.Startup))]
namespace Model
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
