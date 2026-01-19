namespace BCKFreightTMS.Web.Tests
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public SeleniumServerFactory()
        {
            this.ClientOptions.BaseAddress = new Uri("https://localhost");
            var builder = WebApplication.CreateBuilder(Array.Empty<string>());
            var app = builder.Build();
            _ = app.StartAsync();
            this.RootUri = "https://localhost";
        }

        public string RootUri { get; set; }

        public class FakeStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure()
            {
            }
        }
    }
}
