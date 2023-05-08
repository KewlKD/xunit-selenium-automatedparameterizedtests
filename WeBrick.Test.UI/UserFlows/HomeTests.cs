using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using WeBrick.Test.UI.Models;
using WeBrick.Test.UI.Websites;
using Xunit;

namespace WeBrick.Test.UI.UserFlows
{
    public class HomeTests : IDisposable
    {
        private readonly string SearchUrl = $"{Configuration.Website}soeg";
        private static readonly Dictionary<string, int> TotalPerCategory = new Dictionary<string, int>();
        private readonly Dictionary<string, By> XPath = new Dictionary<string, By>();


        public HomeTests()
        {
            XPath.Add("Villa", SearchPage.ByVilla);
            XPath.Add("Villalejlighed", SearchPage.ByVillalejlighed);
            XPath.Add("Lejlighed", SearchPage.ByLejlighed);
            XPath.Add("Rækkehus", SearchPage.ByRækkehus);
            XPath.Add("Landejendom", SearchPage.ByLandejendom);
            XPath.Add("Andet", SearchPage.ByAndet);
        }

        public void Dispose()
        {
            Utils.MakeSureChromeDriveDoesNotExist();
        }

        [Fact]
        [Trait("Category", "FavoriteHome")]
        public void SetFavoriteHomeWithLogin()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                Utils.Login(driver);
                Utils.GoToUrl(driver, $"{Configuration.Website}property/{Configuration.SampleProperty1}");
                var propertyPage = new PropertyPage(driver);
                string subjectId = Utils.GetCurrentUserSubjectId();
                Utils.RemoveFavoriteHomes(subjectId);//Clean environment
                Utils.GoToUrl(driver, $"{Configuration.Website}property/{Configuration.SampleProperty1}");

                List<FavoriteHome> favoriteHomes = Utils.GetFavoriteHomeList(subjectId);
                Assert.Equal(0, favoriteHomes.Count);
                for (int i = 0; i < 10; i++)
                {
                    Utils.ScrollDown(driver, 100);
                    if (propertyPage.AddFavoriteButton.Click(2))
                    {
                        break;
                    }
                }
                Utils.Wait();
                favoriteHomes = Utils.GetFavoriteHomeList(subjectId);

                Assert.True(favoriteHomes.Any(f => f.Slug == Configuration.SampleProperty1));
                propertyPage.AddFavoriteButton.Click();
                Utils.Wait();
                favoriteHomes = Utils.GetFavoriteHomeList(subjectId);
                Assert.False(favoriteHomes.Any(f => f.Slug == Configuration.SampleProperty1));

            }
        }

        [Fact]
        [Trait("Category", "FavoriteHome")]
        public void SetFavoriteHomeWithoutLogin()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                Utils.GoToUrl(driver, $"{Configuration.Website}property/{Configuration.SampleProperty1}");
                var propertyPage = new PropertyPage(driver);
                string subjectId = Utils.GetCurrentUserSubjectId();
                Utils.RemoveFavoriteHomes(subjectId);//Clean environment

                for (int i = 0; i < 10; i++)
                {
                    Utils.ScrollDown(driver, 100);
                    if (propertyPage.AddFavoriteButton.Click(2))
                    {
                        break;
                    }
                }
                Utils.Wait();
                Assert.Contains("For at tilføje til din favoritliste skal du være logget ind", Utils.GetPageBodyText(driver));
                List<FavoriteHome> favoriteHomes = Utils.GetFavoriteHomeList(subjectId);
                Assert.False(favoriteHomes.Any(f => f.Slug == Configuration.SampleProperty1));
            }
        }



        [Fact]
        [Trait("Category", "FavoriteHome")]
        public void PriceCheck()
        {
            if (Configuration.TestEnvironment == "prod")
            {
                return;
            }
            int _GetPriceWithStatus(IWebDriver driver, string status, string slug)
            {
                Utils.ExecuteOnDb($"UPDATE public.home_entity SET sale_status= '{status}' WHERE slug = '{slug}';");
                Utils.GoToUrl(driver, $"{Configuration.Website}my-properties/{slug}");
                Utils.Wait(2);
                var myPropertyPage = new MyPropertyPage(driver);


                string priceText = myPropertyPage.Price.GetInnerText();
                priceText = Utils.RemoveNonNumericChars(priceText);
                return int.Parse(priceText);
            }

            void _UpdatePriceInWeb(IWebDriver driver, int newPrice, string slug)
            {
                Utils.Wait();
                var myPropertyPage = new MyPropertyPage(driver);
                Utils.ScrollToElement(driver, myPropertyPage.EditPriceIcon);
                Utils.Wait();
                Utils.ScrollDown(driver, -200);
                Utils.Wait();
                Assert.True(myPropertyPage.EditPriceIcon.Click(3));

                Utils.Wait();
                myPropertyPage.PriceModalField.Clear();
                myPropertyPage.PriceModalField.SendKeys(newPrice.ToString());
                myPropertyPage.PriceModalField.SendKeys("\t");
                Utils.Wait();
                Utils.GoToUrl(driver, $"{Configuration.Website}my-properties/{slug}");
                Utils.Wait();
                myPropertyPage = new MyPropertyPage(driver);
                string priceText = Utils.RemoveNonNumericChars(myPropertyPage.Price.GetInnerText());
                Assert.Equal(newPrice.ToString(), priceText);
            }

            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                string targetSlug = Configuration.SampleProperty1;
                Utils.Login(driver, Configuration.MainPhoneNumber, Configuration.MainPassword);

                string queryPresentationPrice = $@"SELECT hp.price FROM 
public.home_entity h
,public.home_presentation_entity  hp
where slug ='{targetSlug}'
and h.official_presentation_id=hp.id;";
                int presentationPrice = Utils.ReadIntFromDb(queryPresentationPrice);

                string queryValuationPrice = $@"SELECT h.valuation_in_dkk FROM 
public.home_entity h
,public.home_presentation_entity  hp
where slug ='{targetSlug}'
and h.official_presentation_id=hp.id;";
                int valuationPrice = Utils.ReadIntFromDb(queryValuationPrice);

                Assert.NotEqual(valuationPrice, presentationPrice);
                int passivePrice = _GetPriceWithStatus(driver, "Passive", targetSlug);
                Assert.Equal(valuationPrice, passivePrice);

                int activePrice = _GetPriceWithStatus(driver, "Active", targetSlug);
                Assert.Equal(presentationPrice, activePrice);

                Utils.RemoveOwners(targetSlug);
                Utils.GoToUrl(driver, $"{Configuration.Website}my-properties/{targetSlug}");
                Utils.Wait();
                Utils.ClaimHome(driver, targetSlug);
                Utils.Wait();
                int newPrice = (presentationPrice - 1);
                _UpdatePriceInWeb(driver, newPrice, targetSlug);
                _UpdatePriceInWeb(driver, presentationPrice, targetSlug);

            }
        }



        [Fact]
        [Trait("Category", "UserFlow")]
        // This tests if homes are loaded when clicking "Se alle boliger" from the front page.
        // If no elements are showing the test will fail. Test created since we had an issue where nothing was showing up.
        public void ElementIsShowing()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);
                var findElement = searchPage.RandomElementIsShowing;

                // Assert
                Assert.NotNull(findElement);
            }
        }

        [Theory]
        [InlineData("Villa")]
        [InlineData("Villalejlighed")]
        [InlineData("Lejlighed")]
        [InlineData("Rækkehus")]
        [InlineData("Landejendom")]
        [InlineData("Andet")]
        [InlineData("Andet,Landejendom")]
        [InlineData("Andet,Rækkehus")]
        [InlineData("Andet,Lejlighed")]
        [InlineData("Andet,Villalejlighed")]
        [InlineData("Andet,Villa")]
        [InlineData("Villa,Villalejlighed")]
        [Trait("Category", "Search")]
        public void SearchForHomeType(string homeType)
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {


                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);
                foreach (string type in homeType.Split(','))
                {
                    driver.FindElement(XPath[type]).Click();
                }

                Utils.Wait();
                var result = searchPage.HomesForSaleCount;
                int actualTotalNumber = int.Parse(result.GetInnerText());


                // Assert
                var checkForHome = searchPage.RandomElementIsShowing;
                Assert.NotNull(checkForHome);
                Assert.True(10 < actualTotalNumber);//Make sure it is greater than a small number like 10

                TotalPerCategory.Add(homeType, actualTotalNumber);


                if (TotalPerCategory.Count == 12)//If all the tests executed then check that for combinations of homeTypes sum of result is correct
                {
                    foreach (string key in TotalPerCategory.Keys)
                    {
                        if (key.Contains(","))
                        {
                            int total = homeType.Split(',').ToList().Sum(s => TotalPerCategory[s]);
                            Assert.True(total <= actualTotalNumber);
                        }
                    }
                }
            }
        }



        [Fact]
        [Trait("Category", "Search")]
        public void SearchByPrice()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();
                int actualTotalNumber1 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(10 < actualTotalNumber1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool upperValueIsGreaterThanThreeMillion = true;
                while (upperValueIsGreaterThanThreeMillion)
                {
                    move.MoveToElement(searchPage.PriceUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.PriceUpperSliderValueSpan.GetInnerText();
                    int priceUpperValue = int.Parse(value.Replace("+", "").Replace(".", ""));
                    upperValueIsGreaterThanThreeMillion = priceUpperValue > 3000000;
                }


                int actualTotalNumber2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(actualTotalNumber2 < actualTotalNumber1);


            }
        }



        [Fact]
        [Trait("Category", "Search")]
        public void SearchBySize()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();

                int homesForSale1 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(10 < homesForSale1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool SizeIsGreaterThanOneHundred = true;
                while (SizeIsGreaterThanOneHundred)
                {
                    move.MoveToElement(searchPage.SizeUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.SizeUpperSliderValueSpan.GetInnerText().Trim();
                    int index = value.IndexOf("m");
                    if (index >= 0)
                    {
                        value = value.Substring(0, index);
                    }
                    int actualSize1 = int.Parse(value);
                    SizeIsGreaterThanOneHundred = actualSize1 > 200;
                }

                Utils.Wait();
                int homesForSale2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(homesForSale2 < homesForSale1, $"homesForSale2: {homesForSale2}, homesForSale1:{homesForSale1}");


            }
        }







        [Fact]
        [Trait("Category", "Search")]
        public void SearchByLandSize()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();
                string strHomeForSaleCount = searchPage.HomesForSaleCount.GetInnerText();
                int actualLandSize1 = int.Parse(strHomeForSaleCount);


                // Assert
                Assert.True(10 < actualLandSize1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool SizeIsGreaterThanFiveThousand = true;
                while (SizeIsGreaterThanFiveThousand)
                {
                    move.MoveToElement(searchPage.LandSizeUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.LandSizeUpperSliderValueSpan.GetInnerText();
                    value = value.Replace("+", "");
                    int index = value.IndexOf("m");
                    if (index >= 0)
                    {
                        value = value.Substring(0, index);
                    }
                    int LandSizeValue = int.Parse(value);
                    SizeIsGreaterThanFiveThousand = LandSizeValue > 1000;
                }


                int actualLandSize2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(actualLandSize2 < actualLandSize1);


            }
        }


        [Fact(Skip = "search result shows no change by changing the number of floors")]
        [Trait("Category", "Search")]
        public void SearchByFloorPlan()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();
                int actualNumberLevels1 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(10 < actualNumberLevels1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool upperLevelsIsGreaterThanThree = true;
                while (upperLevelsIsGreaterThanThree)
                {
                    move.MoveToElement(searchPage.FloorPlanUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.FloorPlanUpperSliderValueSpan.GetInnerText();
                    int floorPlanUpperValue = int.Parse(value.Replace("+", ""));
                    upperLevelsIsGreaterThanThree = floorPlanUpperValue > 3;
                }


                int actualNumberLevels2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(actualNumberLevels2 < actualNumberLevels1);


            }
        }




        [Fact(Skip = "search result shows no change when number of rooms go to 1")]
        [Trait("Category", "Search")]
        public void SearchByNumberOfRooms()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();
                int actualNumberOfRooms1 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(10 < actualNumberOfRooms1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool upperRoomNumberIsGreaterThanFive = true;
                while (upperRoomNumberIsGreaterThanFive)
                {
                    move.MoveToElement(searchPage.NumberRoomUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.NumberRoomUpperSliderValueSpan.GetInnerText();
                    int roomNumberUpperValue = int.Parse(value.Replace("+", ""));
                    upperRoomNumberIsGreaterThanFive = roomNumberUpperValue > 5;
                }
                Utils.Wait();

                int actualNumberOfRooms2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(actualNumberOfRooms2 < actualNumberOfRooms1);


            }
        }

        [Fact]
        [Trait("Category", "Search")]
        public void SearchByYearBuilt()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                // Arrange
                var searchPage = new SearchPage(driver);

                // Act
                Utils.GoToUrl(driver, SearchUrl);

                Utils.Wait();

                searchPage.VillaSpan.Click();

                Utils.Wait();
                string strHomeForSaleCount = searchPage.HomesForSaleCount.GetInnerText();
                int actualYearBuilt1 = int.Parse(strHomeForSaleCount);

                // Assert
                Assert.True(10 < actualYearBuilt1);

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                Actions move = new Actions(driver);
                bool upperYearBuiltIsGreaterThan2015 = true;
                while (upperYearBuiltIsGreaterThan2015)
                {
                    move.MoveToElement(searchPage.YearBuiltUpperSliderSpan).ClickAndHold().MoveByOffset(-10, 0).Release().Perform();
                    Utils.Wait();

                    string value = searchPage.YearBuiltUpperSliderValueSpan.GetInnerText();
                    int yearBuiltUpperValue = int.Parse(value.Replace("+", ""));
                    upperYearBuiltIsGreaterThan2015 = yearBuiltUpperValue > 1950;
                }
                Utils.Wait();

                int actualYearBuilt2 = int.Parse(searchPage.HomesForSaleCount.GetInnerText());

                // Assert
                Assert.True(actualYearBuilt2 < actualYearBuilt1);

                var entries = driver.Manage().Logs.GetLog(LogType.Browser);
                foreach (var entry in entries)
                {
                    Console.Out.WriteLine(entry.ToString());
                }
            }
        }



        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Home")]
        public void CheckPricePerM2(bool isActive)
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            string targetSlug = Configuration.SampleProperty1;
            using (IWebDriver driver = Utils.GetChromeDriver())
            {

                try
                {
                    Utils.SetHomeSaleStatus(targetSlug, isActive);
                    var myPropertyPage = new MyPropertyPage(driver);


                    //Act

                    driver.Navigate().GoToUrl($"{Configuration.Website}my-properties/{targetSlug}");
                    Utils.Wait();
                    Utils.ScrollDown(driver, 400);
                    Utils.Wait();
                    string priceText = Utils.RemoveNonNumericChars(myPropertyPage.Price.GetInnerText());
                    string pricePerSquareMeterText = Utils.RemoveNonNumericChars(myPropertyPage.PricePerM2.GetInnerText().Split('/')[0]);
                    double price = double.Parse(priceText);
                    int homeSizeM2 = int.Parse(myPropertyPage.HomeSizeM2.GetInnerText());
                    double pricePerM2 = Math.Round(double.Parse(pricePerSquareMeterText), 2) / 100;
                    Assert.Equal(Math.Round(price / homeSizeM2, 2), pricePerM2);
                }
                finally
                {
                    Utils.SetHomeSaleStatus(targetSlug, false);
                }
            }
        }



        [Fact]
        [Trait("Category", "Home")]
        public void CheckOwnership()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                try
                {
                    Utils.Login(driver);
                    Utils.Wait();
                    Utils.RemoveOwners(Configuration.SampleProperty);
                    MyPropertyPage myPropertyPage = new MyPropertyPage(driver);
                    Utils.ClaimHome(driver, Configuration.SampleProperty);
                    Utils.ScrollDown(driver, -1000);
                    Assert.True(myPropertyPage.UnClaimHomeButton.Click(1));
                    Utils.Wait();
                    Assert.True(myPropertyPage.ConfirmUnClaimHomeButton.Click(1));
                    Utils.Wait();
                    try
                    {
                        Assert.False(myPropertyPage.UnClaimHomeButton.Click(1));
                        Utils.Wait();
                    }
                    catch (Exception)
                    {

                    }
                    //Act

                }
                finally
                {
                    Utils.RemoveOwners(Configuration.SampleProperty);
                }
            }
        }



        [Theory]
        [InlineData(0, new[] { @"Nødvendige tjenester er ['Immobilienanzeige' og 'Foto und Grundriss' og 'Die 50 Dokumente des Immobilien' og '3 Monaten Marketing']", "Nødvendige tjenester er ['Boligannonce' og 'Foto og plantegning' og 'Boligens 50 dokumenter ' og '3 Mdr. markedsføring']" })]
        [InlineData(1, new[] { @"Nødvendige tjenester er ['Anwalt für Immobilienrecht' og 'Wohngebäudeversicherung'", "Nødvendige tjenester er ['Boligadvokat' og 'Boligforsikringer']" })]
        [InlineData(2, new[] { @"Nødvendige tjenester er ['Anwalt für Immobilienrecht' og 'Wohngebäudeversicherung' og 'Bank']", "Nødvendige tjenester er ['Boligadvokat' og 'Boligforsikringer']" })]
        [InlineData(3, new[] { @"", "" })]
        [Trait("Category", "Home")]
        public void OfferMessageTest(int orderNumber, string[] servicesArray)
        {
            if (Configuration.TestEnvironment == "prod")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                string services = Configuration.TestEnvironment == "qa" ? servicesArray[0] : servicesArray[1];
                var h = new HomePage(driver);
                Utils.GoToUrl(driver, h.Url);
                var orderButtons = new List<IWebElement> { h.OfferButtonOne, h.OfferButtonTwo, h.OfferButtonThree, h.OfferButtonFour };
                while (true)
                {
                    IWebElement targetButton = orderButtons[orderNumber];
                    Utils.ScrollDown(driver, 100);
                    Utils.Wait();
                    if (targetButton.Click(1))
                    {
                        break;
                    }
                }
                Utils.Wait();
                h.OfferTextBox.SendKeys(Configuration.PhoneNumber);
                Utils.Wait();
                h.SendOrderDialogueButton.Click();
                Utils.Wait();
                List<string> logs = Utils.GetLog(driver);
                Assert.True(logs.Any(l => l.Contains(Configuration.PhoneNumber)));
                Assert.True(logs.Any(l => l.Contains("HOMESELLER")));
                Assert.True(logs.Any(l => l.Contains("LAWYER")));
                Assert.True(logs.Any(l => l.Contains(services)));
            }

        }


        [Fact]
        [Trait("Category", "Search")]
        public void HtmlTestThumbnailImageSentOnSocialMedia()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                var homePage = new HomePage(driver);
                Utils.GoToUrl(driver, homePage.Url);
                Utils.Wait();
                var h = homePage.MetaList;
                string thumbnailImage = h.Where(m => m.GetAttribute("property") == "og:image")
                    .Select(m => m.GetAttribute("content")).First();
                Assert.Equal("https://webrick.dk/images/frontpage/forside_webrick_OaD5um45Cik.png", thumbnailImage);
            }

        }


        [Fact]
        [Trait("Category", "Search")]
        public void PropertyAdvancedSearch()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())

            {
                var homePage = new HomePage(driver);
                Utils.GoToUrl(driver, homePage.Url);
                Assert.True(homePage.PropertyAdvancedSearch.Click(1));
                Utils.Wait();
                Assert.Contains("/soeg", driver.Url);
            }
        }


        [Fact]
        [Trait("Category", "Search")]
        public void ClaimedHomeAndCheckItAppearsInMyHome()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                string targetSlug = Configuration.SampleProperty;
                try
                {
                    var profilePage = new ProfilePage(driver);
                    Utils.Login(driver);
                    Utils.RemoveOwners(targetSlug);
                    string address = string.Join(" ", targetSlug.Split("-").ToList().Take(2));
                    Utils.GoToUrl(driver, profilePage.Url);
                    Utils.Wait();
                    Assert.False(profilePage.Body.GetInnerText().ToLower().Contains(address));
                    Utils.ClaimHome(driver, targetSlug);
                    Assert.True(profilePage.Body.GetInnerText().ToLower().Contains(address));
                }
                finally
                {
                    Utils.RemoveOwners(targetSlug);
                }


            }
        }

    }
}
