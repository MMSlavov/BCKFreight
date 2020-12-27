using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BCKFreightTMS.Web.Areas.Identity.IdentityHostingStartup))]

namespace BCKFreightTMS.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
