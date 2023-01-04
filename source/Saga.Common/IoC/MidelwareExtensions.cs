using Microsoft.AspNetCore.Builder;
using Saga.Common.Middelware;

namespace Saga.Common.IoC
{
    public static class MidelwareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddelware(this IApplicationBuilder services)
        {
            return services.UseMiddleware<ExceptionMiddelware>();
        }
    }
}
