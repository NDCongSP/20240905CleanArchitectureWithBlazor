using System;
using System.Collections.Generic;
using System.Linq;
using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Application.DTOs.Response;

namespace Application.Contracts
{
    public interface IAccount
    {
        Task CreateAdmin();
        Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
        Task<GeneralResponse> CreateRoleAsysnc(CreateRoleDTO model);
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();
        Task<IEnumerable<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync();
        Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
        Task<GeneralResponse> ChangePassAsync(ChangePassDTO model);
        Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleDTO model);
    }
}
