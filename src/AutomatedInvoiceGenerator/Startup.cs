using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Models;
using AutomatedInvoiceGenerator.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using Serilog.Filters;
using AutomatedInvoiceGenerator.Controllers.API;
using AutomatedInvoiceGenerator.Models.SampleData;
using AutomatedInvoiceGenerator.Controllers;

namespace AutomatedInvoiceGenerator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(c => c
                    .Filter.ByIncludingOnly(s => (Matching.FromSource<GenerateInvoiceService>()(s) || Matching.FromSource<InvoicesApiController>()(s)))
                    .MinimumLevel.Information()
                    .WriteTo.RollingFile("Logs/GenerateInvoices/log-{Date}.txt", fileSizeLimitBytes: 1024 * 1024 * 100, shared: true))
                .WriteTo.Logger(c => c
                    .Filter.ByIncludingOnly(s => (Matching.FromSource<ExportService>()(s)) || Matching.FromSource <ExportController>()(s))
                    .MinimumLevel.Information()
                    .WriteTo.RollingFile("Logs/ExportInvoices/log-{Date}.txt", fileSizeLimitBytes: 1024 * 1024 * 100, shared: true))
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(90);
                    options.LoginPath = "/Account/LogIn";
                    options.LogoutPath = "/Account/LogOff";
                    options.Events = new CookieAuthenticationEvents
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
                    };
                }
                );

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
            });

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            });

            // Add application services.
            services.AddAutoMapper(typeof(AutoMapperProfile));

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

                using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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

                using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    context.Database.Migrate();
                    roleManager.EnsureSeedRoles().GetAwaiter().GetResult();
                    userManager.EnsureSeedAdministrators().GetAwaiter().GetResult();
                }
            }

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
            app.UseAuthentication();

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
