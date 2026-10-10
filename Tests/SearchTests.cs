using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumHomework.Tests
{
    [TestFixture]
    public class SearchTests : BaseTest
    {
        [Test]
        [Description("Поиск MacBook: найден MacBook, названия всех результатов содержат запрос.")]
        public void SearchExistingProduct()
        {
            shop.Search(TestSettings.ProductName);
            shop.WaitForElement(By.CssSelector("#content .product-thumb h4 a"));
            ReadOnlyCollection<IWebElement> products = driver.FindElements(
                By.CssSelector("#content .product-thumb h4 a"));

            Assert.That(products.Count, Is.GreaterThan(0), "Поиск должен вернуть товары.");
            bool expectedProductFound = false;
            foreach (IWebElement product in products)
            {
                string name = product.Text;
                Assert.That(name.ToLowerInvariant(), Does.Contain(TestSettings.ProductName.ToLowerInvariant()),
                    "В выдаче найден неподходящий товар: " + name);
                if (name == TestSettings.ProductName)
                {
                    expectedProductFound = true;
                }
            }
            Assert.That(expectedProductFound, Is.True, "В выдаче должен присутствовать именно MacBook.");
        }

        [Test]
        [Description("Несуществующий запрос: нет карточек товаров, показано сообщение об отсутствии результатов.")]
        public void SearchMissingProduct()
        {
            string query = "notfound" + Guid.NewGuid().ToString("N");
            shop.Search(query);
            string content = shop.WaitForElement(By.Id("content")).Text;

            Assert.That(content, Does.Contain("Нет товаров, которые соответствуют критериям поиска."));
            Assert.That(driver.FindElements(By.CssSelector("#content .product-thumb")).Count,
                Is.EqualTo(0), "При пустой выдаче карточек товаров быть не должно.");
        }
    }
}
