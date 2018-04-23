using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;

namespace WebApi.Multitenancy
{
    public static class TenantConfiguration
    {
        private static Hashtable _tenants = new Hashtable();

        public static IServiceCollection ConfigureTenant(this IServiceCollection serviceCollection, Action<Tenant> configureTenant)
        {
            var tenantServiceCollection = CopyServiceCollection(serviceCollection);
            var tenant = new Tenant(tenantServiceCollection);

            configureTenant?.Invoke(tenant);

            if (!String.IsNullOrWhiteSpace(tenant.TenantId))
            {
                _tenants[tenant.TenantId.ToUpperInvariant()] = tenant;
            }

            return serviceCollection;
        }

        public static Tenant GetTenant(string tenantId) => _tenants[tenantId.ToUpperInvariant()] as Tenant;

        private static IServiceCollection CopyServiceCollection(IServiceCollection source)
        {
            var result = new ServiceCollection();
            ServiceDescriptor[] configuredServices = new ServiceDescriptor[source.Count];

            source.CopyTo(configuredServices, 0);

            for (int i = 0; i < configuredServices.Length; i++)
            {
                result.Insert(i, configuredServices[i]);
            }

            return result;
        }
    }
}