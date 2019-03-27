using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Oppari.Hubs;
using Oppari.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Logic
{
    public class WatchDogChecks
    {
        IHubContext<WatchDogHub,IWatchDog> _hubContext;
        public WatchDogChecks(IHubContext<WatchDogHub, IWatchDog> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
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
                WatchDogHandler watchDogHandler = new WatchDogHandler(_hubContext);
                await watchDogHandler.AddWatchDogErrorToDb(wdError);
            }
        }

        public async Task CheckSqlQueries(string query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }

            using (var context = new WatchDogErrorContext())
            {
                var builds = context.WatchDogErrors.FromSql(query).ToList();
                WatchDogErrorModel wdError = new WatchDogErrorModel("CheckSqlQueries", $"Method returned 0 rows.", 50, DateTime.Now, query);
                WatchDogHandler watchDogHandler = new WatchDogHandler(_hubContext);
                await watchDogHandler.AddWatchDogErrorToDb(wdError);
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
