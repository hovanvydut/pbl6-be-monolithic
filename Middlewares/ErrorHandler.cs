using Microsoft.AspNetCore.Diagnostics;
using Monolithic.Models.Common;
using System.Net;

namespace Monolithic.Middlewares;

public static class ErrorHandler
{
    public static void ConfigureErrorHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var statusCode = exception.GetType() == typeof(BaseException) ?
                                            ((BaseException)exception).StatusCode :
                                            (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(new BaseResponse<int>()
                    {
                        Success = false,
                        StatusCode = statusCode,
                        Message = exception.Message
                    }.ToString());
                }
            });
        });
    }
}