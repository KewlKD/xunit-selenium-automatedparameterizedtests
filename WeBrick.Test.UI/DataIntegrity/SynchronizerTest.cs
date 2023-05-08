using Npgsql;
using Xunit;

namespace WeBrick.Test.UI.ApiCall
{

    [Collection("Sequential")]
    public class SynchronizerTest
    {

        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckBrokerLinkIsLastLinkOfImportListing()
        {
            string sql = @"WITH listingQuery as(
	select il1.id, il1.url, il1.udbudssag from import_listing il1 where il1.id::int in(
	select max(il0.id::int) as id from  home_entity h0 , import_relation r0 ,import_offering o0, import_listing il0
			where 
			(1=1)
			and h0.id = r0.home_id 
			and Lower(h0.sale_status)= 'active'
			and r0.offering_id = o0.id
			and o0.is_open = 1
			and il0.udbudssag = o0.id::int
	group by il0.udbudssag
		)

)
SELECT   count(1) from  home_entity h2 , import_relation r2 ,import_offering o2, listingQuery
			where 
			(1=1)
			and h2.id = r2.home_id 
			and Lower(h2.sale_status)= 'active'
			and r2.offering_id = o2.id
			and o2.is_open = 1
			and listingQuery.udbudssag = o2.id::int
			And listingQuery.url <> h2.broker_link";
            int numberOfUrlMismach = Utils.ReadIntFromDb(sql);
            Assert.Equal(0, numberOfUrlMismach);
        }


        [Fact]
        [Trait("Category", "DataIntegrity")]
        public static void CheckSaleStatusIsActiveWhenIsOpenEqualsOne()
        {
            var sql = $@"
select Count(il0.id)  from  home_entity h0 , import_relation r0 ,import_offering o0, import_listing il0
			where 
			(1=1)
			and h0.id = r0.home_id 
			and r0.offering_id = o0.id
			and o0.is_open = 1
			and Lower(h0.sale_status) <> 'active' 
			and il0.udbudssag = o0.id::int";
            int numberOfHomesWithWrongSaleStatus = Utils.ReadIntFromDb(sql);
            Assert.Equal(0, numberOfHomesWithWrongSaleStatus);

            sql = $@"select Count(1)  from  home_entity h0 , import_relation r0 ,import_offering o0
			where 
			(1=1)
			and h0.id = r0.home_id 
			and r0.offering_id = o0.id
			and o0.is_open = 0
			and Lower(h0.sale_status) = 'active'
            and h0.owner_subject_ids = '{{}}'";
            numberOfHomesWithWrongSaleStatus = Utils.ReadIntFromDb(sql);
            Assert.Equal(0, numberOfHomesWithWrongSaleStatus);


        }
    }
}




