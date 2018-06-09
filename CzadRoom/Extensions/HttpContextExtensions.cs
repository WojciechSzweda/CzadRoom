using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CzadRoom.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserID(this HttpContext httpContext) {
            return httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
