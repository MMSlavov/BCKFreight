namespace BCKFreightTMS.Web.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>
    {
        private readonly SeleniumServerFactory<Startup> server;
        private readonly IWebDriver browser;

        // Be sure that selenium-server-standalone-3.141.59.jar is running
        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArguments("--headless");
            opts.AcceptInsecureCertificates = true;
            this.browser = new ChromeDriver(opts);
        }

        [Fact]
        public void ContactsDatatableContainsData()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri + "/Contacts");
            Assert.NotEmpty(this.browser.FindElements(By.TagName("div")));
        }
    }
}
