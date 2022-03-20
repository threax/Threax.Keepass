using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Threax.Keepass.InputModels;
using Threax.Keepass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Threax.AspNetCore.BuiltInTools;
using Threax.AspNetCore.Models;
using Threax.AspNetCore.UserBuilder.Entities;
using Threax.Sqlite.Ext;
using Threax.Keepass.Mappers;

namespace Threax.Keepass.Database
{
    public static class AppDatabaseServiceExtensions
    {
        /// <summary>
        /// Setup the app database. This will register AppDbContext in the services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <returns></returns>
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, string connectionString)
        {
            SqliteFileExtensions.TryCreateFile(connectionString);

            //Add the database
            services.AddAuthorizationDatabase<AppDbContext>()
                    .AddDbContextPool<AppDbContext>(o =>
                    {
                        o.UseSqlite(connectionString, options =>
                        {
                            options.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
                        });
                    });

            return services;
        }

        /// <summary>
        /// Setup the app mapper. This will make the AppMapper class available in services.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="includeAutomapperConfig">Set this to true to register the automapper config in the services. This is used to allow the automapper unit test to work.</param>
        /// <returns></returns>
        public static IServiceCollection AddAppMapper(this IServiceCollection services, bool includeAutomapperConfig = false)
        {
            //Setup mappings between your objects here
            //Check out the AutoMapper docs for more info
            //https://github.com/AutoMapper/AutoMapper/wiki
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                //Auto find profile classes
                var profiles = typeof(AppDatabaseServiceExtensions).GetTypeInfo().Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Profile)))
                    .Select(i => Activator.CreateInstance(i) as Profile)
                    .ToList();

                cfg.AddProfiles(profiles);
            });

            if (includeAutomapperConfig)
            {
                services.AddSingleton<MapperConfiguration>(mapperConfig);
            }

            //Register the AppMapper, The Automapper config is hidden behind the AppMapper, which is what should be used by your classes.
            services.AddScoped<AppMapper>(s => new AppMapper(mapperConfig.CreateMapper(s.GetRequiredService)));

            return services;
        }

        /// <summary>
        /// Setup the app repositories. This will setup the services with the repositories defined in this program.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.ConfigureReflectedServices(typeof(AppDatabaseServiceExtensions).GetTypeInfo().Assembly);

            return services;
        }

        /// <summary>
        /// Run the migrate tool.
        /// </summary>
        /// <param name="toolArgs">The tools args.</param>
        public static Task Migrate(this ToolArgs toolArgs)
        {
            var context = toolArgs.Scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return context.Database.MigrateAsync();
        }

        /// <summary>
        /// Run the seed tool, this should check to make sure that it is safe to apply the seed data.
        /// This means that the seed tool should make sure tables are empty before modifying them
        /// or otherwise leave existing data in the database alone.
        /// </summary>
        /// <param name="toolArgs">The tools args.</param>
        public static async Task Seed(this ToolArgs toolArgs)
        {
            //Seed the authorization database, this will automatically manage roles and will add
            //any roles not currently in the database.
            var context = toolArgs.Scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.SeedAuthorizationDatabase(Roles.DatabaseRoles());
        }

        /// <summary>
        /// Add a user as an "admin" this means they get all the roles.
        /// </summary>
        /// <param name="toolArgs">The tools args.</param>
        public static Task AddAdmin(this ToolArgs toolArgs)
        {
            if (toolArgs.Args.Count == 0)
            {
                throw new ToolException("Must add user guids as args to the addadmin tool.");
            }

            var repo = toolArgs.Scope.ServiceProvider.GetRequiredService<IUserEntityRepository>();
            return repo.AddAdmins(toolArgs.Args.Select(i => new User()
            {
                UserId = Guid.Parse(i),
                Name = $"AddAdmin {i}"
            }), Roles.DatabaseRoles());
        }
    }
}
