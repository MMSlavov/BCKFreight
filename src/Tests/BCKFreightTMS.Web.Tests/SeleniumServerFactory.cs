namespace BCKFreightTMS.Web.Tests
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Hosting;

    public sealed class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public string RootUri { get; private set; } = string.Empty;

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                        .UseUrls("https://localhost:0");
                });
        }

        public string GetRootUri()
        {
            if (string.IsNullOrEmpty(this.RootUri))
            {
                var server = this.Server;
                var addresses = server.Features.Get<IServerAddressesFeature>();
                this.RootUri = addresses?.Addresses.FirstOrDefault() ?? "https://localhost";
            }

            return this.RootUri;
        }
    }
}
