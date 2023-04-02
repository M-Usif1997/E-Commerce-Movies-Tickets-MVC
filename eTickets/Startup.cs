using E_Commerce_Movies.Data;
using E_Commerce_Movies.Data.Cart;
using E_Commerce_Movies.Data.Repositories;
using E_Commerce_Movies.Data.Services;
using E_Commerce_Movies.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }



        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //DbContext configuration
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Services configuration
            services.AddScoped<IActorsRepo,ActorsRepo>();
            services.AddScoped<IProducersRepo, ProducersRepo>();
            services.AddScoped<ICinemasRepo, CinemasRepo>();
            services.AddScoped<IMoviesRepo, MoviesRepo>();
            services.AddScoped<IOrdersRepo, OrdersRepo>();


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

             services.AddControllersWithViews();



          

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
