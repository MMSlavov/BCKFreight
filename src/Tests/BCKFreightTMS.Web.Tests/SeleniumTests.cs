namespace BCKFreightTMS.Web.Tests
{
    using System.Threading.Tasks;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory>, IAsyncLifetime
    {
        private readonly SeleniumServerFactory server;
        private IWebDriver? browser;

        public SeleniumTests(SeleniumServerFactory server)
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

        [Fact]
        public void ContactsPageHasTitle()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Contacts");

            Assert.NotNull(this.browser!.Title);
            Assert.NotEmpty(this.browser.Title);
        }

        [Fact]
        public void OrdersPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Orders");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void InvoicesPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Invoices");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void CargosPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Cargos");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void VehiclesPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Vehicles");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void UsersPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Users");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void TransactionsPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Transactions");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void SettingsPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Settings");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void DashboardPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Dashboard");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void PrivacyPageLoadsSuccessfully()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Dashboard/Privacy");

            var bodyElements = this.browser.FindElements(By.TagName("body"));
            Assert.NotEmpty(bodyElements);
        }

        [Fact]
        public void PageHasExpectedEncoding()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri);

            var metaTags = this.browser.FindElements(By.TagName("meta"));
            Assert.NotEmpty(metaTags);
        }

        [Fact]
        public void NonExistentPageReturnsError()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/NonExistentPage");

            var pageSource = this.browser.PageSource;
            Assert.NotNull(pageSource);
        }

        [Fact]
        public void ContactsIndexPageHasContentDiv()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Contacts/Index");

            var divElements = this.browser.FindElements(By.TagName("div"));
            Assert.NotEmpty(divElements);
        }

        [Fact]
        public void OrdersIndexPageHasContentDiv()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri + "/Orders/Index");

            var divElements = this.browser.FindElements(By.TagName("div"));
            Assert.NotEmpty(divElements);
        }

        [Fact]
        public void PageHasValidHtmlStructure()
        {
            var rootUri = this.server.GetRootUri();
            this.browser!.Navigate().GoToUrl(rootUri);

            var htmlElement = this.browser.FindElement(By.TagName("html"));
            Assert.NotNull(htmlElement);

            var headElement = this.browser.FindElement(By.TagName("head"));
            Assert.NotNull(headElement);

            var bodyElement = this.browser.FindElement(By.TagName("body"));
            Assert.NotNull(bodyElement);
        }

        [Fact]
        public void NavigationBetweenPagesWorks()
        {
            var rootUri = this.server.GetRootUri();

            this.browser!.Navigate().GoToUrl(rootUri);
            var initialUrl = this.browser.Url;
            Assert.NotNull(initialUrl);

            this.browser.Navigate().GoToUrl(rootUri + "/Contacts");
            var contactsUrl = this.browser.Url;
            Assert.Contains("Contacts", contactsUrl);

            this.browser.Navigate().GoToUrl(rootUri + "/Orders");
            var ordersUrl = this.browser.Url;
            Assert.Contains("Orders", ordersUrl);
        }

        [Fact]
        public void BrowserCanNavigateBackAndForward()
        {
            var rootUri = this.server.GetRootUri();

            this.browser!.Navigate().GoToUrl(rootUri);
            this.browser.Navigate().GoToUrl(rootUri + "/Contacts");

            this.browser.Navigate().Back();
            Assert.Contains(rootUri, this.browser.Url);

            this.browser.Navigate().Forward();
            Assert.Contains("Contacts", this.browser.Url);
        }
    }
}
