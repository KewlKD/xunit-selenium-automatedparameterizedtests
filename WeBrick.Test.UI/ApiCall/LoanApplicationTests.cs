using Xunit;
using Newtonsoft.Json.Linq;
using System.Linq;


namespace WeBrick.Test.UI.ApiCall
{

    [Collection("Sequential")]
    public class LoanApplicationTests
    {

        [Fact]
        [Trait("Category", "LoanApplications")]
        public static void LoanApplicationsNotAuthorized()
        {
            JToken result = ApiUtil.GetLoanApplications().First();
            Assert.Contains("You must be logged in to get all your loan applications", result.ToString());
        }
    }
}


