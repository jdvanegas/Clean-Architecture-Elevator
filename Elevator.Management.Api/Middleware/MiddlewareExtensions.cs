﻿using Microsoft.AspNetCore.Builder;

namespace Elevator.Management.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) =>
            builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}