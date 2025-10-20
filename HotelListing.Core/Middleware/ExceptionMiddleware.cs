using HotelListing.Exceptions;
using HotelListing.Models.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.Core.Middleware
{
    public class ExceptionMiddleware
    {
        public  RequestDelegate _next;
        public ILogger <ExceptionMiddleware> _logger;        

       
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

       
        public async Task InvokeAsync(HttpContext context)
        {
            //middleware hijacks every request and wraps it in a try catchblock
            try
            {

                await _next(context);
            }
            catch(Exception err)
            {
                await HandleExceptions(context, err);
                _logger.LogError("Something went wrong int the path :{Path}", context.Request.Path);
            }
        }

        //create error response
        private Task HandleExceptions(HttpContext context, Exception ex) {

            //create a 500 internal server error response
            context.Response.ContentType = "application/json";
           
            HttpStatusCode defaultStatusCode = HttpStatusCode.InternalServerError;
//convert into json response
            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message

            };

            switch (ex)
                //when its bad request exception type
            {
                // if exception is of type not found exception
                case NotFoundException notFoundException:
                    //create new statuscode of notfound insted of internal server error
                    defaultStatusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                case UnAuthorizedException unauthorizedException:
                    defaultStatusCode = HttpStatusCode.Unauthorized;
                    errorDetails.ErrorType = "Unauthorized";
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{defaultStatusCode}",
                Title = GetDefaultTitleForStatusCode(defaultStatusCode),
                Status = (int)defaultStatusCode,
                Detail = ex.Message,
                Instance = context.Request.Path

            };

           

            //convert details inst json string
            //string response = JsonConvert.SerializeObject(errorDetails);
            string response = JsonConvert.SerializeObject(problemDetails);
            context.Response.StatusCode = (int)defaultStatusCode;
            return context.Response.WriteAsync(response);
            
        }
                    // Fix for CS8070, CS0723, CS1003, CS1002, CS1513
        // Replace the GetDefaultTitleForStatusCode method with correct switch syntax and parameter type
        private static string GetDefaultTitleForStatusCode(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.BadRequest:
                    return "Bad request";

                case HttpStatusCode.Unauthorized:
                    return "Unauthorized";

                case HttpStatusCode.Forbidden:
                    return "Forbidden";

                case HttpStatusCode.InternalServerError:
                    return "Internal Server Error";

                default:
                    return "err";
            }
        }
    }
}
