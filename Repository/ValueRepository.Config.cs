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
    public partial class ValueRepositoryConfig : IServiceSetup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            OnConfigureServices(services);

            services.TryAddScoped<IValueRepository, ValueRepository>();
        }

        partial void OnConfigureServices(IServiceCollection services);
    }
}