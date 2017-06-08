using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRavenLight.AspNetCore
{
    public static class SentryExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseSentryExceptionHandler(this IApplicationBuilder builder,string dsnUrl)
        {
            return builder.UseMiddleware<SentryExceptionHandlerMiddleware>();
        }
    }
}
