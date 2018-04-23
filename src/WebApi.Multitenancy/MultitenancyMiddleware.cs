using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace WebApi.Multitenancy
{
    public class MultitenancyMiddleware
    {
        private readonly RequestDelegate _next;

        Func<HttpRequest, string> _tenantIdentificationStrategy = UrlPathPrefixIdentificationStrategy;

        public MultitenancyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public MultitenancyMiddleware(RequestDelegate next, Func<HttpRequest, string> tenantIdentificationStrategy)
            : this(next)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var tenantId = _tenantIdentificationStrategy(context.Request);
            var tenant = GetTenant(tenantId);

            if (tenant == null)
            {
                //forbiden or whatever
            }
            else
            {
                context.RequestServices = tenant.ServiceCollection.BuildServiceProvider();
            }

            return _next(context);
        }

        private Tenant GetTenant(string tenantId)
            => TenantConfiguration.GetTenant(tenantId);

        private static string UrlPathPrefixIdentificationStrategy(HttpRequest currentRequest)
        {
            string tenantId = null;

            var requestPath = currentRequest?.Path;
            if (requestPath != null && requestPath.HasValue)
            {
                var pathSegments = requestPath.Value.Value.Split('/');
                if (pathSegments.Length >= 2)
                {
                    tenantId = pathSegments[1];
                }
            }

            return tenantId;
        }
    }

    public static class MultitenancyMiddlewareExtensions
    {
        public static MvcOptions UseTenantRoutePrefix(this MvcOptions mvcOptions, string tenantParameterName = "tenantId")
        {
            mvcOptions.UseCentralRoutePrefix(new RouteAttribute("{" + tenantParameterName + ":alpha}"));
            return mvcOptions;
        }

        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultitenancyMiddleware>();
        }

        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder builder,
            Func<HttpRequest, object> _tenantIdentificationStrategy)
        {
            return builder.UseMiddleware<MultitenancyMiddleware>(_tenantIdentificationStrategy);
        }
    }
}