using AbsolCase.Configuration;
using AbsolCase.Configurations;
using AbsolCase.Models;
using AbsolCase.Utility;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using System;
using System.IO;

namespace AbsolCase
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR(conf => {conf.MaximumReceiveMessageSize = null;});
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =MicrosoftAccountDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddMicrosoftAccount(o =>
            {
            o.ClientId = Configuration
            ["Authentication:ClientId"];
            o.ClientSecret = Configuration
            ["Authentication:ClientSecret"];
            });
            services.Configure<MyConfiguration>(Configuration.GetSection("App"));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time   
            });
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddScoped<ConfigureSession>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor, IConfiguration configuration)
        {
            //RequestSender.SetHttpContextAccessor(accessor);
            RequestSender.SetIHttpContextAccessor(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(), app.ApplicationServices.GetRequiredService<IConfiguration>());
            MessageHub.SetHttpContextAccessor(accessor, configuration);
            app.UseSession();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseDeveloperExceptionPage();
            app.UseExceptionHandler("/Error");
            app.UseStaticFiles();
          
            app.UseRouting();

            app.UseAuthorization();
            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/messageHub");
                endpoints.MapControllerRoute("default", "{area=public}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("controllers", "{controller=common}/{action=Index}/{id?}");
            });
            RotativaConfiguration.Setup((IHostingEnvironment)env);
        }
    }
}
