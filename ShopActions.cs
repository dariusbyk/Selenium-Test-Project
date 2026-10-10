using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumHomework
{
    // Общие действия, чтобы не копировать заполнение форм в каждый тест.
    public class ShopActions
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public ShopActions(IWebDriver browser)
        {
            driver = browser;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestSettings.WaitSeconds));
        }

        public void OpenPage(string route)
        {
            driver.Navigate().GoToUrl(TestSettings.BaseUrl + "index.php?route=" + route);
        }

        public IWebElement WaitForElement(By locator)
        {
            return wait.Until(delegate(IWebDriver browser)
            {
                IWebElement element = browser.FindElement(locator);
                if (element.Displayed && element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        public void EnterText(By locator, string text)
        {
            IWebElement element = WaitForElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        public void Register(string email, string password)
        {
            OpenPage("account/register");
            EnterText(By.Id("input-firstname"), "Student");
            EnterText(By.Id("input-lastname"), "Test");
            EnterText(By.Id("input-email"), email);
            EnterText(By.Id("input-telephone"), "70000000000");
            EnterText(By.Id("input-password"), password);
            EnterText(By.Id("input-confirm"), password);
            WaitForElement(By.Name("agree")).Click();
            WaitForElement(By.CssSelector("#content input[type='submit']")).Click();
        }

        public void Login(string email, string password)
        {
            OpenPage("account/login");
            EnterText(By.Id("input-email"), email);
            EnterText(By.Id("input-password"), password);
            WaitForElement(By.CssSelector("#content input[type='submit']")).Click();
        }

        public void WaitForRoute(string route)
        {
            wait.Until(delegate(IWebDriver browser)
            {
                return browser.Url.Contains("route=" + route);
            });
        }

        public string CreateUser()
        {
            string email = NewEmail();
            Register(email, TestSettings.Password);
            WaitForRoute("account/success");
            OpenPage("account/logout");
            WaitForRoute("account/logout");
            return email;
        }

        public string NewEmail()
        {
            return "student." + Guid.NewGuid().ToString("N") + "@example.com";
        }

        public void Search(string query)
        {
            OpenPage("common/home");
            EnterText(By.CssSelector("#search input[name='search']"), query);
            WaitForElement(By.CssSelector("#search button")).Click();
            WaitForRoute("product/search");
            WaitForElement(By.Id("input-search"));
        }

        public string GetError()
        {
            return WaitForElement(By.CssSelector(".alert-danger")).Text;
        }
    }
}
