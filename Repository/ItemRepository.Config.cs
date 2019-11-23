using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Threax.ReflectedServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using KeePassWeb.Repository;

namespace KeePassWeb.Repository.Config
{
    public partial class ItemRepositoryConfig : IServiceSetup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            OnConfigureServices(services);

            services.TryAddScoped<IItemRepository, ItemRepository>();
        }

        partial void OnConfigureServices(IServiceCollection services);
    }
}