using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using TurkAzn.Data.Kimlik;
using TurkAzn.Web.Data;
using TurkAzn.Web.Hubs;
using TurkAzn.Data;
using TurkAzn.Web.Models;

namespace TurkAzn.Web
{
    public class Startup
    {
        private IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISOptions>(options => 
            {
                options.ForwardClientCertificate = false;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSignalR();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<Kullanici>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedEmail = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 5;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;

                    })
                .AddRoles<Rol>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options=>{
                options.LoginPath = "/Hesap/GirisYap";
                options.AccessDeniedPath = "/Hesap/GirisEngellendi";
                options.LogoutPath = "/hesap/cikisyap";
            });

            var webRoot = _env.WebRootPath;


            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(webRoot, "Resimler"))
            );

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShoppingCart.GetCart(sp)); 
            services.AddMemoryCache();
            services.AddSession();
            services.AddControllersWithViews();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Anasayfa/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/hata/{0}");
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute("default", "{controller=Anasayfa}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "areas", "areas", pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            //var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            ////_context.Database.EnsureCreated();

            //if (!_context.Roller.Any())
            //{
            //    Rol Admin = new Rol()
            //    {
            //        Rol_AD = "Admin"
            //    };
            //    Rol Kullanici = new Rol()
            //    {
            //        Rol_AD = "Kullanici"
            //    };
                

            //    _context.Roller.Add(Admin);
            //    _context.Roller.Add(Kullanici);
            //    _context.SaveChanges();
            //}
            //if (!_context.SiteAyari.Any())
            //{
            //    SiteAyari ayar = new SiteAyari()
            //    {
            //        IyzicoApiKey = "sandbox-VN0d99BY8moZfZ6TX8yCK6wuZG90T4j3",
            //        IyzicoApiSecret = "sandbox-6t0rcMXvVG2WNlICP0WFswMOg3O1E1Jd",
                   
            //        komisyon = 30,
            //        IyzicoBaseURL = "https://sandbox-api.iyzipay.com",
            //    };
            //    _context.SiteAyari.Add(ayar);
            //    _context.SaveChanges();
            //}
        }
    }
}
