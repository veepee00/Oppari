using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Oppari.Controllers;



namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FileTestMethod1()
        {          
            //Method must return ArgumentNullException if either parameter is empty or null
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckOldFilesFromDirectory(@"C:\Temp", null));
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckOldFilesFromDirectory(null, "*."));
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckOldFilesFromDirectory(@"C:\Temp", ""));
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckOldFilesFromDirectory("", "*."));
        }

        [TestMethod]
        public void SqlQueryTestMethod1()
        {
            //Method must return ArgumentNullException if parameter is empty or null
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckSqlQueries(""));
            Assert.ThrowsException<ArgumentNullException>(() => WatchDogController.CheckSqlQueries(null));
        }

    }
}