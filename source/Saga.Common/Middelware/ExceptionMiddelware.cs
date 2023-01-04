using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Saga.Common.Models;
using System.Net;

namespace Saga.Common.Middelware
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {

            var response = GetErrorResponse(exception);
            var responseJson = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return httpContext.Response.WriteAsync(responseJson);
        }

        private static ErrorResponse GetErrorResponse(Exception exception)
        {
            var errorResponse = new ErrorResponse()
            {
                Description = exception.Message,
                Timestamp = DateTime.Now
            };

            return errorResponse;
        }
    }
}
