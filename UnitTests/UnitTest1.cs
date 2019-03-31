using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Oppari.Controllers;
using Oppari.Logic;
using Microsoft.AspNetCore.SignalR;
using Oppari.Hubs;
using System.Threading.Tasks;

namespace UnitTests
{    
    [TestClass]
    public class UnitTest1
    {
        WatchDogChecks wdChecks = new WatchDogChecks(null);
        WatchDogHandler wdHandler = new WatchDogHandler(null);
        [TestMethod]
        public async Task FileTestMethod1()
        {          
            //Method must return ArgumentNullException if either parameter is empty or null
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async() => await wdChecks.CheckOldFilesFromDirectory(@"C:\Temp", null));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdChecks.CheckOldFilesFromDirectory(null, "*."));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdChecks.CheckOldFilesFromDirectory(@"C:\Temp", ""));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdChecks.CheckOldFilesFromDirectory("", "*."));
        }

        [TestMethod]
        public async Task SqlQueryTestMethod1()
        {
            //Method must return ArgumentNullException if parameter is empty or null
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdChecks.CheckSqlQueries(""));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdChecks.CheckSqlQueries(null));
        }

        [TestMethod]
        public async Task NullAOrEmptyTestMethod()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await wdHandler.AddWatchDogErrorToDb(null));
        }

    }
}