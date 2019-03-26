using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oppari.Models;
using Microsoft.EntityFrameworkCore;
using Oppari.Controllers;
using Oppari.Hubs;
using Oppari.Logic;
using Microsoft.AspNetCore.SignalR;

namespace Oppari
{
    //public interface IChecker
    //{
    //    void Check();
    //}

    //public abstract class CheckerBase : IChecker
    //{
    //    protected string[] arguments;

    //    public CheckerBase(string[] argu)
    //    {
    //        arguments = argu;
    //    }


    //    public virtual void Check()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Check1 : CheckerBase
    //{
    //    public Check1(string[] argu) : base(argu)
    //    {
    //    }

    //    public override void Check()
    //    {

    //    }
    //}

    //public class Check2: CheckerBase
    //{
    //    public Check2(string[] argu) : base(argu)
    //    {
    //    }


    //    public override void Check()
    //    {
            
    //    }
    //}

    public class Startup
    {
        //public static List<IChecker> watchDogChecks = new List<IChecker>();
        public static List<Action> watchDogTests = new List<Action>();
        private readonly IHubContext<WatchDogHub> _hubContext;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            WatchDogHandler watchDogHandler = new WatchDogHandler(_hubContext);
            WatchDogChecks watchDogChecks = new WatchDogChecks();

            watchDogTests.Add(() => watchDogChecks.CheckOldFilesFromDirectory(@"C:\OppariUnitTests", ".txt"));
            watchDogTests.Add(() => watchDogChecks.CheckSqlQueries("SELECT * FROM dbo.WatchDogErrors"));

            //watchDogHandler.WatchDogTimer();

            //"Cherkers.EkaChecker", "arg1;arg2"
            //Chekers.ViisasCheker

            //foreach(var tietokantarivi in rivit)
            //    var instanssi = IOC.XXX(tietokantarivi.nimi, args)
            //    watchdogtests.add(instanssi)
            //    )

            //watchDogTests.Add(new Check1(new string[] { "aa" }));
            //watchDogTests.Add(new Check2(new string[] { "aa" }));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var connection = @"Server=(localdb)\mssqllocaldb;Database=WatchDog_Db;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<WatchDogErrorContext>
                (options => options.UseSqlServer(connection));

            services.AddSignalR();
            services.AddHostedService<WatchDogHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<WatchDogHub>("/watchDogHub");
            });
        }
    }
}
