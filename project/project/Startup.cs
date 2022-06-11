using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using project.CustomTokenProviders;

namespace project
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
            services.AddDbContext<HelperlandContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Myconnection")));
            //services.AddDbContext<HelperLanddContext>(optionsBuilder =>
            // optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Myconnection"), options => options.EnableRetryOnFailure()));

            //services.AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
            //{
            //    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(30);
            //    // This allows the username to have valid email characters.
            //    // See: https://en.wikipedia.org/wiki/Email_address
            //    options.User.AllowedUserNameCharacters = String.Empty;
            //})
            //    .AddEntityFrameworkStores<ApplicationDatasource, long>()
            //    .AddDefaultTokenProviders();
            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = true; // checks if the mail is confirmed  or not
                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            })              
              .AddEntityFrameworkStores<HelperlandContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

            //services.AddDefaultIdentity<User>(options =>
            //  options.SignIn.RequireConfirmedAccount = true)
            //    .AddRoles<IdentityRole>() //Line that can help you
            //    .AddEntityFrameworkStores<HelperlandContext>();
            

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromMinutes(1));
            services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromDays(3));
            //===================== Instead I copied this options portion above in IdentityRole Section ===================
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 7;
            //    options.Password.RequireNonAlphanumeric = false;
            //});

            //  *******************************  for Cookies ************************************* 
            // services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.ConsentCookie.IsEssential = true;//<--NOTE THIS
            //    options.CheckConsentNeeded = context => false;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.IsEssential = true;//<--NOTE THIS
            //        options.Cookie.HttpOnly = true;
            //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //        options.Cookie.SameSite = SameSiteMode.None;
            //        options.LoginPath = "/Starting/Index";
            //        //options.LogoutPath = "/Account/Login";
            //    });
            services.AddMvc().AddXmlDataContractSerializerFormatters();
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
            }
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Starting}/{action=Index}/{id?}");
            });
        }
    }
}
