﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Application.DTOs.Response;

namespace Application.Services.Authen
{
    public interface IAccount
    {
        Task CreateAdmin();
        Task<GeneralResponse> CreateAccountAsync(CreateAccountRequestDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginRequestDTO model);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequestDTO model);
        Task<GeneralResponse> CreateRoleAsysnc(CreateRoleRequestDTO model);
        Task<IEnumerable<GetRoleResponseDTO>> GetRolesAsync();
        Task<IEnumerable<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync();
        Task<GeneralResponse> ChangeUserRoleAsync(AssignUserRoleRequestDTO model);
        Task<GeneralResponse> ChangePassAsync(ChangePassRequestDTO model);
        Task<GeneralResponse> AssignUserRoleAsync(AssignUserRoleRequestDTO model);
    }
}
