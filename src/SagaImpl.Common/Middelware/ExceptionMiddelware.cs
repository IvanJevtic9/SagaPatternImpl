using Microsoft.AspNetCore.Http;
using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SagaImpl.Common.Middelware
{
    public class ErrorResponse
    {
        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class ExceptionMiddelware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddelware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            var response = GetErrorResponse(exception);
            var responseJson = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetHttpStatusCode(exception);

            return context.Response.WriteAsync(responseJson);
        }

        private static ErrorResponse GetErrorResponse(Exception exception)
        {
            ErrorResponse errorResponse;

            if (exception is ValidationException validationException)
            {
                errorResponse = new ErrorResponse()
                {
                    Description = validationException.Message,
                    Timestamp = DateTime.Now
                };
            }
            else
            {
                errorResponse = new ErrorResponse()
                {
                    Description = exception.Message,
                    Timestamp = DateTime.Now
                };
            }

            return errorResponse;
        }

        private static int GetHttpStatusCode(Exception exception)
        {
            if (exception is ValidationException || exception is ApplicationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}

