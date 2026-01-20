namespace BCKFreightTMS.Web.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Hosting;

    public sealed class SeleniumServerFactory : WebApplicationFactory<TestStartup>
    {
        public string RootUri { get; private set; } = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set the content root to the actual web project directory
            // The WebApplicationFactory needs to find the web project's wwwroot and other assets
            // We need to find the path relative to the test project
            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var testProjectDir = Path.GetDirectoryName(testAssemblyPath);

            var contentRoot = Path.GetFullPath(Path.Combine(testProjectDir!, "..", "..", "..", "..", "..", "Web", "BCKFreightTMS.Web"));

            if (Directory.Exists(contentRoot))
            {
                builder.UseContentRoot(contentRoot);
            }

            base.ConfigureWebHost(builder);
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>()
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
