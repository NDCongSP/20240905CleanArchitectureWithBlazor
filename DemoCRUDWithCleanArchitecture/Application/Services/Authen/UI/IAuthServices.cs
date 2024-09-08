using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.Extentions;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authen.UI
{
    [BaseAddress(ApiRoutes.Identity.BasePath)]
    public interface IAuthServices
    {
        [Post(ApiRoutes.Identity.Login)]
        Task<LoginResponse> LoginAsync(LoginRequestDTO model);
        Task LogoutAsync();
    }
}
