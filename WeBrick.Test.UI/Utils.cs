using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WeBrick.Test.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using Xunit;
using Npgsql;
using WeBrick.Test.UI.Models;
using System.Linq;
using System.IO;
using System.Net;
using WeBrick.Test.UI.Websites;

namespace WeBrick.Test.UI
{
    public static class Utils
    {
        private static string DriverPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        internal const int TimeOutBySecond = 55;



        internal const int WaitTime = 3500;
        public static IWebDriver GetChromeDriver(bool v)
        {
            MakeSureChromeDriveDoesNotExist();
            DownloadChromeDriverIfItDoesotExist();
            var chromeOptions = new ChromeOptions();
            chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
            chromeOptions.AddArguments(new List<string>() { "headless" });
            var chromeDriverService = ChromeDriverService.CreateDefaultService(DriverPath);

            ChromeDriver driver = null;
            if (Configuration.RunHeadless)
            {
                driver = new ChromeDriver(chromeDriverService, chromeOptions);
            }
            else
            {
                driver = new ChromeDriver(); //for No HeadLess
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TimeOutBySecond);

            return driver;
        }

        private static void DownloadChromeDriverIfItDoesotExist()
        {
            string chromeDriverExe = Path.Combine(DriverPath, "chromedriver.exe");
            string chromeDrivernoExtension = DriverPath + "\\chromedriver";
            if (!File.Exists(chromeDriverExe))
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Configuration.ChromeDriverUrl, chromeDriverExe);
                    System.IO.Compression.ZipFile.ExtractToDirectory(chromeDriverExe, DriverPath);
                }
            }

            if (!File.Exists(chromeDrivernoExtension))
            {
                File.Copy(chromeDriverExe, chromeDrivernoExtension);
            }

            if (!File.Exists(chromeDriverExe))
            {
                throw new Exception("Driver is not downloaded/unzipped correctly");
            }
        }

        internal static float CompareImages(string firstImage, string secondImage)
        {
            var img1 = new Bitmap(firstImage);
            var img2 = new Bitmap(secondImage);
            float diffCount = 0;
            float equallCount = 0;
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        string img1_ref = img1.GetPixel(i, j).ToString();
                        string img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            diffCount++;
                        }
                        else
                        {
                            equallCount++;
                        }
                    }
                }
            }
            return equallCount / (diffCount + equallCount);
        }

        internal static bool HomeAppearsInSearch(IWebDriver driver, string address)
        {
            var searchPage = new SearchPage(driver);
            GoToUrl(driver, searchPage.Url);
            Wait();
            searchPage.SearchInputField.SendKeys(address);
            Wait();
            searchPage.SearchButton.Click();
            Wait();
            string bodyText = searchPage.Body.GetInnerText().ToLower();
            string correctedAddress = address.ToLower().Trim().Replace(",", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            return bodyText.Contains(correctedAddress);

        }


        internal static void GoToUrl(IWebDriver driver, string url)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            driver.Navigate().GoToUrl(url);
            wait.WaitForDocumentReadyStateComplete();
        }

        public static void ScrollToElement(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Utils.Wait(0.2);
        }

        public static void ScrollDown(IWebDriver driver, int height)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollBy(0,{height})");
            Utils.Wait(0.2);

        }

        internal static string GetPageBodyInnerHtml(IWebDriver driver)
        {
            return driver.FindElement(By.TagName("body")).GetInnerHtml();
        }

        public static void Wait()
        {
            Thread.Sleep(WaitTime);
        }

        internal static void Wait(double timeFraction)
        {
            Thread.Sleep((int)(WaitTime * timeFraction));
        }
        public static string GetPageBodyText(IWebDriver driver)
        {
            return driver.FindElement(By.TagName("body")).GetInnerText();
        }

        public static IEnumerable<object[]> GetKommuneList()
        {
            yield return new object[] { "Copenhagen" };
            yield return new object[] { "Aarhus" };
            yield return new object[] { "Aalborg" };
            yield return new object[] { "Odense" };
            yield return new object[] { "Esbjerg" };
            yield return new object[] { "Vejle" };
            yield return new object[] { "Frederiksberg" };
            yield return new object[] { "Randers" };
            yield return new object[] { "Viborg" };
            yield return new object[] { "Kolding" };
            yield return new object[] { "Silkeborg" };
            yield return new object[] { "Herning" };
            yield return new object[] { "Horsens" };
            yield return new object[] { "Roskilde" };
            yield return new object[] { "N�stved" };
            yield return new object[] { "Slagelse" };
            yield return new object[] { "S�nderborg" };
            yield return new object[] { "Gentofte" };
            yield return new object[] { "Holb�k" };
            yield return new object[] { "Hj�rring" };
            yield return new object[] { "Gladsaxe" };
            yield return new object[] { "Guldborgsund" };
            yield return new object[] { "Helsing�r" };
            yield return new object[] { "Frederikshavn" };
            yield return new object[] { "Aabenraa" };
            yield return new object[] { "Svendborg" };
            yield return new object[] { "Skanderborg" };
            yield return new object[] { "Ringk�bing-Skjern" };
            yield return new object[] { "K�ge" };
            yield return new object[] { "Holstebro" };
            yield return new object[] { "Haderslev" };
            yield return new object[] { "Rudersdal" };
            yield return new object[] { "lyngby" };
            yield return new object[] { "Faaborg-Midtfyn" };
            yield return new object[] { "Hvidovre" };
            yield return new object[] { "Varde" };
            yield return new object[] { "Fredericia" };
            yield return new object[] { "Kalundborg" };
            yield return new object[] { "Hiller�d" };
            yield return new object[] { "Taastrup" };
            yield return new object[] { "Ballerup" };
            yield return new object[] { "Greve" };
            yield return new object[] { "Skive" };
            yield return new object[] { "Favrskov" };
            yield return new object[] { "Hedensted" };
            yield return new object[] { "Vordingborg" };
            yield return new object[] { "Lolland" };
            yield return new object[] { "Thisted" };
            yield return new object[] { "Frederikssund" };
            yield return new object[] { "Vejen" };
            yield return new object[] { "Mariagerfjord" };
            yield return new object[] { "Egedal" };
            yield return new object[] { "Syddjurs" };
            yield return new object[] { "Assens" };
            yield return new object[] { "Bornholm" };
            yield return new object[] { "T�rnby" };
            yield return new object[] { "Ikast-Brande" };
            yield return new object[] { "Gribskov" };
            yield return new object[] { "Fredensborg" };
            yield return new object[] { "T�nder" };
            yield return new object[] { "Jammerbugt" };
            yield return new object[] { "Fures�" };
            yield return new object[] { "Norddjurs" };
            yield return new object[] { "Middelfart" };
            yield return new object[] { "Vesthimmerland" };
            yield return new object[] { "R�dovre" };
            yield return new object[] { "Br�nderslev" };
            yield return new object[] { "Faxe" };
            yield return new object[] { "Br�ndby" };
            yield return new object[] { "Ringsted" };
            yield return new object[] { "Odsherred" };
            yield return new object[] { "Nyborg" };
            yield return new object[] { "Halsn�s" };
            yield return new object[] { "Sor�" };
            yield return new object[] { "Nordfyn" };
            yield return new object[] { "Rebild" };
            yield return new object[] { "Albertslund" };
            yield return new object[] { "Lejre" };
            yield return new object[] { "Herlev" };
            yield return new object[] { "Billund" };
            yield return new object[] { "H�rsholm" };
            yield return new object[] { "Aller�d" };
            yield return new object[] { "Kerteminde" };
            yield return new object[] { "Struer" };
            yield return new object[] { "Stevns" };
            yield return new object[] { "Odder" };
            yield return new object[] { "Glostrup" };
            yield return new object[] { "Mors�" };
            yield return new object[] { "Lemvig" };
            yield return new object[] { "Solr�d" };
            yield return new object[] { "Ish�j" };
            yield return new object[] { "Vallensb�k" };
            yield return new object[] { "Drag�r" };
            yield return new object[] { "Langeland" };
            yield return new object[] { "�r�" };
            yield return new object[] { "Sams�" };
            yield return new object[] { "Fan�" };
            yield return new object[] { "L�s�" };
        }

        internal static void CreateFileIfNotExists(string file)
        {
            if (!File.Exists(file))
            {
                using (File.Create(file)) { };
            }
        }

        internal static int DeleteAllSearchAgents()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                throw new Exception($"{nameof(DeleteAllSearchAgents)} is available just in QA");
            }
            using (var conn = new NpgsqlConnection(Configuration.BuyerDirectoryConnectionString))
            {
                conn.Open();
                var command = new NpgsqlCommand("delete from public.agent_entity", conn);
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows;
            }
        }

        internal static int ReadIntFromDb(string query)
        {
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int intNum = dr.GetInt32(0);
                return intNum;
            }
        }

        internal static string RemoveParameterPart(string url)
        {
            List<string> parts = url.Split('/').ToList();
            string lastPart = parts.Last();
            List<string> parameterSign = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (parameterSign.Any(p => lastPart.Contains(p)))
            {
                parts.RemoveAt(parts.Count - 1);
                return string.Join("/", parts);
            }
            return url;
        }

        internal static string RemoveNonNumericChars(string text)
        {
            List<char> chars = text.ToCharArray().ToList();
            for (int i = 0; i < chars.Count; i++)
            {
                if (chars[i] < '0' || chars[i] > '9')
                {
                    chars.RemoveAt(i);
                    i--;
                }
            }
            return new string(chars.ToArray());
        }

        internal static void ExecuteOnDb(string sql)
        {
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                var command = new NpgsqlCommand(sql, conn);
                int effectedRows = command.ExecuteNonQuery();
            }
        }

        internal static string RemoveAuthorizedGoogleUsages(string bodyHtml)
        {
            return bodyHtml.Replace("googleapis", "").Replace("google maps", "").Replace("googletagmanager", "").Replace("google_maps_key", "")
                            .Replace("google_error_reporting_api_key", "").Replace("google_project_id", "").Replace("google_tagmanager_key", "").Replace("google_error_reporting_enabled", "")
                            .Replace("google.com/maps", "").Replace("\"google\"", "").Replace(" google\"", "").Replace("google,", "")
                            .Replace("https://www.google.com/intl/da_us/help", "")
                            .Replace("google tag", "")
                            .Replace("google ", "")
                            .Replace(">google<", "")
                            .Replace(" google.", "")
                            .Replace("support.google.com", "")
                            .Replace("tools.google.com", "")
                            .Replace("google-pay", "")
                            .Replace(" google<", "")
                            .Replace("googlebot", "");
        }

        public static IWebDriver GetChromeDriverNotMaximized()
        {
            MakeSureChromeDriveDoesNotExist();
            DownloadChromeDriverIfItDoesotExist();
            var chromeOptions = new ChromeOptions();
            if (Configuration.RunHeadless)
            {
                chromeOptions.AddArguments(new List<string>() { "headless " });
            }

            chromeOptions.AddArguments(new List<string>() { "--window-size=1800,1080" });
            var chromeDriverService = ChromeDriverService.CreateDefaultService(DriverPath);

            var driver = new ChromeDriver(chromeDriverService, chromeOptions);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TimeOutBySecond);

            return driver;
        }

        public static void MakeSureChromeDriveDoesNotExist()
        {
            try
            {
                Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
                if (chromeDriverProcesses != null)
                {
                    foreach (Process chromeDriver in chromeDriverProcesses)
                    {
                        chromeDriver.Kill();
                    }
                }
                chromeDriverProcesses = Process.GetProcessesByName("chrome");
                if (chromeDriverProcesses != null)
                {
                    foreach (Process chromeDriver in chromeDriverProcesses)
                    {
                        chromeDriver.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void ClaimHome(IWebDriver driver, string slug)
        {
            Utils.GoToUrl(driver, $"{Configuration.Website}my-properties/{slug}");
            Utils.Wait();
            Utils.ScrollDown(driver, 400);
            var myPropertyPage = new MyPropertyPage(driver);
            Assert.True(myPropertyPage.ClaimHomeButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ClaimHomeConfirmationButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ClaimHomeNextStepButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipImagesTitleButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipPriceTitleButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipDescriptionTitleButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipPersonalTitleButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipDataTitleButton.Click(1));
            Utils.Wait();
            Assert.True(myPropertyPage.ToolTipDocumentsTitleCloseButton.Click(1));
            Utils.Wait();
        }


        public static void Login(IWebDriver driver, string phoneNumber, string password)
        {
            var cookieName = "__is_loggedin";


            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            bool succeeded = false;
            while (!succeeded)
            {
                try
                {
                    Utils.GoToUrl(driver, Configuration.Website);//it is needed to resolve HttpBasic auth in PT
                    wait.WaitForDocumentReadyStateComplete();
                    if (Configuration.RunHeadless)
                    {
                        Utils.GoToUrl(driver, Configuration.LoginUrl);
                        wait.WaitForDocumentReadyStateComplete();
                    }
                    else
                    {
                        var menuButtonBy = By.XPath("/html/body/div[1]/div/header/div/div/div[2]/button[2]");
                        var loginButtonBy = By.XPath("/html/body/div[3]/div[3]/ul/li");

                        wait.Until(ExpectedConditions.ElementExists(menuButtonBy)).Click();
                        Utils.Wait();
                        wait.Until(ExpectedConditions.ElementExists(loginButtonBy)).Click();
                    }

                    Utils.Wait(2);

                    driver.FindElement(By.Name("UserEnteredPhoneNumber")).SendKeys(phoneNumber);
                    driver.FindElement(By.Name("Password")).SendKeys(password);
                    driver.FindElement(By.Name("button")).Click();
                    succeeded = true;
                }
                catch (Exception)
                {

                }
            }
            if (Configuration.RunHeadless)
            {
                Utils.Wait();
            }
            else
            {
                var cookie = wait.WaitForCookie(cookieName);
                Assert.True(cookie != null);
                Assert.True(cookie.Expiry < DateTime.Now.AddDays(7));
            }
        }

        public static void Login(IWebDriver driver)
        {
            Login(driver, Configuration.PhoneNumber, Configuration.Password);
        }


        public static List<string> GetLog(IWebDriver driver)
        {
            var entries = driver.Manage().Logs.GetLog(LogType.Browser);

            List<string> result = entries.Select(e => e.ToString()).ToList();
            return result;
        }

        public static string GetCurrentUserSubjectId()
        {
            string subjectId = "";
            using (var conn = new NpgsqlConnection(Configuration.AuthConnectionString))
            {
                conn.Open();
                string query = $"select \"SubjectId\" from \"Users\" where \"PhoneNumber\" like '%{Configuration.PhoneNumber}%' ;";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                subjectId = dr.GetString(0);
            }
            return subjectId;
        }

        public static List<FavoriteHome> GetFavoriteHomeList(string subjectId)
        {
            var favoriteHomes = new List<FavoriteHome>();
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"SELECT fe.id, h.slug FROM public.home_favorite_entity fe, public.home_entity h where h.Id = fe.home_id and subject_id = '{subjectId}';";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var favoriteHome = new FavoriteHome(dr.GetString(0), dr.GetString(1));
                    favoriteHomes.Add(favoriteHome);
                }
            }
            return favoriteHomes;
        }

        internal static void RemoveFavoriteHomes(string subjectId)
        {
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"delete from public.home_favorite_entity where subject_id = '{subjectId}';";
                var command = new NpgsqlCommand(query, conn);
                command.ExecuteNonQuery();
            }
        }

        public static void TakeScreenshot(string filename)
        {
            using var bitmap = new Bitmap(1920, 1080);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
            }
            bitmap.Save(filename, ImageFormat.Jpeg);
        }


        public static void Logout(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Utils.GoToUrl(driver, Configuration.Website);
            if (Configuration.RunHeadless)
            {
                Utils.GoToUrl(driver, Configuration.LogoutUrl);
            }
            else
            {
                var menuButtonBy = By.XPath("/html/body/div[1]/div/header/div/div/div[2]/button[2]");
                var logoutButtonBy = By.XPath("/html/body/div[3]/div[3]/ul/li");

                wait.Until(ExpectedConditions.ElementExists(menuButtonBy)).Click();

                wait.Until(ExpectedConditions.ElementExists(logoutButtonBy)).Click();
            }
        }


        public static void RemoveOwners(string homeSlug)
        {

            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $" update home_entity set owner_subject_ids= '{{}}' where slug = '{homeSlug}';";
                var command = new NpgsqlCommand(query, conn);
                int nofUpdatedRows = command.ExecuteNonQuery();
                if (nofUpdatedRows <= 0)
                {
                    throw new Exception($"Exception running update {query}");
                }
            }
        }

        public static void SetHomeSaleStatus(string homeSlug, bool isActive)
        {

            string saleStatus = isActive ? "Active" : "Passive";
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $" update home_entity set sale_status= '{saleStatus}' where slug = '{homeSlug}';";
                var command = new NpgsqlCommand(query, conn);
                int nofUpdatedRows = command.ExecuteNonQuery();
                if (nofUpdatedRows <= 0)
                {
                    throw new Exception($"Exception running update {query}");
                }
            }
        }


        internal static List<IWebElement> GetLinks(IWebDriver driver, bool justVisibleLinks)
        {
            List<IWebElement> links = driver.FindElements(By.TagName("a")).ToList();
            if (justVisibleLinks)
            {
                links = links.Where(link => (link as WebElement).Displayed).ToList();
            }

            return links;
        }

        internal static List<string> GetInternalLinks(IWebDriver driver, string websiteName, bool justVisibleLinks)
        {
            List<string> internalLinks = GetLinks(driver, justVisibleLinks).Select(link => link.GetAttribute("href")).ToList().Where(link => link != null).Where(link =>
            (link.ToLower().Contains(websiteName) && link.ToLower().Trim().StartsWith("http")) ||
            (!link.ToLower().Contains(websiteName) && !link.ToLower().Trim().StartsWith("http"))).ToList();
            internalLinks.RemoveAll(link =>
            link.Contains("https://www.facebook.com/") || link.Contains("https://www.instagram.com") || link.Contains("https://www.linkedin.com")
            || link.Contains("mailto:") || link.Contains("i0.wp.com") || link.Contains("storage.googleapis.com"));
            return internalLinks;
        }
    }
}

