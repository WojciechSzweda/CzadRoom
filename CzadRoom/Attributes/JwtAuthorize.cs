using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Attributes
{
    public class JwtAuthorize : AuthorizeAttribute
    {
        public JwtAuthorize() : base() {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
