using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeBrick.Test.UI.Models.Auth
{
    public class NonValidatingAuthoritiyValidationStrategy : IAuthorityValidationStrategy
    {
        public AuthorityValidationResult IsIssuerNameValid(string issuerName, string expectedAuthority)
        {
            return AuthorityValidationResult.SuccessResult;
        }

        public AuthorityValidationResult IsEndpointValid(string endpoint, IEnumerable<string> expectedAuthority)
        {
            return AuthorityValidationResult.SuccessResult;
        }
    }
}
