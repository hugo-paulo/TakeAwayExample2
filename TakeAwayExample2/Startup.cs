using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TakeAwayExample2.DataAccess;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.DataAccess.Repository;
using System.Globalization;
using Microsoft.AspNetCore.Identity.UI.Services;
using TakeAwayExample2.Utility;
using Microsoft.EntityFrameworkCore.Internal;
using System.Configuration;
using Stripe;

namespace TakeAwayExample2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Context for identity, so EF only create the indenty tables.
            //We using same connection string because want to add these tables to same DB
            services.AddDbContext<LoginDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("TakeAwayDBConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LoginDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("TakeAwayDBConnection")));

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<IDbSetInitializer, DbSetInitializer>();

            //can also add a file upload limit here

            //We add session so that we can keep the shopping cart items in memory
            //Adding Sessions to the project
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddRazorPagesOptions(options => {
                    options.Conventions.AddPageRoute("/Customer/Home/Index", ""); //To make custom home page need to add this and delete the default page
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //Dont need this default with the above routing
            //services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*This is to make the price input work correctly with decimal inputs*/
            var cultureInfo = new CultureInfo("en-US"); //For RSA ("en-ZA")
            cultureInfo.NumberFormat.NumberGroupSeparator = "."; //can also use ","

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //call the session method that we set up
            app.UseSession();

            //Dont need this default when using mvc?
            //app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            //Dont need this default with the above
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
        }
    }
}
