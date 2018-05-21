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
    public class DataTests
    {
        const string scriptCreateSnapshot = "CREATE DATABASE [__snapshot__] ON (NAME = __dbName__, FILENAME = '__fileLocation__\\__snapshotFileName__') AS SNAPSHOT OF [__dbName__]; ";
        const string scriptDropSnapshot = "DROP DATABASE [__snapshot__];";
        const string scriptRestoreFromSnapshot = "USE [master];" +

            "DECLARE @kill varchar(8000) = '';" +
            "SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'" +
            "FROM sys.dm_exec_sessions " +
            "WHERE database_id = db_id('__dbName__') " +

            "EXEC(@kill);" +

            "RESTORE DATABASE __dbName__ " +
            "FROM DATABASE_SNAPSHOT = '__snapshot__'";

        private static string ReplaceTokens(string script)
        {
            string snapshotName = ConfigurationManager.AppSettings["TestSnapshotName"],
            snapshotFileName = ConfigurationManager.AppSettings["TestSnapshotFileName"],
            dbName = ConfigurationManager.AppSettings["TestDatabaseName"];
            string tokenizedScript = script.Replace("__fileLocation__", Environment.CurrentDirectory)
                .Replace("__snapshotFileName__", snapshotFileName)
                .Replace("__snapshot__", snapshotName)
                .Replace("__dbName__", dbName);

            return tokenizedScript;
        }
        private static void DropDatabase()
        {            
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                try
                {
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, ReplaceTokens(scriptDropSnapshot));
                }
                catch (Exception)
                {
                    //log exception
                }
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Debug.WriteLine("Class Initialize");
            //TODO: pull all const strings from files

            //D:\Source\Sandbox\ApiSample\DataTests\Snapshots\ApiSample_SS.ss
            //string snapshotFileName = ConfigurationManager.AppSettings["TestSnapshotFileName"];
            //string createSnapshot = scriptCreateSnapshot.Replace("__fileLocation__", Environment.CurrentDirectory).Replace("__snapshotName__", snapshotFileName);
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                try
                {
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, ReplaceTokens(scriptCreateSnapshot));
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    DropDatabase();
                    //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, ReplaceTokens(scriptCreateSnapshot));
                }
            }
        }
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");


            using (ApiSampleDbContext context = new ApiSampleDbContext("MasterConnection"))
            {
                try
                {
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, ReplaceTokens(scriptRestoreFromSnapshot));
                }
                catch (Exception)
                {
                    DropDatabase();
                    throw;
                }
            }
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Debug.WriteLine("Test Cleanup");

        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Debug.WriteLine("Class Cleanup");

            DropDatabase();
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
