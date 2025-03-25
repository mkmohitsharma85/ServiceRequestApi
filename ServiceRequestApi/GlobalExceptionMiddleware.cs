using Newtonsoft.Json;
using ServiceRequest.Common.ResponseDTO;
using System.Net;

namespace ServiceRequestApi
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError($"Something went wrong: {exception}");

            var responseDto = new ResponseDTO<string>
            {
                Message = exception.Message,
                Code = HttpStatusCode.InternalServerError,
                Response = new Response<string>
                {
                    Data = null
                }
            };

            var result = JsonConvert.SerializeObject(responseDto);
            return context.Response.WriteAsync(result);
        }
    }

}
