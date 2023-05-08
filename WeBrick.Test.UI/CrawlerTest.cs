using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using WeBrick.Test.UI.Websites;
using Xunit;

namespace WeBrick.Test.UI
{

    [Collection("Sequential")]
    public class CrawlerTest
    {
        [Theory]
        [InlineData(1000)]
        [Trait("Category", "Crawl")]
        public void CrawlWebrick(int maxUrlsToCheck)
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                Utils.GoToUrl(driver, Configuration.Website);
                List<string> internalLinks = Utils.GetInternalLinks(driver, Configuration.WebsiteName, true).Distinct().ToList();
                var sourceLinkChildLinksDict = new Dictionary<string, List<string>>();
                sourceLinkChildLinksDict.Add(Configuration.Website, internalLinks);
                var linkDictionary = new Dictionary<string, List<IWebElement>>();
                var visitedLinks = new List<string>();
                var failedLinks = new Dictionary<string, string>();
                while (internalLinks.Count > 0)
                {
                    string hrefUnderCheck = internalLinks[0];
                    if (hrefUnderCheck.StartsWith("https://facebook.com"))
                    {
                        internalLinks.RemoveAt(0);
                        continue;
                    }
                    if (!visitedLinks.Contains(hrefUnderCheck))
                    {
                        string bodyText = string.Empty;
                        try
                        {

                            visitedLinks.Add(hrefUnderCheck);
                            Utils.GoToUrl(driver, hrefUnderCheck);
                            Assert.True(driver.Url.ToLower().StartsWith("https://"));
                            if (hrefUnderCheck.Contains("soeg"))
                            {
                                for (int i = 1; i < 5; i++)
                                {
                                    Utils.ScrollDown(driver, 1000);
                                    Utils.Wait(0.3);
                                }
                            }
                            List<string> newLinks = Utils.GetInternalLinks(driver, Configuration.WebsiteName, true);
                            newLinks.RemoveAll(link => visitedLinks.Contains(link) || internalLinks.Contains(link));
                            if (newLinks.Count > 0)
                            {
                                if (sourceLinkChildLinksDict.ContainsKey(hrefUnderCheck))
                                {
                                    sourceLinkChildLinksDict[hrefUnderCheck].AddRange(newLinks);
                                }
                                else
                                {
                                    sourceLinkChildLinksDict.Add(hrefUnderCheck, newLinks);
                                }
                            }
                            internalLinks.AddRange(newLinks);
                            if (visitedLinks.Count > maxUrlsToCheck)//
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        bodyText = Utils.GetPageBodyText(driver).ToLower();
                        string bodyHtml = Utils.GetPageBodyInnerHtml(driver).ToLower();
                        string parentLinks = string.Join(",", sourceLinkChildLinksDict.Keys.Where(k => sourceLinkChildLinksDict[k].Contains(hrefUnderCheck)));

                        //string tempParentLink= string.Join(",", sourceLinkChildLinksDict.Keys.Where(k => sourceLinkChildLinksDict[k].Contains("https://webrick.dk/sider/markedsforing"))); 
                        string bodyHtmlWithRemoveAuthorizedGoogleTags = Utils.RemoveAuthorizedGoogleUsages(bodyHtml);

                        Assert.DoesNotContain("google", bodyHtmlWithRemoveAuthorizedGoogleTags);
                        Assert.DoesNotContain("drive.google.com", bodyHtml);
                        Assert.DoesNotContain("Der er sket en fejl".ToLower(), bodyText + "__" + hrefUnderCheck);

                    }
                    internalLinks.RemoveAt(0);
                }
                Assert.True(visitedLinks.Count > 200);
            }
        }

        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckSearchUrlInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                homepage.SearchHomeLink.Click();
                Utils.Wait();
                Assert.Equal("https://webrick.dk/soeg?homeTypes=&lowerPrice=&upperPrice=&lowerHomeSize=&upperHomeSize=&lowerLotSize=&upperLotSize=&lowerNumberOfFloors=&upperNumberOfFloors=&lowerNumberOfRooms=&upperNumberOfRooms=&lowerYearBuilt=&upperYearBuilt=&address=&saleStatus=ACTIVE", driver.Url);
            }
        }



        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckSelfSaleUrlInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                homepage.SelfSaleHomeLink.Click();
                Utils.Wait();
                Assert.Equal("https://webrick.dk/my-properties/find", driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckSearchButtonInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                homepage.SearchHomeButton.Click();
                Utils.Wait();
                string searchUrl = driver.Url;
                Assert.Equal("https://webrick.dk/soeg?homeTypes=&lowerPrice=&upperPrice=&lowerHomeSize=&upperHomeSize=&lowerLotSize=&upperLotSize=&lowerNumberOfFloors=&upperNumberOfFloors=&lowerNumberOfRooms=&upperNumberOfRooms=&lowerYearBuilt=&upperYearBuilt=&address=&saleStatus=ACTIVE", searchUrl);
            }
        }

        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckLoanUrlInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                homepage.LoanHomeLink.Click();
                Utils.Wait();
                string currentUrl = driver.Url;
                Assert.Equal("https://webrick.dk/laanebevis", currentUrl);
            }
        }


        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckPropertySellInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                Utils.ScrollToElement(driver, homepage.PropertySellLink);
                Utils.ScrollDown(driver, 300);
                homepage.PropertySellLink.Click();
                Utils.Wait();
                string currentUrl = driver.Url;
                Assert.Equal("https://cms.webrick.dk/saelg-din-bolig/", currentUrl);
            }
        }
        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckByPropertySellInProduction()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                Utils.ScrollToElement(driver, homepage.BuyPropertyLink);
                Utils.ScrollDown(driver, 300);
                homepage.BuyPropertyLink.Click();
                Utils.Wait();
                string currentUrl = driver.Url;
                Assert.Equal("https://cms.webrick.dk/koeb-din-bolig-paa-webrick/", currentUrl);
            }
        }

        [Fact]
        [Trait("Category", "CheckUrl")]
        public void CheckByQuestionsAnswersLink()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                HomePage homepage = new HomePage(driver);
                Utils.GoToUrl(driver, homepage.Url);
                Utils.ScrollToElement(driver, homepage.QuestionsAnswersLink);
                Utils.ScrollDown(driver, 300);
                homepage.QuestionsAnswersLink.Click();
                Utils.Wait();
                string currentUrl = driver.Url;
                Assert.Equal("https://cms.webrick.dk/spoergsmaal-svar/", currentUrl);
            }
        }

        [Fact]
        [Trait("Category", "Crawl")]
        public void CrawlBackOffice()
        {
            return;//enable it just when it is needed;
            string visitedLinksFile = @"./visitedBackofficeLinks.txt";
            string errorFile = @"./errorBackofficeLinks.txt";
            string allLinksFile = @"./allLinks.txt";

            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                Utils.Login(driver);

                Utils.GoToUrl(driver, Configuration.BackOffice);

                var linkDictionary = new Dictionary<string, List<IWebElement>>();
                Utils.CreateFileIfNotExists(visitedLinksFile);
                Utils.CreateFileIfNotExists(errorFile);
                Utils.CreateFileIfNotExists(allLinksFile);
                List<string> visitedLinks = File.ReadAllText(visitedLinksFile).Split('\n').ToList();
                List<string> allLinks = File.ReadAllText(allLinksFile).Split('\n').ToList();
                allLinks.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                var sourceLinkChildLinksDict = new Dictionary<string, List<string>>();
                sourceLinkChildLinksDict.Add(Configuration.BackOffice, allLinks);

                if (allLinks.Count == 0)
                {
                    allLinks = Utils.GetInternalLinks(driver, Configuration.BackOfficeName, true);
                }
                var failedLinks = new Dictionary<string, string>();
                while (allLinks.Count > 0)
                {
                    string hrefUnderCheck = allLinks[0];

                    if (hrefUnderCheck.ToLower().Contains("signout") || hrefUnderCheck.ToLower().Contains("storage.googleapis.com")
                        || hrefUnderCheck.ToLower().Contains("facebook.com"))
                    {
                        allLinks.RemoveAt(0);
                        continue;
                    }
                    if (!visitedLinks.Contains(hrefUnderCheck))
                    {
                        string bodyText = string.Empty;
                        string parentLinks = string.Empty;
                        try
                        {
                            visitedLinks.Add(hrefUnderCheck);
                            File.AppendAllText(visitedLinksFile, hrefUnderCheck + "\n");
                            parentLinks = string.Join(",", sourceLinkChildLinksDict.Keys.Where(k => sourceLinkChildLinksDict[k].Contains(hrefUnderCheck)));

                            Utils.GoToUrl(driver, hrefUnderCheck);
                            //Utils.Wait();
                            Assert.True(driver.Url.ToLower().StartsWith("https://"));
                            List<string> newLinks = Utils.GetInternalLinks(driver, Configuration.BackOfficeName, true);
                            if (newLinks.Count > 0)
                            {
                                if (sourceLinkChildLinksDict.ContainsKey(hrefUnderCheck))
                                {
                                    sourceLinkChildLinksDict[hrefUnderCheck].AddRange(newLinks);
                                }
                                else
                                {
                                    sourceLinkChildLinksDict.Add(hrefUnderCheck, newLinks);
                                }
                            }
                            allLinks.AddRange(newLinks);
                            File.WriteAllText(allLinksFile, string.Join("\n", allLinks));
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(errorFile,
                                hrefUnderCheck + "\n" + ex.Message + "\n" + parentLinks + "\n\n\n");
                        }
                        bodyText = Utils.GetPageBodyText(driver).ToLower();
                        string bodyHtml = Utils.GetPageBodyInnerHtml(driver).ToLower();

                        bodyHtml = Utils.RemoveAuthorizedGoogleUsages(bodyHtml);

                        Assert.DoesNotContain("google", bodyHtml);

                        Assert.DoesNotContain("drive.google.com", bodyHtml);
                        File.WriteAllText(@".\1backoffice.txt", visitedLinks.Count + "\n");
                        var deletes = allLinks.Where(l => l.Contains("delete")).ToList();
                        Assert.DoesNotContain("error", bodyText);
                    }
                    allLinks.RemoveAt(0);
                }
                Assert.True(visitedLinks.Count > 80);
            }
        }
    }
}
