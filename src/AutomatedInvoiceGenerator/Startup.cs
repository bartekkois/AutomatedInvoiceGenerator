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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using Serilog.Filters;
using AutomatedInvoiceGenerator.Controllers.API;

namespace AutomatedInvoiceGenerator
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .Filter.ByIncludingOnly(s => (Matching.FromSource<GenerateInvoiceService>()(s) || Matching.FromSource<InvoicesApiController>()(s)) || Matching.FromSource<ExportService>()(s))
                .MinimumLevel.Information()
                .WriteTo.RollingFile("Logs/Log-{Date}.txt", fileSizeLimitBytes: 1024 * 1024 * 100)
                .CreateLogger();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
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

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                })
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

            services.AddTransient<IGenerateInvoiceService, GenerateInvoiceService>();
            services.AddTransient<IExportService, ExportService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserResolverService, UserResolverService>();
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
            loggerFactory.AddSerilog();

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
                config.CreateMap<Customer, CustomerDto>().ReverseMap().ForMember(dest => dest.ServiceItemsSets, opt => opt.Ignore());
                config.CreateMap<ServiceItemsSet, ServiceItemsSetDto>().ReverseMap().ForMember(dest => dest.OneTimeServiceItems, opt => opt.Ignore()).ForMember(dest => dest.SubscriptionServiceItems, opt => opt.Ignore());
                config.CreateMap<OneTimeServiceItem, OneTimeServiceItemDto>().ReverseMap().ForMember(dest => dest.InvoiceItems, opt => opt.Ignore());
                config.CreateMap<SubscriptionServiceItem, SubscriptionServiceItemDto>().ReverseMap().ForMember(dest => dest.InvoiceItems, opt => opt.Ignore());
                config.CreateMap<Invoice, InvoiceDto>().ReverseMap();
                config.CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
            });

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });

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
