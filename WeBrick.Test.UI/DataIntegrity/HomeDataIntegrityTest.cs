using Xunit;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using Npgsql;

namespace WeBrick.Test.UI.ApiCall
{

    [Collection("Sequential")]
    public class HomeDataIntegrityTest
    {

        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckDuplicateHomes()
        {

            var ownerIds = new List<string>();
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM home_entity GROUP BY broker_link,slug HAVING COUNT(*) > 1";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    long numberOfDuplicateHomes = dr.GetInt64(0);
                    Assert.Equal(0, numberOfDuplicateHomes);
                }
            }
        }

        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckPresentationsWithNoHome()
        {
            var ownerIds = new List<string>();
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = @"with subquery as (
SELECT  p.id as pid, h.id as hid FROM public.home_presentation_entity p
left JOIN public.home_entity h ON p.home_Id = h.id
	) select pid from subquery where hid is null;
	";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string presentationWithNoHome = dr.GetString(0);
                    Assert.Equal("", presentationWithNoHome);
                }
            }
        }

        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckPictureWithNoHome()
        {

            var ownerIds = new List<string>();
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = @"with subquery as (
SELECT  p.id as pid, h.id as hid FROM public.home_picture_entity p
left JOIN public.home_entity h ON p.home_Id = h.id
	) select pid from subquery where hid is null;
	";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string presentationWithNoHome = dr.GetString(0);
                    Assert.Equal("", presentationWithNoHome);
                }
            }
        }

        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckOwnersAreAvailable()
        {

            var ownerIds = new List<string>();
            using (var conn = new NpgsqlConnection(Configuration.LocalConnectionString))
            {
                conn.Open();
                string query = $"select distinct array_to_string(owner_subject_ids,',') from public.home_entity where owner_subject_ids <> '{{}}'";
                var command = new NpgsqlCommand(query, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    ownerIds.Add(dr.GetString(0));
                }
            }

            var ownersThatDoNotExist = new List<string>();
            using (var conn = new NpgsqlConnection(Configuration.AuthConnectionString))
            {
                conn.Open();

                foreach (string ownerId in ownerIds)
                {
                    string query = $@"SELECT count(""SubjectId"") FROM public.""Users""	where ""SubjectId"" = '{ownerId}';";
                    using (var command = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int usersForCurrentHome = dr.GetInt16(0);
                                Assert.True(usersForCurrentHome > 0);//make sure her is a user for each home owner
                            }
                        }
                    }
                }
            }
        }



        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckAddressQueryFormat()
        {
            var character = new List<string>();
            string query = $@"with subquery as (select address_query, slug, id from public.home_entity where address_query like '%, , %')select count(1) from subquery;";
            int nofWrongAddresses = Utils.ReadIntFromDb(query);
            Assert.Equal(0, nofWrongAddresses);

        }
    }
}



