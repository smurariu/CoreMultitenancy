using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Multitenancy
{
    public class Tenant
    {
        public string TenantId { get; set; }
        public IServiceCollection ServiceCollection { get; }

        public Tenant(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }
}