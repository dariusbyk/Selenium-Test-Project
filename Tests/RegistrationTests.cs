using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumHomework.Tests
{
    [TestFixture]
    public class RegistrationTests : BaseTest
    {
        [Test]
        [Description("Регистрация с новым email: аккаунт создаётся.")]
        public void RegisterNewUser()
        {
            string email = shop.NewEmail();
            shop.Register(email, TestSettings.Password);
            shop.WaitForRoute("account/success");
            string title = shop.WaitForElement(By.CssSelector("#content h1")).Text;
            Assert.That(title, Does.Contain("создан"), "Ожидается подтверждение создания аккаунта.");
        }

        [Test]
        [Description("Повторная регистрация с занятым email: сообщение о существующем аккаунте.")]
        public void RegisterExistingEmail()
        {
            string email = shop.CreateUser();
            shop.Register(email, TestSettings.Password);
            string error = shop.GetError();

            Assert.That(error, Does.Contain("E-Mail уже зарегистрирован"));
            Assert.That(driver.Url, Does.Contain("route=account/register"));
        }
    }
}
