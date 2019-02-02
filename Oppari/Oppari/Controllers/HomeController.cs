using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oppari.Models;
using Microsoft.EntityFrameworkCore;

namespace Oppari.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ExecuteFileTests()
        {

        }

        public void ExecuteSqlQueryTests()
        {

        }

        public string CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
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

        public string CheckSqlQueries()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ComputerBuildingContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ComputerBuilding_Db;Trusted_Connection=True;ConnectRetryCount=0");
            using (var context = new ComputerBuildingContext(optionsBuilder.Options))
            {
                var builds = context.Builds.FromSql("SELECT * FROM dbo.Builds").ToList();
                if (builds.Count() > 0)
                {
                    return "Tietokannasta löytyi " + builds.Count().ToString() + " tietokonetta.";
                }
                else
                {
                    return "Tietokannasta ei löytynyt yhtään tietokonetta!";
                }
            }          
        }
    }
}
