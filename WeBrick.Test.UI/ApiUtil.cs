using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using Xunit;


namespace WeBrick.Test.UI
{
    public static class ApiUtil
    {
        const int MaxNumberToCheck = 100;

        public static IEnumerable<JToken> My(string subjectId)
        {
            int pageNumber = -1;
            long totalItems = 0;
            int numberOfTestedHomes = 0;
            string nextCursor = "";

            pageNumber++;
            string query = @"query 
{  
	my 
	{ 
		favorite 
		{  
			homes 
			{  
				id
				createdAt
				homeId
				home 
				{  
					id
					slug
					address 
					{ 
						street1
						street2
						postcode
						city
					}
					location 
					{ 
						latitude
						longitude
					}
					sizeInM2
					lotSizeInM2
					type
					primaryHeatSource
					secondaryHeatSource
					yearBuilt
					yearRenovated
					latestSaleYear
					latestSaleType
					latestSalePrice
					isInCityZone
					isInCountrySideZone
					isInSummerHouseZone
					landRegistrationNumber
					picturesForEditing 
					{  
						id
						status
						description
						url 
					}
					officialPresentation 
					{  
						id
						homeId
						status
						title
						teaser
						description
						price
						pricePerM2
						pictures 
						{  
							id
							status
							description
							url
						}
						rooms
						yearRenovated
						toilets
						baths
						unregisteredStructures 
						{  
							type
							sizeInM2 
						}
						monthlyHeatingCost
						monthlyElectricityCost
						monthlyWaterCost
						monthlyInsuranceCost
						monthlyLandTaxCost
						monthlyOwnersUnionCost
						totalMonthlyCost
						aboutOwnerTitle
						aboutOwnerDescription 
					}
					draftPresentation 
					{  
						id
						homeId
						status
						title
						teaser
						description
						price
						pricePerM2
						pictures 
						{  
							id
							status
							description
							url  
						}
						rooms
						yearRenovated
						toilets
						baths
						unregisteredStructures 
						{  
							type
							sizeInM2 
						}
						monthlyHeatingCost
						monthlyElectricityCost
						monthlyWaterCost
						monthlyInsuranceCost
						monthlyLandTaxCost
						monthlyOwnersUnionCost
						totalMonthlyCost
						aboutOwnerTitle
						aboutOwnerDescription 
					}
					ownerIds
					owners 
					{  
						id
						firstNames
						lastNames
						fullName
						isProfileVerified
						avatarUrl
					}
					hasVerifiedOwner
					municipalityCode
					municipality 
					{  
						code
						name 
					}
					saleStatus
					daysSinceCurrentSaleStatusWasSet
					isMyFavorite
					energyRating
					energyRatingExpiresAt
					ejendomsdatarapportInfo 
					{ 
						status
					} 
				}
				subjectId
				subject 
				{  
					firstNames
					lastNames
					fullName
					isProfileVerified
					avatarUrl
				} 
			}
		} 
	} 
} 
";
            using (var client = new RestClient(Configuration.Api))
            {
                JToken homeList = null;
                JToken home = null;
                RestResponse response = null;
                int homesInCurrentBatch = 0;
                string url = $"{Configuration.Api}graphql";

                var request = new RestRequest(url, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("text/plain", query, ParameterType.RequestBody);

                response = client.ExecuteAsync(request).Result;
                Assert.True(response.IsSuccessful);
                JObject responseJson = JObject.Parse(response.Content);
                totalItems = (long)((JValue)responseJson["data"]["listHomes"]["pagination"]["totalItems"]).Value;
                nextCursor = responseJson["data"]["listHomes"]["pagination"]["nextCursor"].ToString();
                homeList = responseJson["data"]["listHomes"]["items"];
                home = homeList.First;
                while (home != null)
                {
                    Assert.False(string.IsNullOrEmpty(response?.Content));
                    Assert.False(response?.Content.Contains("error"), response?.Content);
                    if (!string.IsNullOrWhiteSpace(home["home"].ToString()))
                    {
                        Assert.True(home["home"]["latestSalePriceInDkk"] != null || (home["home"] as JValue)?.Value != null);
                        Assert.True(((JValue)home["home"]["latestSalePrice"]).Value == null || long.Parse(home["home"]["latestSalePrice"].ToString()) > 0);

                        try
                        {
                            if (((JContainer)home["home"]["allPresentations"]).Count > 1)
                            {

                            }
                        }
                        catch (Exception)
                        {

                        }
                        homesInCurrentBatch++;
                        numberOfTestedHomes++;
                        yield return (home);
                    }
                    home = home.Next;
                }
            }
        }

        public static IEnumerable<JToken> GetLoanApplications()
        {

            var query = @"{ 
    ""operationName"": ""LoanApplications"",

	""variables"": {  },
    ""query"": ""query LoanApplications {\n  loanApplications {\n    id\n    createdAt\n    lastUpdatedAt\n    homeId\n    stateId\n    amount\n    homeType\n    municipalityCodes\n    
state {\n      id\n      state\n      __typename\n    }\n    applicants {\n      id\n      createdAt\n      lastUpdatedAt\n      applicationId\n      firstName\n      lastName\n      
phone\n      email\n      address\n      homeId\n      wealthTotal\n      debtTotal\n      documents {\n        id\n        createdAt\n        lastUpdatedAt\n        applicantId\n        
typeId\n        filename\n        mimeType\n        meta\n        type {\n          id\n          type\n          __typename\n        }\n        __typename\n      }\n      __typename\n    
}\n    childrenAges\n    vehiclePlateNumbers\n    archived\n    __typename\n  }
\n}""
}";
            using (var client = new RestClient(Configuration.Api))
            {
                JToken homeList = null;
                JToken home = null;
                RestResponse response = null;
                int homesInCurrentBatch = 0;
                string url = $"{Configuration.Api}graphql";

                var request = new RestRequest(url, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("text/plain", query, ParameterType.RequestBody);

                response = client.ExecuteAsync(request).Result;
                Assert.True(response.IsSuccessful);
                JObject responseJson = JObject.Parse(response.Content);
                yield return responseJson;
            }
        }


        public static JToken GetHomeBySlug(string slug)
        {
            var query = @"{ 
    ""operationName"": ""HomeBySlug"",
    ""variables"": { 
                ""slug"": """ + slug + @"""
    },
    ""query"": ""query HomeBySlug($slug: String!) {\n  homeBySlug(slug: $slug) {\n    id\n    slug\n    popularity\n    hasVerifiedOwner\n    isMyFavorite\n    valuationInDkk\n    address {\n      street1\n      street2\n      postcode\n      city\n      __typename\n    }\n    location {\n      latitude\n      longitude\n      __typename\n    }\n    energyRating\n    energyRatingExpiresAt\n    saleStatus\n    daysSinceCurrentSaleStatusWasSet\n    sizeInM2\n    lotSizeInM2\n    type\n    primaryHeatSource\n    secondaryHeatSource\n    yearBuilt\n    yearRenovated\n    latestSaleYear\n    latestSaleType\n    latestSalePrice\n    isInCityZone\n    isInCountrySideZone\n    isInSummerHouseZone\n    landRegistrationNumber\n    ownerIds\n    brokerLink\n    owners {\n      id\n      fullName\n      firstNames\n      lastNames\n      isProfileVerified\n      avatarUrl\n      __typename\n    }\n    officialPresentation {\n      id\n      homeId\n      status\n      title\n      teaser\n      description\n      price\n      pricePerM2\n      pictures {\n        id\n        url\n        description\n        __typename\n      }\n      rooms\n      yearRenovated\n      toilets\n      baths\n      unregisteredStructures {\n        type\n        sizeInM2\n        __typename\n      }\n      monthlyHeatingCost\n      monthlyElectricityCost\n      monthlyWaterCost\n      monthlyInsuranceCost\n      monthlyLandTaxCost\n      monthlyOwnersUnionCost\n      totalMonthlyCost\n      aboutOwnerDescription\n      __typename\n    }\n    draftPresentation {\n      id\n      homeId\n      status\n      title\n      teaser\n      description\n      price\n      pricePerM2\n      pictures {\n        id\n        url\n        description\n        __typename\n      }\n      rooms\n      yearRenovated\n      toilets\n      baths\n      unregisteredStructures {\n        type\n        sizeInM2\n        __typename\n      }\n      monthlyHeatingCost\n      monthlyElectricityCost\n      monthlyWaterCost\n      monthlyInsuranceCost\n      monthlyLandTaxCost\n      monthlyOwnersUnionCost\n      totalMonthlyCost\n      aboutOwnerDescription\n      __typename\n    }\n    documents {\n      all {\n        id\n        type\n        looseType\n        title\n        description\n        uploadedAt\n        uploadedBy\n        url\n        __typename\n      }\n      elinstallationsrapport {\n        id\n        type\n        looseType\n        title\n        description\n        uploadedAt\n        uploadedBy\n        url\n        __typename\n      }\n      tilstandsrapport {\n        id\n        type\n        looseType\n        title\n        description\n        uploadedAt\n        uploadedBy\n        url\n        __typename\n      }\n      energimaerkning {\n        id\n        type\n        looseType\n        title\n        description\n        uploadedAt\n        uploadedBy\n        url\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n""
}";
            using (var client = new RestClient(Configuration.Api))
            {
                JToken homeList = null;
                JToken home = null;
                RestResponse response = null;
                int homesInCurrentBatch = 0;
                string url = $"{Configuration.Api}graphql";

                var request = new RestRequest(url, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("text/plain", query, ParameterType.RequestBody);

                response = client.ExecuteAsync(request).Result;
                Assert.True(response.IsSuccessful);
                JObject responseJson = JObject.Parse(response.Content);
                return responseJson;
            }
            return null;
        }


        public static IEnumerable<JToken> SearchHome(int? lowerPrice, long itemsPerBatch, int? upperPrice, int? lowerHomeSize, int? upperHomeSize, int? lowerLotSize, int? upperLotSize, int? lowerNumberOfFloors, int? upperNumberOfFloors, int? lowerNumberOfRooms, int? upperNumberOfRooms, int? lowerYearBuild, int? upperYearBuild, string address, string bounds, SaleStatus saleStatus)
        {
            int pageNumber = -1;
            long totalItems = 0;
            int numberOfTestedHomes = 0;
            string nextCursor = "";

            while (pageNumber == -1 || (totalItems > 0 && numberOfTestedHomes <= MaxNumberToCheck && numberOfTestedHomes < totalItems && (pageNumber + 1) * itemsPerBatch < 10000))
            {
                pageNumber++;
                //TODO enable it for other environement when prod updated with latest version
                string averagePricePerM2Str = "averagePricePerM2\n";
                string allPresentationsStr = "allPresentations{\n          id\n          title\n          price\n}\n";
                string query = @"{ 
    ""operationName"": ""SearchHomes"",
    ""variables"": { 
                    ""criteria"": { 
                        ""homeTypes"": null,
            ""lowerPrice"": " + (lowerPrice == null ? "null" : lowerPrice.ToString()) + @",
            ""upperPrice"": " + (upperPrice == null ? "null" : upperPrice.ToString()) + @",
            ""lowerHomeSize"": " + (lowerHomeSize == null ? "null" : lowerHomeSize.ToString()) + @",
            ""upperHomeSize"": " + (upperHomeSize == null ? "null" : upperHomeSize.ToString()) + @",
            ""lowerLotSize"": " + (lowerLotSize == null ? "null" : lowerLotSize.ToString()) + @",
            ""upperLotSize"": " + (upperLotSize == null ? "null" : upperLotSize.ToString()) + @",
            ""lowerNumberOfFloors"": " + (lowerNumberOfFloors == null ? "null" : lowerNumberOfFloors.ToString()) + @",
            ""upperNumberOfFloors"": " + (upperNumberOfFloors == null ? "null" : upperNumberOfFloors.ToString()) + @",
            ""lowerNumberOfRooms"": " + (lowerNumberOfRooms == null ? "null" : lowerNumberOfRooms.ToString()) + @",
            ""upperNumberOfRooms"": " + (upperNumberOfRooms == null ? "null" : upperNumberOfRooms.ToString()) + @",
            ""lowerYearBuilt"": " + (lowerYearBuild == null ? "null" : lowerYearBuild.ToString()) + @",
            ""upperYearBuilt"": " + (upperYearBuild == null ? "null" : upperYearBuild.ToString()) + @",
            ""address"":" + (address == null ? "null" : @"""" + address + @"""") + @",
            ""bounds"": " + (bounds == null ? "null" : bounds) + @",
            ""saleStatus"": """ + saleStatus + @""",
                    },
        ""pagination"": { 
                                            " + (string.IsNullOrWhiteSpace(nextCursor)
                    ? string.Empty
                    : @"""cursor"":""" + nextCursor + @""",") + @"
                                            ""itemsPerBatch"": " + itemsPerBatch + @"
                            }
                },
    ""query"": ""query SearchHomes($criteria: SearchHomesCriteriaInput!, $pagination: PaginationInput!) {\n  listHomes: searchHomes(criteria: $criteria, pagination: $pagination) {\n    items {\n      home {\n        " +
    averagePricePerM2Str
    + @"        location {\n          latitude\n          longitude\n          __typename\n        }\n        id\n        yearBuilt\n        slug\n        latestSalePriceInDkk\n        latestSalePrice\n        isMyFavorite\n        address {\n          street1\n          street2\n          postcode\n          city\n          __typename\n        }\n        location {\n          latitude\n          longitude\n          __typename\n        }\n        saleStatus\n        sizeInM2\n        lotSizeInM2\n        \n        " +
    allPresentationsStr
    + @"          officialPresentation {\n          id\n          title\n          price\n          pricePerM2\n          description\n          pictures {\n            id\n            url\n            description\n            __typename\n          }\n          rooms\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    pagination {\n      itemsPerBatch\n      nextCursor\n      previousCursor\n      totalItems\n      __typename\n    }\n    __typename\n  }\n}
""
}
            ";
                using (var client = new RestClient(Configuration.Api))
                {
                    JToken homeList = null;
                    JToken home = null;
                    RestResponse response = null;
                    int homesInCurrentBatch = 0;
                    string url = $"{Configuration.Api}graphql";

                    var request = new RestRequest(url, Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("text/plain", query, ParameterType.RequestBody);

                    response = client.ExecuteAsync(request).Result;
                    Assert.True(response.IsSuccessful);
                    JObject responseJson = JObject.Parse(response.Content);

                    totalItems = (long)((JValue)responseJson["data"]["listHomes"]["pagination"]["totalItems"]).Value;

                    nextCursor = responseJson["data"]["listHomes"]["pagination"]["nextCursor"].ToString();
                    homeList = responseJson["data"]["listHomes"]["items"];
                    home = homeList.First;
                    while (home != null)
                    {
                        Assert.False(string.IsNullOrEmpty(response?.Content));
                        Assert.False(response?.Content.Contains("error"), response?.Content);
                        if (!string.IsNullOrWhiteSpace(home["home"].ToString()))
                        {
                            Assert.True(home["home"]["latestSalePriceInDkk"] != null || (home["home"] as JValue)?.Value != null);
                            Assert.True(((JValue)home["home"]["latestSalePrice"]).Value == null || long.Parse(home["home"]["latestSalePrice"].ToString()) > 0);

                            homesInCurrentBatch++;
                            numberOfTestedHomes++;
                            yield return (home);
                        }
                        home = home.Next;
                    }
                    if ((pageNumber + 1) * itemsPerBatch >= totalItems)
                    {
                        Assert.True(homesInCurrentBatch <= itemsPerBatch);
                    }
                }
            }
            //it can be uncommented if the maxNumberToCheck is big enough Assert.Equal(totalItems, numberOfTestedHomes);
        }

    }
}
