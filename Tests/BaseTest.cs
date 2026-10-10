using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumHomework.Tests
{
    [NonParallelizable]
    public class BaseTest
    {
        protected IWebDriver driver;
        protected ShopActions shop;

        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1280,900");

            // По умолчанию Chrome запускается с видимым окном.
            if (TestContext.Parameters.Get("Headless", "false") == "true")
            {
                options.AddArgument("--headless");
            }

            driver = new ChromeDriver(options);
            shop = new ShopActions(driver);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (driver != null && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    string folder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                    Directory.CreateDirectory(folder);
                    string fileName = TestContext.CurrentContext.Test.MethodName + "_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".png";
                    string path = Path.Combine(folder, fileName);
                    try
                    {
                        ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
                        screenshotDriver.GetScreenshot().SaveAsFile(path);
                        TestContext.AddTestAttachment(path, "Скриншот при падении");
                        TestContext.Out.WriteLine("Страница: " + driver.Url);
                    }
                    catch (WebDriverException error)
                    {
                        TestContext.Out.WriteLine("Не удалось сделать скриншот: " + error.Message);
                    }
                }
            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver = null;
                }
            }
        }
    }
}
