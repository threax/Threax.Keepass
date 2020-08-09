using Threax.Keepass.Controllers;
using Threax.Keepass.Controllers.Api;
using Threax.Keepass.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.BuiltInTools;
using Threax.AspNetCore.CorsManager;
using Threax.AspNetCore.Halcyon.ClientGen;
using Threax.AspNetCore.Halcyon.Ext;
using Threax.AspNetCore.IdServerAuth;
using Threax.AspNetCore.UserBuilder;
using Threax.AspNetCore.UserLookup.Mvc.Controllers;
using Threax.Extensions.Configuration.SchemaBinder;
using Threax.Sqlite.Ext.EfCore3;
using Microsoft.Extensions.Hosting;
using Threax.Keepass.Services;
using KeePassLib.Interfaces;

namespace Threax.Keepass
{
    public class Startup
    {
        //Replace the following values with your own values
        private IdServerAuthAppOptions authConfig = new IdServerAuthAppOptions()
        {
            Scope = "Threax.Keepass", //The name of the scope for api access
            DisplayName = "Threax.Keepass", //Change this to a pretty name for the client/resource
            ClientId = "Threax.Keepass", //Change this to a unique client id
            AdditionalScopes = new List<String> { /*Additional scopes here "ScopeName", "Scope2Name", "etc"*/ },
            ClientCredentialsScopes = new List<string> { "Threax.IdServer" }
        };
        //End user replace

        private AppConfig appConfig = new AppConfig();
        private ClientConfig clientConfig = new ClientConfig();
        private CorsManagerOptions corsOptions = new CorsManagerOptions();
        private KeePassConfig keePassConfig = new KeePassConfig();

        public Startup(IConfiguration configuration)
        {
            Configuration = new SchemaConfigurationBinder(configuration);
            Configuration.Bind("JwtAuth", authConfig);
            Configuration.Bind("AppConfig", appConfig);
            Configuration.Bind("ClientConfig", clientConfig);
            Configuration.Bind("Cors", corsOptions);
            Configuration.Bind("KeePass", keePassConfig);

            clientConfig.BearerCookieName = $"{authConfig.ClientId}.BearerToken";

            if (string.IsNullOrWhiteSpace(appConfig.CacheToken))
            {
                appConfig.CacheToken = this.GetType().Assembly.ComputeMd5();
            }
        }

        public SchemaConfigurationBinder Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Threax.AspNetCore.Docker.Certs.CertManager.LoadTrustedRoots(o => Configuration.Bind("CertManager", o));

            services.AddThreaxProgressiveWebApp(o => Configuration.Bind("DisplayConfig", o));

            //Add the client side configuration object
            services.AddClientConfig(clientConfig, o =>
            {
                o.RouteArgsToClear = new List<string>() { "inPagePath" };
            });

            services.AddAssetBundle(o =>
            {
                o.UseBundles = appConfig.UseAssetBundles;
            });

            ApiExplorerController.Allow = appConfig.AllowApiExplorer;

            services.AddTimeTracking();

            services.AddHalClientGen(new HalClientGenOptions()
            {
                SourceAssemblies = new Assembly[] { this.GetType().GetTypeInfo().Assembly, typeof(UserSearchController).Assembly },
                CSharp = new CSharpOptions()
                {
                    Namespace = "Threax.Keepass.Client"
                }
            });

            services.AddConventionalIdServerAuthentication(o =>
            {
                o.AppOptions = authConfig;
                o.CookiePath = appConfig.PathBase;
                o.AccessDeniedPath = "/Account/AccessDenied";
                o.EnableIdServerMetadata = appConfig.EnableIdServerMetadata;
                o.CustomizeCookies = cookOpt =>
                {
                    cookOpt.BearerHttpOnly = false;
                };
            });

            services.AddAppDatabase(appConfig.ConnectionString);
            services.AddAppMapper();
            services.AddAppRepositories();

            var halOptions = new HalcyonConventionOptions()
            {
                BaseUrl = appConfig.BaseUrl,
                HalDocEndpointInfo = new HalDocEndpointInfo(typeof(EndpointDocController), appConfig.CacheToken),
                EnableValueProviders = appConfig.EnableValueProviders
            };

            services.AddConventionalHalcyon(halOptions);
            services.AddHalcyonClient();

            services.AddExceptionErrorFilters(new ExceptionFilterOptions()
            {
                DetailedErrors = appConfig.DetailedErrors
            });

            services.AddThreaxIdServerClient(o =>
            {
                Configuration.Bind("IdServerClient", o);
            })
            .SetupHttpClientFactoryWithClientCredentials(o =>
            {
                Configuration.Bind("IdServerClient", o);
                o.GetSharedClientCredentials = s => Configuration.Bind("SharedClientCredentials", s);
            });

            // Add framework services.
            services.AddMvc(o =>
            {
                o.UseExceptionErrorFilters();
                o.UseConventionalHalcyon(halOptions);
            })
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.SetToHalcyonDefault();
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddRazorRuntimeCompilation()
            .AddConventionalIdServerMvc()
            .AddThreaxUserLookup(o =>
            {
                o.UseIdServer();
            })
            .AddThreaxCacheUi(appConfig.CacheToken, o =>
            {
                o.CacheControlHeader = appConfig.CacheControlHeaderString;
            });

            services.ConfigureHtmlRapierTagHelpers(o =>
            {
                o.FrontEndLibrary = HtmlRapier.TagHelpers.FrontEndLibrary.Bootstrap4;
            });

            services.AddScoped<IToolRunner>(s =>
            {
                return new ToolRunner()
                .AddTool("migrate", new ToolCommand("Migrate database to newest version. Run anytime new migrations have been added.", async a =>
                {
                    await a.Migrate();
                    a.Scope.ServiceProvider.GetRequiredService<AppDbContext>().ConvertToEfCore3();
                }))
                .AddTool("seed", new ToolCommand("Seed database data. Only needed for an empty database.", async a =>
                {
                    await a.Seed();
                }))
                .AddTool("addadmin", new ToolCommand("Add given guids as a user with permissions to all roles in the database.", async a =>
                {
                    await a.AddAdmin();
                }))
                .AddTool("updateConfigSchema", new ToolCommand("Update the schema file for this application's configuration.", async a =>
                {
                    var json = await Configuration.CreateSchema();
                    await File.WriteAllTextAsync("appsettings.schema.json", json);
                }))
                .UseClientGenTools();
            });

            services.AddUserBuilderForUserWhitelistWithRoles();

            services.AddThreaxCSP(o =>
            {
                o.AddDefault().AddNone();
                o.AddImg().AddSelf().AddData();
                o.AddConnect().AddSelf();
                o.AddManifest().AddSelf();
                o.AddFont().AddSelf();
                o.AddFrame().AddSelf().AddEntries(new String[] { authConfig.Authority });
                o.AddScript().AddSelf().AddNonce();
                o.AddStyle().AddSelf().AddNonce();
                o.AddFrameAncestors().AddSelf();
            });

            services.AddLogging(o =>
            {
                o.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });

            services.AddSingleton(keePassConfig);
            services.AddSingleton<IStatusLogger, NullStatusLogger>();
            services.AddSingleton<IKeePassService, KeePassService>();

            services.AddEntryPointRenderer<EntryPointController>(e => e.Get());
            services.AddSingleton<AppConfig>(appConfig);

            if (appConfig.EnableResponseCompression)
            {
                services.AddResponseCompression();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
                //Can add ForwardedHeaders.XForwardedFor later, but tricky with container proxy since we don't know its ip
                //This is enough to get https detection working again, however.
                //https://github.com/aspnet/Docs/issues/2384
            });

            app.UseUrlFix(o =>
            {
                o.CorrectPathBase = appConfig.PathBase;
            });

            if (appConfig.EnableResponseCompression)
            {
                app.UseResponseCompression();
            }

            //Setup static files
            var staticFileOptions = new StaticFileOptions();
            if (appConfig.CacheStaticAssets)
            {
                staticFileOptions.OnPrepareResponse = ctx =>
                {
                    //If the request is coming in with a v query it can be cached
                    if (!String.IsNullOrWhiteSpace(ctx.Context.Request.Query["v"]))
                    {
                        ctx.Context.Response.Headers["Cache-Control"] = appConfig.CacheControlHeaderString;
                    }
                };
            }
            app.UseStaticFiles(staticFileOptions);

            app.UseCorsManager(corsOptions, loggerFactory);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "cacheUi",
                    pattern: "{controller=Home}/{cacheToken}/{action=Index}/{*inPagePath}");

                endpoints.MapControllerRoute(
                    name: "root",
                    pattern: "{action=Index}/{*inPagePath}",
                    defaults: new { controller = "Home" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{*inPagePath}");
            });
        }
    }
}
