using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeBrick.Test.UI.Extensions
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }

        public static List<IWebElement> FindElements(this IWebDriver driver, string tagName, string classNamesSpaceDelimitted, string textStartWith, string textEndsWith, Dictionary<string, string> attributes, int locationFromTop, int locationFromLeft, string innerHtml)
        {
            List<string> innerHtmlList = string.IsNullOrWhiteSpace(innerHtml) ? null : new List<string>() { innerHtml };
            return FindElements(driver, tagName, classNamesSpaceDelimitted, textStartWith, textEndsWith, attributes, true, Utils.WaitTime, locationFromTop, locationFromLeft, innerHtmlList);
        }

        public static List<IWebElement> FindElements(this IWebDriver driver, string tagName, string classNamesSpaceDelimitted, string textStartWith, string textEndsWith, Dictionary<string, string> attributes, int locationFromTop, int locationFromLeft, List<string> innerHtml)
        {
            return FindElements(driver, tagName, classNamesSpaceDelimitted, textStartWith, textEndsWith, attributes, true, Utils.WaitTime, locationFromTop, locationFromLeft, innerHtml);
        }

        public static List<IWebElement> FindElements(this IWebDriver driver, string tagName, string classNamesSpaceDelimitted, string textStartWith, string textEndsWith, Dictionary<string, string> attributes, bool returnOnceResultIsJustOneRecord, int timeoutInSeconds, int locationFromTop = 0, int locationFromLeft = 0, List<string> innerHtmlList = null)
        {
            int timeoutInMiliSeconds = timeoutInSeconds * 1000;
            if (timeoutInMiliSeconds == 0)
            {
                timeoutInMiliSeconds = Utils.WaitTime * 1000;
            }
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeoutInMiliSeconds));
            By by = By.TagName(tagName);
            List<IWebElement> elements = driver.FindElements(by).ToList();

            if (returnOnceResultIsJustOneRecord && elements.Count == 1)
            {
                return elements;
            }

            if (!string.IsNullOrEmpty(classNamesSpaceDelimitted))
            {
                foreach (string className in classNamesSpaceDelimitted.Split(' '))
                {
                    if (String.IsNullOrWhiteSpace(className))
                    {
                        continue;
                    }
                    elements = elements.Where(e => e.GetAttribute("class").Contains(className)).ToList();
                    if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                    {
                        return elements;
                    }
                }
            }

            if (!string.IsNullOrEmpty(textStartWith))
            {
                elements = elements.Where(e => e.GetInnerText().Trim().ToLower().StartsWith(textStartWith.ToLower())).ToList();
                if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                {
                    return elements;
                }
            }

            if (!string.IsNullOrEmpty(textEndsWith))
            {
                elements = elements.Where(e => e.GetInnerText().Trim().ToLower().StartsWith(textEndsWith.ToLower())).ToList();
                if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                {
                    return elements;
                }
            }

            if (attributes != null)
            {
                foreach (KeyValuePair<string, string> attribute in attributes)
                {
                    elements = elements.Where(e => e.GetAttribute(attribute.Key).Contains(attribute.Value)).ToList();
                    if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                    {
                        return elements;
                    }
                }
            }
            if (innerHtmlList != null)
            {
                var matches = new List<IWebElement>();
                foreach (string innerHtml in innerHtmlList)
                {
                    matches.AddRange(elements.Where(e => e.GetInnerHtml().Trim().ToLower().Contains(innerHtml.ToLower())).ToList());
                }
                elements = matches;
                if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                {
                    return elements;
                }
            }

            if (locationFromTop != 0)
            {
                List<int> distinctLocations = elements.Select(e => e.Location.Y).Distinct().ToList();
                distinctLocations.Sort();
                int targetY = distinctLocations.Skip(locationFromTop - 1).First();
                elements = elements.Where(e => e.Location.Y == targetY).ToList();
                if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                {
                    return elements;
                }
            }

            if (locationFromLeft != 0)
            {
                List<int> distinctLocations = elements.Select(e => e.Location.X).Distinct().ToList();
                distinctLocations.Sort();
                int targetX = distinctLocations.Skip(locationFromLeft - 1).First();
                elements = elements.Where(e => e.Location.X == targetX).ToList();
                if (returnOnceResultIsJustOneRecord && elements.Count == 1)
                {
                    return elements;
                }
            }
            return elements;

        }
    }
}
