using E_Commerce_Movies.Data;
using E_Commerce_Movies.Data.Cart;
using E_Commerce_Movies.Data.Services;
using E_Commerce_Movies.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies
{
    public class Startup
    {
        public Startup(IConfiguration configuration , IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //DbContext configuration
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Services configuration
            services.AddScoped<IActorsService, ActorsService>();
            services.AddScoped<IProducersService, ProducersService>();
            services.AddScoped<ICinemasService, CinemasService>();
            services.AddScoped<IMoviesService, MoviesService>();
            services.AddScoped<IOrdersService, OrdersService>();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //to be used from Non-Controller class
            services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));   //this means to different people at the sameTime asking for ShoppingChart object are going to get different instances

            //Authentication and authorization
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddMemoryCache();  // Apps running on a server farm (multiple servers) should ensure sessions are sticky when using the in-memory cache
                                        //Sticky sessions ensure that requests from a client all go to the same server. For example, Azure Web apps use Application Request Routing (ARR) to route all requests to the same server.
            services.AddSession();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;        //To Apply Authorization for sepecific controllers
     
            });

            var builder = services.AddControllersWithViews();

            if (Env.IsDevelopment())
            {

                builder.AddRazorRuntimeCompilation();
            }


          

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
                
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseSession();


            //Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=movies}/{action=Index}/{id?}");
            });

            //Seed database
            //because i want to use this service every time my program starts i'm using injected service this way 


            AppDbInitializer.Seed(app);
            AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();
        }


       
    }
}
