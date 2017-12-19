using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OJewelryTest2.Startup))]
namespace OJewelryTest2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
