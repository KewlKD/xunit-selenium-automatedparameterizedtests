
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using WeBrick.Test.UI.Extensions;

namespace WeBrick.Test.UI.Websites
{
    public class SelfSalePage : Website
    {
        public override string Url => $"{Configuration.Website}my-properties/find";
        public override By AcceptAllCoockiesId => throw new NotImplementedException();
        public override IWebElement AcceptAllCoockies => throw new NotImplementedException();
        public override IWebDriver Driver { get; internal set; }

        public IWebElement SearchtextBox => Driver.FindElements("input", "MuiInputBase-input MuiInputBase-inputAdornedEnd", "", "", null, 1, 1, "")[0];
        public SelfSalePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
