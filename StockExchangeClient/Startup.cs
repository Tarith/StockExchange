using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockExchangeClient.Startup))]
namespace StockExchangeClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
