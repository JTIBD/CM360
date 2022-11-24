using System;
using System.Net;
using System.Threading.Tasks;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Extensions;
using FMAplication.Models.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FMAplication.Middlewares
{
    public class ItemInHttpContextMiddleware
    {
        private readonly RequestDelegate _next;

        public ItemInHttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRequest userRequest)
        {

            var useragentinfo = userRequest.GetUserAgentInfo;
            if (context.Items.ContainsKey("UserAgentInfo"))
            {
                context.Items.Remove("UserAgentInfo");
            }
            var userIp = userRequest.GetUserIp;
            if (context.Items.ContainsKey("UserIP"))
            {
                context.Items.Remove("UserIP");
            }
            context.Items.Add("UserIP", userIp);
            if (context.User != null )
            {
                var appUser = context.User.ToAppUser();
                appUser.UserAgentInfo = useragentinfo;
                if (context.Items.ContainsKey("AppUser"))
                {
                    context.Items.Remove("AppUser");
                }
                context.Items.Add("AppUser", appUser);

            }
            try
            {
                await this._next(context);
            }catch(Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            AppErrorType errorType = AppErrorType.Default;
            if (ex is AppException) { 
                code = HttpStatusCode.BadRequest;
                errorType = AppErrorType.Alert;
            }
            else if (ex is DefaultException) { 
                code = HttpStatusCode.BadRequest;
                errorType = AppErrorType.Default;
            }
            context.Response.StatusCode = (int)code;
            var result = JsonConvert.SerializeObject(new AppError(ex, errorType));
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
