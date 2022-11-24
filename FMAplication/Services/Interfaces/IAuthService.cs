using FMAplication.Models;
using FMAplication.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMAplication.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> GetJWTToken(LoginModel model);
        Task<object> GetJWTToken(AdLoginModel model);

    }
}
