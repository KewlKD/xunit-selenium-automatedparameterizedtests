using OpenQA.Selenium;
using System;

namespace WeBrick.Test.UI.Websites
{
    public class SearchPage : Website
    {
        public override string Url => $"{Configuration.Website}soeg";
        public override By AcceptAllCoockiesId => throw new NotImplementedException();
        public override IWebElement AcceptAllCoockies => throw new NotImplementedException();
        public override IWebDriver Driver { get; internal set; }

        public static By ByRandomElementIsShowing => By.CssSelector("#__next > div > div > div.MuiGrid-root.jss24.MuiGrid-container.MuiGrid-spacing-xs-2 > div:nth-child(1)");
        public IWebElement RandomElementIsShowing => Driver.FindElement(ByRandomElementIsShowing);


        public static By BySearchSpecificHouse => By.XPath("/html/body/div[1]/div/div/div[2]/div[1]/div/a/div/div[1]/img");
        public IWebElement SearchSpecificHouse => Driver.FindElement(BySearchSpecificHouse);


        public static By ByActualHomePrice => By.XPath("/html/body/div[1]/div/section/div/div/div[2]/div/section[1]/div/div[1]");
        public IWebElement ActualHomePrice => Driver.FindElement(ByActualHomePrice);


        public static By BySearchInputField => By.XPath("/html/body/div[1]/div/div/div[1]/div[1]/div/input");
        public IWebElement SearchInputField => Driver.FindElement(BySearchInputField);


        public static By BySearchButton => By.XPath("/html/body/div[1]/div/div/div[1]/div[1]/div/div/button");
        public IWebElement SearchButton => Driver.FindElement(BySearchButton);


        public static By ByHomesForSaleCount => By.XPath(@"//*[@id=""__next""]/ div/div/div[1]/div[1]/button");
        public IWebElement HomesForSaleCount => Driver.FindElement(ByHomesForSaleCount);

        public static By ByHeader => By.XPath(@"//*[@id=""__next""]/div/footer/div/div/div[1]");
        public IWebElement Header => Driver.FindElement(ByHeader);











        // Paths for login and logout
        public static By ByLoginErrorMessage => By.XPath("/html/body/main/div/div/form/fieldset/div[2]/span");
        public IWebElement LoginErrorMessage => Driver.FindElement(ByLoginErrorMessage);
        public static By ByMenuButton => By.XPath(@"//*[@id=""__next""]/ div[1]/header/div/div/div[2]/button[2]");
        public IWebElement MenuButton => Driver.FindElement(ByMenuButton);

        public static By ByLoginButton => By.XPath("/html/body/div[3]/div[3]/ul/li");
        public IWebElement LoginButton => Driver.FindElement(ByLoginButton);

        public static By ByLogoutButton => By.CssSelector("body>div.MuiPopover-root.jss47>div.MuiPaper-root.MuiMenu-paper.MuiPopover-paper.MuiPaper-elevation8.MuiPaper-rounded>ul>li");
        public IWebElement LogoutButton => Driver.FindElement(ByLogoutButton);

        public static By ByVilla => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[2]/span[1]");
        public IWebElement VillaSpan => Driver.FindElement(ByVilla);

        public static By ByVillalejlighed => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[2]/span[1]");
        public IWebElement VillalejlighedSpan => Driver.FindElement(ByVillalejlighed);

        public static By ByLejlighed => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[3]/span[1]");
        public IWebElement LejlighedSpan => Driver.FindElement(ByLejlighed);


        public static By ByRækkehus => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[4]/span[1]");
        public IWebElement RækkehusSpan => Driver.FindElement(ByRækkehus);

        public static By ByLandejendom => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[5]/span[1]");
        public IWebElement LandejendomSpan => Driver.FindElement(ByLandejendom);


        public static By ByAndet => By.XPath("/html/body/div[1]/div/div/div[1]/div[2]/div/div[6]/span[1]");
        public IWebElement AndetSpan => Driver.FindElement(ByAndet);


        public static By ByAdvancedSearchSlider => By.XPath(@"//*[@id=""__next""]/ div/div/div[1]/div[3]/div[1]/div[2]");
        public IWebElement AdvancedSearchSliderSpan => Driver.FindElement(ByAdvancedSearchSlider);


        public static By ByPriceUpperSlider => By.XPath(@"//*[@id=""__next""]/div/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[1]/div[2]/div/span/span[4]");
        public IWebElement PriceUpperSliderSpan => Driver.FindElement(ByPriceUpperSlider);

        public static By ByPriceUpperSliderValue => By.XPath(@"//*[@id=""__next""]/ div[1]/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[1]/div[2]/div/div/span[2]");
        public IWebElement PriceUpperSliderValueSpan => Driver.FindElement(ByPriceUpperSliderValue);


        public static By BySizeUpperSlider => By.XPath(@"//*[@id=""__next""]/div/div[1]/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[2]/div[2]/div/span/span[4]");
        public IWebElement SizeUpperSliderSpan => Driver.FindElement(BySizeUpperSlider);

        public static By BySizeUpperSliderValue => By.XPath(@"//*[@id=""__next""]/div/div[1]/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[2]/div[2]/div/div/span[2]/span");
        public IWebElement SizeUpperSliderValueSpan => Driver.FindElement(BySizeUpperSliderValue);


        public static By ByLandSizeUpperSlider => By.XPath(@"//*[@id=""__next""]/div/div[1]/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[3]/div[2]/div/span/span[4]");
        public IWebElement LandSizeUpperSliderSpan => Driver.FindElement(ByLandSizeUpperSlider);

        public static By ByLandSizeUpperSliderValue => By.XPath(@"//*[@id=""__next""]/div/div[1]/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[3]/div[2]/div/div/span[2]/span");
        public IWebElement LandSizeUpperSliderValueSpan => Driver.FindElement(ByLandSizeUpperSliderValue);


        public static By ByFloorPlanUpperSlider => By.XPath(@"//*[@id=""__next""]/div/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[4]/div[2]/div/span/span[4]");
        public IWebElement FloorPlanUpperSliderSpan => Driver.FindElement(ByFloorPlanUpperSlider);

        public static By ByFloorPlanUpperSliderValue => By.XPath(@"//*[@id=""__next""]/div/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[4]/div[2]/div/div/span[2]");
        public IWebElement FloorPlanUpperSliderValueSpan => Driver.FindElement(ByFloorPlanUpperSliderValue);


        public static By ByNumberRoomUpperSlider => By.XPath(@"//*[@id=""__next""]/div[1]/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[5]/div[2]/div/span/span[4]");
        public IWebElement NumberRoomUpperSliderSpan => Driver.FindElement(ByNumberRoomUpperSlider);

        public static By ByNumberRoomUpperSliderValue => By.XPath(@"//*[@id=""__next""]/ div[1]/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[5]/div[2]/div/div/span[2]");
        public IWebElement NumberRoomUpperSliderValueSpan => Driver.FindElement(ByNumberRoomUpperSliderValue);


        public static By ByYearBuiltUpperSlider => By.XPath(@"//*[@id=""__next""]/ div[1]/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[6]/div[2]/div/span/span[4]");
        public IWebElement YearBuiltUpperSliderSpan => Driver.FindElement(ByYearBuiltUpperSlider);


        public static By ByYearBuiltUpperSliderValue => By.XPath(@"//*[@id=""__next""]/ div[1]/div/div[1]/div[3]/div[2]/div/div/div/div/div[1]/div[6]/div[2]/div/div/span[2]");
        public IWebElement YearBuiltUpperSliderValueSpan => Driver.FindElement(ByYearBuiltUpperSliderValue);



        public SearchPage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
