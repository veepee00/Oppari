using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oppari.Models;

namespace Oppari.Controllers
{
    public class WatchDogController : Controller
    {
        public static bool watchDogRunning = true;
        public IActionResult Index()
        {
            int errorCount;
            using (var context = new WatchDogErrorContext())
            {
                errorCount = context.WatchDogErrors.Count();
            }
            ViewData["ErrorCount"] = errorCount;
            return View();
        }

        public static async Task WatchDogTimer()
        {
            while (watchDogRunning)
            {
                ExecuteWatchDogTests();
                await Task.Delay(10000);
            }
        }

        public IActionResult WatchDogErrors()
        {
            int errorCount;
            List<WatchDogErrorModel> errorList = new List<WatchDogErrorModel>();
            using (var context = new WatchDogErrorContext())
            {
                errorCount = context.WatchDogErrors.Count();
                errorList = context.WatchDogErrors.Where(e => e.Status == 50).ToList();
            }

            ViewData["ErrorCount"] = errorCount;
            ViewData["ErrorList"] = errorList;
            return View();
        }

        public static void ExecuteWatchDogTests()
        {
            try
            {
                if (Startup.watchDogTests.Count == 0)
                {
                    watchDogRunning = false;
                    throw new Exception("Suoritettavia testejä ei löytynyt!");                    
                }
                foreach (var method in Startup.watchDogTests)
                {
                    method.Invoke();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }

        public static void CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
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
                //jippii
            }
            else
            {
                WatchDogErrorModel wdError = new WatchDogErrorModel("CheckOldFilesFromDirectory", $"Method returned 0 files.", 50, DateTime.Now, folder, mask);
                AddWatchDogErrorToDb(wdError);
            }
        }

        public static void CheckSqlQueries(string query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }

            using (var context = new WatchDogErrorContext())
            {
                var builds = context.WatchDogErrors.FromSql(query).ToList();
                if (builds.Count() == 10)
                {
                    //jippii
                }
                else
                {
                    WatchDogErrorModel wdError = new WatchDogErrorModel("CheckSqlQueries", $"Method returned 0 rows.", 50,DateTime.Now, query);
                    AddWatchDogErrorToDb(wdError);
                }
            }
        }

        public static void AddWatchDogErrorToDb(WatchDogErrorModel wdError)
        {
            if (wdError == null)
            {
                throw new ArgumentNullException();
            }

            using (var context = new WatchDogErrorContext())
            {
                context.WatchDogErrors.Add(wdError);
                context.SaveChanges();
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
            var optionsBuilder = new DbContextOptionsBuilder<WatchDogErrorContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=WatchDog_Db;Trusted_Connection=True;ConnectRetryCount=0");
            using (var context = new WatchDogErrorContext(optionsBuilder.Options))
            {
                var builds = context.WatchDogErrors.FromSql(query).ToList();

                return $"Found {builds.Count().ToString()} matches with query: {query}.";
            }
        }
    }
}