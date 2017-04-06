using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Models;
using AutomatedInvoiceGenerator.Services;
using AutomatedInvoiceGenerator.Models.SampleData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.IO;
using AutoMapper;
using AutomatedInvoiceGenerator.DTO;

namespace AutomatedInvoiceGenerator
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(90);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                options.User.RequireUniqueEmail = true;
            });

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            });

            // Add application services.
            services.AddAutoMapper();

            services.AddTransient<IExportService, ExportService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var supportedCultures = new[]
{
                      new CultureInfo("en-US"),
                      new CultureInfo("en-GB"),
                      new CultureInfo("en"),
                      new CultureInfo("pl-PL"),
                      new CultureInfo("pl")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                    var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

                    context.Database.Migrate();
                    context.EnsureSeedData().GetAwaiter().GetResult();
                    roleManager.EnsureSeedRoles().GetAwaiter().GetResult();
                    userManager.EnsureSeedAdministrators().GetAwaiter().GetResult();
                    userManager.EnsureSeedUsers().GetAwaiter().GetResult();
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                    var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

                    context.Database.Migrate();
                    roleManager.EnsureSeedRoles().GetAwaiter().GetResult();
                    userManager.EnsureSeedAdministrators().GetAwaiter().GetResult();
                }
            }

            Mapper.Initialize(config =>
            {
                config.CreateMap<Group, GroupDto>().ReverseMap();
                config.CreateMap<Customer, CustomerDto>().ReverseMap();
            });

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Customers}/{id?}");
            });
        }
    }
}
