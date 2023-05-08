using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;
using System.Collections.Generic;
using WeBrick.Test.UI.Websites;
using System.Text.RegularExpressions;
using WeBrick.Test.UI.Fixtures;
using System.Net.Http;
using IdentityModel.Client;
using WeBrick.Test.UI.Models.Auth;

namespace WeBrick.Test.UI.UserFlows
{
    [Collection("Sequential")]
    public partial class AuthTest : IClassFixture<DriverFixture>
    {
        private const string cookieName = "__is_loggedin";
        DriverFixture Fixture;

        public AuthTest(DriverFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Login")]
        public void LoginFailureFullscreen()
        {
            // Arrange
            var searchPage = new SearchPage(Fixture.Driver);

            // Act
            Utils.GoToUrl(Fixture.Driver, Configuration.LoginUrl);

            Fixture.Driver.FindElement(By.Name("UserEnteredPhoneNumber")).SendKeys(Configuration.PhoneNumber);
            Fixture.Driver.FindElement(By.Name("Password")).SendKeys("WrongDummyPassword");
            Fixture.Driver.FindElement(By.Name("button")).Click();

            IWebElement loginErrorMessage = searchPage.LoginErrorMessage;

            // Assert
            Assert.NotNull(loginErrorMessage);

        }

        [Fact]
        [Trait("Category", "Authrentication")]
        [Trait("Category", "Home")]
        public void LoginFullscreen()
        {
            // Arrange
            var searchPage = new SearchPage(Fixture.Driver);

            // Act
            Utils.Login(Fixture.Driver);

            var wait = new WebDriverWait(Fixture.Driver, TimeSpan.FromSeconds(10));

            var cookie = wait.WaitForCookie(cookieName);

            Assert.Equal("__is_loggedin", cookie.Name);
            Assert.Equal("true", cookie.Value);

        }



        [Fact]
        [Trait("Category", "Authrentication")]
        [Trait("Category", "Home")]
        public void LogoutFullscreen()
        {
            // Arrange
            var searchPage = new SearchPage(Fixture.Driver);

            // Act
            Utils.Login(Fixture.Driver);
            Utils.Logout(Fixture.Driver);

            searchPage = new SearchPage(Fixture.Driver);


            //TODO: Find a better way to check for existence of loginButton
            Utils.Wait();
            var wait = new WebDriverWait(Fixture.Driver, TimeSpan.FromSeconds(10));

            // Assert

            Assert.True(wait.WaitForCookieAbsence(cookieName));

        }

        [Fact]
        [Trait("Category", "Authrentication")]
        [Trait("Category", "Register")]
        public void CheckTitle()
        {
            // Arrange
            var registrationPage = new RegistrationPage(Fixture.Driver);
            Fixture.Driver.Navigate().GoToUrl(registrationPage.Url);
            Assert.Equal(registrationPage.Title.GetInnerText(), "Indtast informationer og fortsæt");

        }

        public static IEnumerable<object[]> WrongValuesForRegistrationForm()
        {
            var wrongPhoneNumbers = new List<string>() { "", " ", "#", "sdsff", };
            var wrongEmailCases = new List<string>() { "", " ", "fsfj.sdsd", "@@" };
            foreach (string phoneNumber in wrongPhoneNumbers)
            {
                foreach (string email in wrongEmailCases)
                {
                    yield return new object[] { "correct", "correct", phoneNumber, email };
                }
            }
            yield return new object[] { "correct", "correct", "2123123", "." };
            yield return new object[] { "correct", "correct", "#", "test@webrick.dk" };
            yield return new object[] { "correct", "correct", " ", "test@webrick.dk" };
            yield return new object[] { "correct", "", "2123123", "test@webrick.dk" };
            yield return new object[] { "correct", " ", "2123123", "test@webrick.dk" };
            yield return new object[] { "", "correct", "2123123", "test@webrick.dk" };
            yield return new object[] { " ", "correct", "2123123", "test@webrick.dk" };
        }

        [Theory]
        [Trait("Category", "Authrentication")]
        [Trait("Category", "Register")]
        [MemberData(nameof(WrongValuesForRegistrationForm))]
        public void NegativeUserRegistrationTest(string name, string surname, string phoneNumber, string email)
        {
            // Arrange
            var registrationPage = new RegistrationPage(Fixture.Driver);
            Fixture.Driver.Navigate().GoToUrl(registrationPage.Url);
            var wait = new WebDriverWait(Fixture.Driver, TimeSpan.FromSeconds(10));
            wait.WaitForDocumentReadyStateComplete();

            // Act
            registrationPage.Name.SendKeys(name);
            registrationPage.Surname.SendKeys(surname);
            registrationPage.Phone.SendKeys(phoneNumber);
            registrationPage.Email.SendKeys(email);
            registrationPage.ContinueBtn.Click();
            Utils.Wait();
            string allText = registrationPage.DivContainsAllFields.GetInnerText(3);
            Assert.Contains("Indtast informationer og fortsæt", allText);//make sure it do not go to next step when it has error
        }

        public static IEnumerable<object[]> PositiveUserRegistrationTestData()
        {
            var correctNames = new List<string>() { "correct", "#" };
            var correctSurnNames = new List<string>() { "correct", "#" };
            var correctPhoneNumbers = new List<string>() { "4540567564" };
            var correctEmailCases = new List<string>() { "kyle@ck.dk", "mehran@hotmail" };
            foreach (string name in correctNames)
            {
                foreach (string surname in correctSurnNames)
                {
                    foreach (string phoneNumber in correctPhoneNumbers)
                    {
                        foreach (string email in correctEmailCases)
                        {
                            yield return new object[] { name, surname, phoneNumber, email };
                        }
                    }
                }
            }
        }


        [Theory]
        [Trait("Category", "Authrentication")]
        [Trait("Category", "Register")]
        [MemberData(nameof(PositiveUserRegistrationTestData))]
        public void PositiveUserRegistrationTest(string name, string surname, string phoneNumber, string email)
        {

            // Arrange
            var registrationPage = new RegistrationPage(Fixture.Driver);
            Fixture.Driver.Navigate().GoToUrl(registrationPage.Url);
            // Act
            registrationPage.Name.SendKeys(name);
            registrationPage.Surname.SendKeys(surname);
            registrationPage.Phone.SendKeys(phoneNumber);
            registrationPage.Email.SendKeys(email);
            registrationPage.ContinueBtn.Click();
            Utils.Wait(0.2);
            string allText = registrationPage.DivContainsAllFields.GetInnerText(3);
            int numberOfErrors = Regex.Matches(allText, "This field is required.").Count;
            Assert.Equal(0, numberOfErrors);
            Fixture.Driver.Navigate().GoToUrl(registrationPage.Url);

        }

        [Fact]
        [Trait("Category", "Authrentication")]
        public void GetDiscoveryDocumentAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var discoResponse = httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = Configuration.AuthUrl,
                    Policy = new DiscoveryPolicy
                    {
                        RequireHttps = false,
                        ValidateIssuerName = false,
                        AuthorityValidationStrategy = new NonValidatingAuthoritiyValidationStrategy(),
                        ValidateEndpoints = false,
                    }
                }).Result;
                Assert.False(discoResponse.IsError);
            }
        }
    }

}
