using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Threax.ReflectedServices;

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