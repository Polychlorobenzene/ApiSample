using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

using ApiSample.Data.Repositories;
using ApiSample.Data.Entities;
using ApiSample.Data;
using System.Diagnostics;
using System.Data.Entity;

namespace DataTests
{
    [TestClass]
    public class DataTests : UnitTestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase.ClassInitialize(testContext);
        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase.ClassCleanup();
        }

        [TestMethod]
        public void DeleteAppPeopleTest()
        {
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                var people = context.Persons;
                context.Persons.RemoveRange(people);
                context.SaveChanges();
            }
            //totally new connection
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                var people = context.Persons.ToList();
                Assert.AreEqual(people.Count, 0, "There shouldn't be any people.");

            }
        }

        [TestMethod]
        public void TestGetPeopleWithDefaults()
        {
            using (ApiSampleRepository repository = new ApiSampleRepository())
            {
                PagedSearchDto dto = new PagedSearchDto();
                dto.PageSize = 25;
                dto.PageNumber = 2;
                dto.OrderByColumn = "PersonId";
                dto.OrderAscending = true;
                dto.TotalRows = 0;
                PagedSearchResponseDto<List<PersonSearchResultDto>> response = repository.SearchPeople(dto);
                Assert.IsTrue(response.Result.Count == 25);
                Assert.IsTrue(response.Result.First().PersonId == 26);
            }
        }
        [TestMethod]
        public void TestGetPeople()
        {
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                var people = context.Persons.Take(25);
                Assert.IsTrue(people.Count() == 25);
            }
        }
    }
}
