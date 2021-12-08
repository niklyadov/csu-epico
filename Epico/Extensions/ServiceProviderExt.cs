using Microsoft.Extensions.DependencyInjection;

namespace Epico.Extensions
{
    public static class ServiceProviderExt
    {
        public static T GetService<T>(this ServiceProvider provider) where T : class
        {
            return provider.GetService(typeof(T)) as T;
        }
    }
}