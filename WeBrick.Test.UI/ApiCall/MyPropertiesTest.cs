using System.Collections.Generic;
using RestSharp;
using Xunit;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace WeBrick.Test.UI.ApiCall
{

    [Collection("Sequential")]
    public class MyPropertiesTest
    {

        [Fact]
        [Trait("Category", "MyProperties")]
        private static void LoadMyPropertiesPage()
        {
            List<JToken> homes = ApiUtil.SearchHome(null, 50, null, null, null, null, null, null, null, null, null, null, null, null, null, SaleStatus.ACTIVE).ToList();
            if (homes.Count == 0)
            {
                homes = ApiUtil.SearchHome(null, 50, null, null, null, null, null, null, null, null, null, null, null, null, null, SaleStatus.PASSIVE).ToList();
            }
            JToken responseJson = ApiUtil.GetHomeBySlug(homes[0]["home"]["slug"].ToString());
            long valuationInDkk = ((long)((JValue)responseJson["data"]["homeBySlug"]["valuationInDkk"]).Value);
            Assert.True(valuationInDkk >= 0);
            long popularity = ((long)((JValue)responseJson["data"]["homeBySlug"]["popularity"]).Value);
            Assert.True(popularity >= 0);
        }

    }
}







