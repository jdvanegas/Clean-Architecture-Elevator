using Elevator.Management.Application.Exceptions;
using Elevator.Management.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Elevator.Management.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private static Task ConvertException(HttpContext context, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new BaseResponse(validationException.ValdationErrors));
                    break;

                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new BaseResponse(badRequestException.Message, false));
                    break;

                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(new BaseResponse(notFoundException.Message, false));
                    break;

                case { }:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(new BaseResponse(exception.Message, false));
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(result);
        }
    }
}