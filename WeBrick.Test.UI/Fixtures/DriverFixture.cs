using OpenQA.Selenium;
using System;

namespace WeBrick.Test.UI.Fixtures
{
    public class DriverFixture : IDisposable
    {
        public IWebDriver Driver { get; }
        public DriverFixture()
        {
            Driver = Utils.GetChromeDriver();
        }

        public void Dispose()
        {
            Driver.Dispose();
            Utils.MakeSureChromeDriveDoesNotExist();
        }
    }
}
