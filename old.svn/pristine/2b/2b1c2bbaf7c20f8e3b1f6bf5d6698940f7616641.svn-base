using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using SmartTrapWebApp.Data;
using SmartTrapWebApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using SmartTrapWebApp.Services;
using SmartTrapWebApp.Areas.Identity;
using SmartTrapWebApp.Models.Configurations;
using SmartTrapWebApp.Configrations;
using SmartTrapWebApp.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.Validation;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.Services;
using IdentityServer4.Models;
using SmartTrapWebApp.Services.Tables;

namespace SmartTrapWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }
        class TokenReqeustValidator : ICustomTokenRequestValidator
        {
            public TokenReqeustValidator()
            {
            }
            public Task ValidateAsync(CustomTokenRequestValidationContext context)
            {
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim(ClaimTypes.Role, Helpers.Constants.Role.SystemAdmin));
                //if (context.Result.ValidatedRequest.Subject.IsInRole(Resources.RoleNameResource.SystemAdmistrator))
                //{
                //    context.Result.ValidatedRequest.ClientClaims.Add(new Claim(ClaimTypes.Role, Resources.RoleNameResource.SystemAdmistrator));
                //}
                return Task.FromResult(0);
            }
        }

        class ProfileService : IProfileService
        {
            public Task GetProfileDataAsync(ProfileDataRequestContext context)
            {

                //                context.IssuedClaims.Add(new Claim(ClaimTypes.Role, Resources.RoleNameResource.SystemAdmistrator));
                if (context.Subject.Identity.AuthenticationType == "Identity.Application")
                {
                    if (context.Subject.IsInRole(Helpers.Constants.Role.SystemAdmin))
                    {
                        context.IssuedClaims.Add(new Claim(Helpers.Constants.Role.ROLES, Helpers.Constants.Role.SystemAdmin));
                    }
                    else
                    {
                        context.IssuedClaims.Add(new Claim(Helpers.Constants.Role.ROLES, Helpers.Constants.Role.User));
                    }
                }
                else
                {
                    foreach(var c in context.Subject.Claims)
                    {
                        if(c.Type == Helpers.Constants.Role.ROLES)
                        {
                            switch (c.Value)
                            {
                                case Helpers.Constants.Role.SystemAdmin:
                                    context.IssuedClaims.Add(new Claim(Helpers.Constants.Role.ROLES, Helpers.Constants.Role.SystemAdmin));

                                    break;

                                default:
                                    context.IssuedClaims.Add(new Claim(Helpers.Constants.Role.ROLES, Helpers.Constants.Role.User));

                                    break;


                            }
                        }
                    }
                }
                return Task.FromResult(0);
            }

            public Task IsActiveAsync(IsActiveContext context)
            {
                return Task.FromResult(0);
            }
        }
        class AuthorizeValidator : ICustomAuthorizeRequestValidator
        {
            public Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
            {
//                context.Result.ValidatedRequest.ClientClaims.Add(new Claim(ClaimTypes.Role, Helpers.Constants.Role.SystemAdmin));
                return Task.FromResult(0);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityErrorDescriberJp>();

            services.AddIdentityServer(o=>
            {
            }).AddApiAuthorization<ApplicationUser, ApplicationDbContext>(o=>
            {
                //o.IdentityResources.AddProfile(o =>
                //{
                //    o.AllowAllClients();
                //});
                
            })
            .AddJwtBearerClientAuthentication()
            .AddCustomTokenRequestValidator<TokenReqeustValidator>()
            .AddProfileService<ProfileService>()
//            .AddCustomAuthorizeRequestValidator<AuthorizeValidator>()
            ;

            ////Roleベース／Claimベースの承認追加
            services.AddAuthorization(o =>
            {
                o.AddPolicy(Helpers.Constants.Role.SystemAdmin, p => p.RequireRole(Helpers.Constants.Role.SystemAdmin));
            });


            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
#if !DEBUG_WEB_API
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
#endif
            //Eメール送信サービスを定義
            services.AddTransient<IEmailSender, MmiEmailSender>();

            //Validationエラーメッセージローカライズ
            services.AddMvc(o =>
            {
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(msg => "{0}は空白にできません");
                o.ModelMetadataDetailsProviders.Add(
                        new ValidationMetadataProviderJp(
                            "SmartTrapWebApp.Resources.DefaultValidationResource",
                            typeof(DefaultValidationResource)));
            }).AddNewtonsoftJson();

            var apiKeys = this.Configuration.GetSection("ApiKeys");
            var secrets = this.Configuration.GetSection("Secrets");
            var constants = this.Configuration.GetSection("Constants");
            services.Configure<ApiKeys>(this.Configuration.GetSection("ApiKeys"));
            services.Configure<Secrets>(this.Configuration.GetSection("Secrets"));
            services.Configure<Constants>(this.Configuration.GetSection("Constants"));
            services.Configure<IdentityOptions>(options =>
            {
#if DEBUG
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
#else
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
#endif
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                //新規登録はEメールによる承認が必要
                options.SignIn.RequireConfirmedEmail = true;


            });
            var constantsInst = constants.Get<Constants>();
            var secretsInst = secrets.Get<Secrets>();

            services.AddSingleton(typeof(MemberTableClient), new MemberTableClient(constantsInst, secretsInst));
            services.AddSingleton(typeof(SensorTableClient), new SensorTableClient(constantsInst, secretsInst));
            services.AddSingleton(typeof(SensorMemberTableClient), new SensorMemberTableClient(constantsInst, secretsInst));

            services.AddSingleton<SoracomWebApiClient.Api.AuthApi>();
            services.AddSingleton<SoracomWebApiClient.Api.SigfoxDeviceApi>();

            ////Jwtトークンのカスタマイズ
            //services.Configure<JwtBearerOptions>(options =>
            //{
            //    var onTokenValidated = options.Events.OnTokenValidated;

            //    options.Events.OnTokenValidated = async ctx =>
            //    {
            //        await onTokenValidated(ctx);
            //    };
            //    options.SaveToken = true;

            //});


            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
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
#if !DEBUG_WEB_API
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
#endif

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
#if !DEBUG_WEB_API
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                
                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
#endif
            app.UseApiVersioning();
        }
    }
}
