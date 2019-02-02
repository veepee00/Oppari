using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var files = CheckOldFilesFromDirectory(@"C:\OppariUnitTests", ".txt");

            Assert.IsTrue(files.Count == 3);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //Method must return ArgumentNullException if any parameter is empty or null
            Assert.ThrowsException<ArgumentNullException>(() => CheckOldFilesFromDirectory(@"C:\Temp", null));
            Assert.ThrowsException<ArgumentNullException>(() => CheckOldFilesFromDirectory(null, "*."));
            Assert.ThrowsException<ArgumentNullException>(() => CheckOldFilesFromDirectory(@"C:\Temp", ""));
            Assert.ThrowsException<ArgumentNullException>(() => CheckOldFilesFromDirectory("", "*."));
        }

        private List<string> CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
        {
            if (folder == "" || folder == null || mask == "" || mask == null)
            {
                throw new ArgumentNullException();
            }

            List<string> files = new List<string>(Directory.GetFiles(folder));
            List<string> returnFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((fi.LastAccessTime < DateTime.Now.AddMinutes(time)) && fi.Extension == mask)
                {
                    Console.WriteLine(fi.Extension);
                    returnFiles.Add(file);
                }
            }
            return returnFiles;
        }
    }
}
