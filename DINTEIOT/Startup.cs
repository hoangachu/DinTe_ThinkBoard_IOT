using DINTEIOT.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DINTEIOT
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static string connectionString = "";
        public static string ConnectionStringsThinkBoard = "";
        public static string usernametb = "";
        public static string passwordtb = "";
        public static string thinkportaccesstoken { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("DefaultConnection").ToString();
            ConnectionStringsThinkBoard = Configuration.GetValue<string>("ConnectionStringsThinkBoard");
            usernametb = Configuration.GetValue<string>("usernametb");
            passwordtb = Configuration.GetValue<string>("passwordtb");
         
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStationDataController, StationDataController>();
            services.AddScoped<IOrganController,OrganController>();
            services.AddScoped<IMonitorStationController, MonitorStationController>();
            services.AddScoped<IMonitorDatabaseController, MonitorDatabaseController>();
            services.AddScoped<IWarningMarginController, WarningMarginController>();
            services.AddControllersWithViews();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/account/login";
                     options.Cookie.Name = "ac_session";
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=index}/{id?}");
            });
        }
    }
}
