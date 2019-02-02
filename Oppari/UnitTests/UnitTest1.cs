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
            HomeController homeController = new HomeController();

            //Method must return ArgumentNullException if either parameter is empty or null
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckOldFilesFromDirectory(@"C:\Temp", null));
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckOldFilesFromDirectory(null, "*."));
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckOldFilesFromDirectory(@"C:\Temp", ""));
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckOldFilesFromDirectory("", "*."));
        }

        [TestMethod]
        public void SqlQueryTestMethod1()
        {
            HomeController homeController = new HomeController();
            //Method must return ArgumentNullException if parameter is empty or null
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckSqlQueries(""));
            Assert.ThrowsException<ArgumentNullException>(() => homeController.CheckSqlQueries(null));
        }

    }
}