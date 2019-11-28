using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Threax.ReflectedServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Threax.Keepass.Repository;

namespace Threax.Keepass.Repository.Config
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