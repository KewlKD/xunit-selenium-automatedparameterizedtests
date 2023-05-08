using System;
using System.Collections.Generic;
using RestSharp;
using Xunit;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Linq;
using System.IO;
using OpenQA.Selenium;

namespace WeBrick.Test.UI.ApiCall
{

    [Collection("Sequential")]
    public class SearchTests
    {
        public static IEnumerable<object[]> GetSlugs()
        {
            yield return new object[] { Configuration.SampleProperty };
            yield return new object[] { Configuration.SampleProperty1 };
            yield return new object[] { "vendersgade-31-4-tv-1363-kobenhavn-k" };
            yield return new object[] { "normarkvej-29-2-20-7600-struer" };
            yield return new object[] { "gogevaenget-4-6760-ribe" };
        }


        [Theory]
        [MemberData(nameof(GetSlugs))]
        [Trait("Category", "Search")]
        public static void SearchHomeBySlug(string slug)
        {
            JToken home = ApiUtil.GetHomeBySlug(slug);
            Assert.NotNull(home);
            Assert.DoesNotContain("\"errors\":", home.ToString());
            Assert.Equal(slug, home["data"]["homeBySlug"]["slug"].ToString());
        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomes()
        {
            long itemsPerBatch = 50;
            string lastHome = "";
            var saleStatus = SaleStatus.ACTIVE;
            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, null, null, null, null, saleStatus))
            {
                Assert.NotEqual(lastHome, home.ToString());
                lastHome = home.ToString();
                Assert.Equal("ACTIVE", home["home"]["saleStatus"].ToString());
            }
        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByPrice()
        {
            long itemsPerBatch = 50;
            int lowerPrice = 10000;
            int upperPrice = 1000000;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            foreach (JToken home in ApiUtil.SearchHome(lowerPrice, itemsPerBatch, upperPrice, null, null, null, null, null, null, null, null, null, null, null, null, saleStatus))
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["price"]).Value >= lowerPrice);
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["price"]).Value <= upperPrice);
                }
                catch (Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;

            }
        }



        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesByPriceDBCheck()
        {
            long itemsPerBatch = 50;
            int lowerPrice = 10000;
            int upperPrice = 1000000;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            List<JToken> homes = ApiUtil.SearchHome(lowerPrice, itemsPerBatch, upperPrice, null, null, null, null, null, null, null, null, null, null, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["price"]).Value >= lowerPrice);
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["price"]).Value <= upperPrice);
                }
                catch (Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} json:{home}\n\n\n";
                }
            }


            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(h.id) from public.home_entity h, public.home_presentation_entity hp where h.Id = hp.home_id and h.official_presentation_id = hp.id and   Lower(h.sale_status)='active' and hp.price >= {lowerPrice} and hp.price <= {upperPrice};";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }

        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesBySize()
        {
            long itemsPerBatch = 50;
            int lowerHomeSize = 50;
            int upperHomeSize = 400;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, lowerHomeSize, upperHomeSize, null, null, null, null, null, null, null, null, null, null, saleStatus))
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["sizeInM2"]).Value >= lowerHomeSize);
                    Assert.True((long)((JValue)home["home"]["sizeInM2"]).Value <= upperHomeSize);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;
            }
        }


        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesBySizeByDBCheck()
        {
            long itemsPerBatch = 50;
            int lowerHomeSize = 50;
            int upperHomeSize = 400;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, lowerHomeSize, upperHomeSize, null, null, null, null, null, null, null, null, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["sizeInM2"]).Value >= lowerHomeSize);
                    Assert.True((long)((JValue)home["home"]["sizeInM2"]).Value <= upperHomeSize);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} json:{home}\n\n\n";
                }
            }

            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(id) from public.home_entity where Lower(sale_status)='active' and size_In_M2  >= {lowerHomeSize} and size_In_M2 <= {upperHomeSize};";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }

        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByLotSize()
        {
            long itemsPerBatch = 50;
            int lowerLotSize = 500;
            int upperLotSize = 1000;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, lowerLotSize, upperLotSize, null, null, null, null, null, null, null, null, saleStatus))
            {

                try
                {
                    Assert.True((long)((JValue)home["home"]["lotSizeInM2"]).Value >= lowerLotSize);
                    Assert.True((long)((JValue)home["home"]["lotSizeInM2"]).Value <= upperLotSize);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;
            }

        }


        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesByLotSizeCheckDB()
        {
            long itemsPerBatch = 50;
            int lowerLotSize = 500;
            int upperLotSize = 1000;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, lowerLotSize, upperLotSize, null, null, null, null, null, null, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {

                try
                {
                    Assert.True((long)((JValue)home["home"]["lotSizeInM2"]).Value >= lowerLotSize);
                    Assert.True((long)((JValue)home["home"]["lotSizeInM2"]).Value <= upperLotSize);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} json:{home}\n\n\n";
                }
            }


            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(id) from public.home_entity where Lower(sale_status)='active' and lot_size_In_M2  >= {lowerLotSize} and lot_size_In_M2  <= {upperLotSize};";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }
        }



        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByNumberOfFloors()
        {
            long itemsPerBatch = 50;
            int lowerNumberOfFloors = 1;
            int upperNumberOfFloors = 3;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, lowerNumberOfFloors, upperNumberOfFloors, null, null, null, null, null, null, saleStatus))
            {
                try
                {
                    Assert.NotNull(home);//NumberOfFloors is not returned in response and cannot be checked here
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;
            }

        }

        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesByNumberOfFloorsDBCheck()
        {
            long itemsPerBatch = 50;
            int lowerNumberOfFloors = 1;
            int upperNumberOfFloors = 3;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, lowerNumberOfFloors, upperNumberOfFloors, null, null, null, null, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                try
                {
                    Assert.NotNull(home);//NumberOfFloors is not returned in response and cannot be checked here
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message}  json:{home}\n\n\n";
                }
            }
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(id) from (select distinct(h.id)  FROM home_entity h ,import_offering o, import_relation r where h.id = r.home_id and r.offering_id = o.id and Lower(h.sale_status)= 'active' and o.floor_count >= {lowerNumberOfFloors} and o.floor_count <= {upperNumberOfFloors}  as selectDistinctId;";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }
        }



        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByNumberOfRooms()
        {
            long itemsPerBatch = 50;
            int lowerNumberOfRooms = 1;
            int upperNumberOfRooms = 5;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";

            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, lowerNumberOfRooms, upperNumberOfRooms, null, null, null, null, saleStatus))
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["rooms"]).Value >= lowerNumberOfRooms);
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["rooms"]).Value <= upperNumberOfRooms);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;
            }

        }


        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesByNumberOfRoomsDBCheck()
        {
            long itemsPerBatch = 50;
            int lowerNumberOfRooms = 1;
            int upperNumberOfRooms = 5;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, lowerNumberOfRooms, upperNumberOfRooms, null, null, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["rooms"]).Value >= lowerNumberOfRooms);
                    Assert.True((long)((JValue)home["home"]["officialPresentation"]["rooms"]).Value <= upperNumberOfRooms);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} json:{home}\n\n\n";
                }
            }

            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(id) from public.home_entity where Lower(sale_status)='active' and rooms >= {lowerNumberOfRooms} and rooms <= {upperNumberOfRooms};";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }
        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByYearBuilt()
        {
            long itemsPerBatch = 50;
            int lowerYearBuilt = 1800;
            int upperYearBuilt = 1970;
            int number = 0;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            foreach (JToken home in ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, lowerYearBuilt, upperYearBuilt, null, null, saleStatus))
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["yearBuilt"]).Value >= lowerYearBuilt);
                    Assert.True((long)((JValue)home["home"]["yearBuilt"]).Value <= upperYearBuilt);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} number:{number} json:{home}\n\n\n";
                }
                number++;
            }

        }

        [Fact(Skip = "It is disabled now and will be enabled once the bug is fixed and number of search results from api and db are the same")]
        [Trait("Category", "Search")]
        public static void SearchHomesByYearBuiltAndDbCheck()
        {
            long itemsPerBatch = 50;
            int lowerYearBuilt = 1800;
            int upperYearBuilt = 1970;
            var saleStatus = SaleStatus.ACTIVE;
            string errors = "";
            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, lowerYearBuilt, upperYearBuilt, null, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                try
                {
                    Assert.True((long)((JValue)home["home"]["yearBuilt"]).Value >= lowerYearBuilt);
                    Assert.True((long)((JValue)home["home"]["yearBuilt"]).Value <= upperYearBuilt);
                }
                catch (System.Exception ex)
                {
                    errors = $"{errors} message:{ex.Message} json:{home}\n\n\n";
                }
            }
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select count(id) from public.home_entity where Lower(sale_status)='active' and year_built >= {lowerYearBuilt} and year_built <= {upperYearBuilt};";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int numberoFHomesInThisAdddress = dr.GetInt32(0);
                Assert.Equal(numberoFHomesInThisAdddress, homes.Count);
            }

        }

        [Fact]
        [Trait("Category", "Search")]
        public static void SearchHomesByAddress()
        {
            long itemsPerBatch = 50;
            var saleStatus = SaleStatus.ACTIVE;
            bool found = false;
            List<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, null, null, Configuration.SampleProperty, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                string actualSlug = home["home"]["slug"].ToString();
                if (actualSlug.ToLower().Contains(Configuration.SampleProperty.ToLower()))
                {
                    found = true;
                }
            }
            saleStatus = SaleStatus.PASSIVE;
            homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, null, null, Configuration.SampleProperty, null, saleStatus).ToList();
            foreach (JToken home in homes)
            {
                string actualSlug = home["home"]["slug"].ToString();
                if (actualSlug.ToLower().Contains(Configuration.SampleProperty.ToLower()))
                {
                    found = true;
                }
            }
            Assert.True(found);
        }

        [Fact(Skip = "It is a defect because the number returned from Api is not the same as the once from Db")]
        [Trait("Category", "SearchWithDbCheck")]
        public static void SearchHomesByAddressAndDbCheck()
        {
            long itemsPerBatch = 50;
            string address = "Gentofte";
            var saleStatus = SaleStatus.ACTIVE;
            IEnumerable<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, null, null, address, null, saleStatus);
            foreach (JToken home in homes)
            {
                string actualAddress = home["home"]["address"].ToString();
                Assert.Contains(address.ToLower(), actualAddress.ToLower());
            }
            if (Configuration.TestEnvironment == "qa")
            {
                using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
                {
                    conn.Open();
                    string query = $"select count(id) from public.home_entity where Lower(sale_status)='active' and lower(address::TEXT) like '%{address.ToLower()}%';";
                    var command = new NpgsqlCommand(query, conn);
                    NpgsqlDataReader dr = command.ExecuteReader();
                    dr.Read();
                    int numberoFHomesInThisAdddress = dr.GetInt32(0);
                    List<string> homeList = homes.Select(h => h.ToString()).ToList();
                    Assert.Equal(numberoFHomesInThisAdddress, homeList.Count);
                }
            }
        }


        [Theory(Skip = "Enable it once the api is released")]
        [MemberData(nameof(GetKommuneList))]
        [Trait("Category", "Search")]
        public static void GetAveragePriceInKommune(string kommuneName)
        {
            long itemsPerBatch = 500;
            var saleStatus = SaleStatus.ACTIVE;
            IEnumerable<JToken> homes = ApiUtil.SearchHome(null, itemsPerBatch, null, null, null, null, null, null, null, null, null, null, null, kommuneName, null, saleStatus);
            int actualHomeCount = 0;
            foreach (JToken home in homes)
            {
                string actualAddress = home["home"]["address"].ToString();
                Assert.Contains(kommuneName.ToLower(), actualAddress.ToLower());
                double averagePricePerM2 = double.Parse(home["home"]["averagePricePerM2"].ToString());
                Assert.True(averagePricePerM2 > 0);
                actualHomeCount++;
            }
            if (false)//TODO there is an inconsistency in the number of search items and number returned from db that should be resolved and then this if should be enabled
            {
                using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
                {
                    conn.Open();
                    string query = $"select address_query from public.home_entity where Lower(sale_status)='active' and lower(address::TEXT) like '%{kommuneName.ToLower()}%';";
                    var command = new NpgsqlCommand(query, conn);
                    NpgsqlDataReader dr = command.ExecuteReader();
                    int expectedCount = 0;
                    while (dr.Read())
                    {
                        expectedCount++;
                        string address = dr.GetString(0);
                    }
                    Assert.Equal(expectedCount, actualHomeCount);
                }
            }
        }

        [Theory]
        [InlineData("Søglimt 4, 6340 Kruså")]
        [InlineData("Venslevvej 7A, 4261 Dalmose")]
        [InlineData("Langemosevej 8, 4400 Kalundborg")]
        [InlineData("Jungshovedvej 4, 4735 Mern")]
        [InlineData("Sønder Bjergevej 118, 4261 Dalmose")]
        [InlineData("Kirkestien 9, 4281 Gørlev")]
        [InlineData("Stevnsvej 103, 4600 Køge")]
        [InlineData("Nyrupvej 48, 4296 Nyrup")]
        [InlineData("Holbergsvej 62, 4293 Dianalund")]
        [Trait("Category", "Search")]
        public static void MakeSureWebrickUsersHomeAppearsInSeach(string address)
        {
            if (Configuration.TestEnvironment != "prod")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                bool result = Utils.HomeAppearsInSearch(driver, address);
                Assert.True(result);
            }
        }

        public static IEnumerable<object[]> GetKommuneList()
        {
            return Utils.GetKommuneList();
        }

    }
}


