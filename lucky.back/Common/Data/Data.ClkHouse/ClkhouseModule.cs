using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.ClkHouse
{
    public static class ClkhouseModule
    {
        public static void ClkhouseModuleInitial(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ClkhouseOption>(config.GetSection("ClkhouseOption"));
            services.AddSingleton<ClkhouseUtil>();
            services.AddSingleton<ClkhouseDbExtension>();
        }
    }
}
