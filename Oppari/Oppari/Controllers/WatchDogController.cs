using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oppari.Models;

namespace Oppari.Controllers
{
    public class WatchDogController : Controller
    {
        private static System.Timers.Timer timer;
        private static List<string> errorList = new List<string>();
        private static int errorCount;
        private static bool watchDogRunning;
        public IActionResult Index()
        {
            if (!watchDogRunning)
            {
                watchDogRunning = true;
                StartWatchDog();
            }
            ViewData["ErrorCount"] = errorCount;
            return View();
        }

        public void StartWatchDog()
        {
            timer = new System.Timers.Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(ExecuteWatchDogTests);
            timer.Interval = 10000;
            timer.Enabled = true;
        }

        public IActionResult WatchDogErrors()
        {
            ViewData["ErrorCount"] = errorCount;
            ViewData["ErrorList"] = errorList;
            return View();
        }

        public static void ExecuteWatchDogTests(object source, ElapsedEventArgs e)
        {        
            timer.Enabled = false;
            try
            {
                errorList.Add(CheckOldFilesFromDirectory(@"C:\OppariUnitTests", ".txt"));
                errorList.Add(CheckSqlQueries("SELECT * FROM dbo.Builds"));      
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                errorList.RemoveAll(item => String.IsNullOrEmpty(item));
                errorCount = errorList.Count();
                timer.Enabled = true;
            }
        }

        public static string CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
        {
            //Montako tiedostoa löytyy, joihin ei ole koskettu {time} minuutin sisällä
            if (String.IsNullOrEmpty(folder) || String.IsNullOrEmpty(mask))
            {
                throw new ArgumentNullException();
            }

            List<string> files = new List<string>(Directory.GetFiles(folder));
            List<string> returnFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((fi.LastWriteTime < DateTime.Now.AddMinutes(time)) && fi.Extension == mask)
                {
                    Console.WriteLine(fi.Extension);
                    returnFiles.Add(file);
                }
            }
            if (returnFiles.Count() > 0)
            {
                return null;
            }
            else
            {
                return $"CheckOldFilesFromDirectory failed with folder: {folder} and mask: {mask}";
            }
        }

        public static string CheckSqlQueries(string query)
        {
            //Montako osumaa löytyy haulla sql-kyselyllä {query}
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }
            var optionsBuilder = new DbContextOptionsBuilder<ComputerBuildingContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ComputerBuilding_Db;Trusted_Connection=True;ConnectRetryCount=0");
            using (var context = new ComputerBuildingContext(optionsBuilder.Options))
            {
                var builds = context.Builds.FromSql("SELECT * FROM dbo.Builds").ToList();
                if (builds.Count() > 0)
                {
                    return null;
                }
                else
                {
                    return $"CheckSqlQueries failed with query: {query}.";
                }
            }
        }
        public string DebugCheckOldFilesFromDirectory(string folder, string mask, int time = -10) 
        {
            //Montako tiedostoa löytyy, joihin ei ole koskettu {time} minuutin sisällä
            if (String.IsNullOrEmpty(folder) || String.IsNullOrEmpty(mask))
            {
                throw new ArgumentNullException();
            }

            List<string> files = new List<string>(Directory.GetFiles(folder));
            List<string> returnFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((fi.LastWriteTime < DateTime.Now.AddMinutes(time)) && fi.Extension == mask)
                {
                    Console.WriteLine(fi.Extension);
                    returnFiles.Add(file);
                }
            }

            if (returnFiles.Count() > 0)
            {
                time *= -1;
                return "Tiedostojen lukumäärä, joihin ei ole koskettu " + time.ToString() + " minuutin sisällä: " + returnFiles.Count().ToString();
            }
            else
            {
                return "Tiedostojen lukumäärä, joihin ei ole koskettu " + time.ToString() + " minuutin sisällä ei löytynyt!";
            }
        }

        public string DebugCheckSqlQueries(string query)
        {
            //Montako osumaa löytyy haulla sql-kyselyllä {query}
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }
            var optionsBuilder = new DbContextOptionsBuilder<ComputerBuildingContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ComputerBuilding_Db;Trusted_Connection=True;ConnectRetryCount=0");
            using (var context = new ComputerBuildingContext(optionsBuilder.Options))
            {
                var builds = context.Builds.FromSql("SELECT * FROM dbo.Builds").ToList();

                return $"Found {builds.Count().ToString()} matches with query: {query}.";
            }
        }
    }
}