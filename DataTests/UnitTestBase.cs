using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Entity;
using System.Configuration;
using System.IO;
using ApiSample.Data;

namespace DataTests
{
    public abstract class UnitTestBase
    {
        //const string scriptCreateSnapshot = "CREATE DATABASE [__snapshot__] ON (NAME = __dbName__, FILENAME = '__fileLocation__\\__snapshotFileName__') AS SNAPSHOT OF [__dbName__]; ";
        //const string scriptDropSnapshot = "DROP DATABASE [__snapshot__];";
        //const string scriptRestoreFromSnapshot = "USE [master];" +

        //    "DECLARE @kill varchar(8000) = '';" +
        //    "SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'" +
        //    "FROM sys.dm_exec_sessions " +
        //    "WHERE database_id = db_id('__dbName__') " +

        //    "EXEC(@kill);" +

        //    "RESTORE DATABASE __dbName__ " +
        //    "FROM DATABASE_SNAPSHOT = '__snapshot__'";

        static string _scriptCreateSnapshot = "";
        static string _scriptDropSnapshot = "";
        static string _scriptRestoreFromSnapshot = "";

        private static string GetFileContents(string fileName)
        {
            string path = string.Format("{0}\\DbScripts\\{1}", Environment.CurrentDirectory, fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }
        private static string Script_CreateSnapshot
        {
            get
            {
                if (!string.IsNullOrEmpty(_scriptCreateSnapshot))
                {
                    return _scriptCreateSnapshot;
                }
                else
                {
                    string script = ReplaceTokens(GetFileContents("CreateSnapshot.sql"));
                    _scriptCreateSnapshot = script;
                    return _scriptCreateSnapshot;
                }
            }
        }
        private static string Script_DropSnapshot
        {
            get
            {
                if (!string.IsNullOrEmpty(_scriptDropSnapshot))
                {
                    return _scriptDropSnapshot;
                }
                else
                {
                    string script = ReplaceTokens(GetFileContents("DropSnapshot.sql"));
                    _scriptDropSnapshot = script;
                    return _scriptDropSnapshot;
                }
            }
        }
        private static string Script_RestoreFromSnapshot
        {
            get
            {
                if (!string.IsNullOrEmpty(_scriptRestoreFromSnapshot))
                {
                    return _scriptRestoreFromSnapshot;
                }
                else
                {
                    string script = ReplaceTokens(GetFileContents("RestoreFromSnapshot.sql"));
                    _scriptRestoreFromSnapshot = script;
                    return _scriptRestoreFromSnapshot;
                }
            }
        }

        private static string ReplaceTokens(string script)
        {
            string snapshotName = ConfigurationManager.AppSettings["TestSnapshotName"],
            snapshotFileName = ConfigurationManager.AppSettings["TestSnapshotFileName"],
            dbName = ConfigurationManager.AppSettings["TestDatabaseName"];
            string tokenizedScript = script.Replace("__fileLocation__", Environment.CurrentDirectory + "\\DbScripts")
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
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, Script_DropSnapshot);
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
            int tryCounter = 0;
            using (ApiSampleDbContext context = new ApiSampleDbContext())
            {
                try
                {
                    tryCounter++;
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, Script_CreateSnapshot);
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    DropDatabase();
                    if (tryCounter <= 2)
                        context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, Script_CreateSnapshot);
                    else throw;
                }
            }
        }
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");
            //using (ApiSampleDbContext context = new ApiSampleDbContext("MasterConnection"))
            //{
            //    try
            //    {
            //        context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, Script_RestoreFromSnapshot);
            //    }
            //    catch (Exception)
            //    {
            //        DropDatabase();
            //        throw;
            //    }
            //}
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Debug.WriteLine("Test Cleanup");

            using (ApiSampleDbContext context = new ApiSampleDbContext("MasterConnection"))
            {
                try
                {
                    context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, Script_RestoreFromSnapshot);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Debug.WriteLine("Class Cleanup");

            DropDatabase();
        }
    }

}
