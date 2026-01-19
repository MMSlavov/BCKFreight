namespace BCKFreightTMS.Web.Tests
{
    using System;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IAsyncLifetime
    {
        private readonly SeleniumServerFactory<Startup> server;
        private IWebDriver? browser;

        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
        }

        public async Task InitializeAsync()
        {
            // Wait for server to be ready
            await Task.Delay(500);
            
            var opts = new ChromeOptions();
            opts.AddArguments("--headless");
            opts.AddArguments("--no-sandbox");
            opts.AddArguments("--disable-dev-shm-usage");
            opts.AcceptInsecureCertificates = true;
            this.browser = new ChromeDriver(opts);
        }

        public Task DisposeAsync()
        {
            this.browser?.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public void ContactsDatatableContainsData()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Contacts");
            Assert.NotEmpty(this.browser.FindElements(By.TagName("div")));
        }
    }
}
