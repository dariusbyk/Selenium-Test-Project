using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumHomework.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test]
        [Description("Вход с корректными данными заранее созданного аккаунта.")]
        public void LoginWithCorrectPassword()
        {
            shop.Login(
                TestSettings.Email,
                TestSettings.Password
            );

            shop.WaitForRoute("account/account");

            IWebElement logout = shop.WaitForElement(
                By.CssSelector(
                    "#column-right a[href*='route=account/logout']"
                )
            );

            Assert.That(
                driver.Url,
                Does.Contain("route=account/account"),
                "После входа должен открыться личный кабинет."
            );

            Assert.That(
                logout.Displayed,
                Is.True,
                "После входа должна отображаться ссылка выхода."
            );
        }

        [Test]
        [Description("Вход с существующим email и неправильным паролем.")]
        public void LoginWithWrongPassword()
        {
            shop.Login(
                TestSettings.Email,
                TestSettings.Password + "_wrong"
            );

            string error = shop.GetError();

            Assert.That(
                error,
                Does.Contain(
                    "Неправильно заполнены поле E-Mail и/или пароль"
                ),
                "Должно появиться сообщение о неправильных данных."
            );

            Assert.That(
                driver.Url,
                Does.Contain("route=account/login"),
                "При неправильном пароле должна остаться страница входа."
            );

            int logoutLinksCount = driver.FindElements(
                By.CssSelector(
                    "#column-right a[href*='route=account/logout']"
                )
            ).Count;

            Assert.That(
                logoutLinksCount,
                Is.EqualTo(0),
                "При неудачном входе ссылка выхода не должна появляться."
            );
        }
    }
}