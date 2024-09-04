using Application.Contracts;
using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Extentions;
using Azure;
using Domain.Entity.Authentication;
using Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Repos
{
    public class AccountRepository(RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager, IConfiguration config,
        SignInManager<ApplicationUser> signInManager,
        AppDbContext context) : IAccount
    {
        private async Task<ApplicationUser> FindUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);
        private async Task<IdentityRole> FindRoleByNameAsync(string roleName)
            => await roleManager.FindByNameAsync(roleName);

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<GeneralResponse> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            if (user == null || role is null) return new GeneralResponse(false, "Model state cannot be empty");

            if (await FindRoleByNameAsync(role.Name) == null) await CreateRoleAsysnc(role.Adapt(new CreateRoleDTO()));

            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
            string error = CheckReponse(result);
            if (!string.IsNullOrEmpty(error)) return new GeneralResponse(false, error);
            else return new GeneralResponse(true, $"{user.Name} assigned to {role.Name} role");
        }

        private static string CheckReponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(_ => _.Description);
                return string.Join(Environment.NewLine, error);
            }

            return null;
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,(await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("FullName",user.Name)
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null;
            }
        }

        private async Task<GeneralResponse> SaveRefreshTokenAsync(string userId, string token)
        {
            try
            {
                var user = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
                if (user == null)
                    await context.RefreshTokens.AddAsync(new RefreshToken() { UserId = userId, Token = token });
                else
                    user.Token = token;
                await context.SaveChangesAsync();
                return new GeneralResponse(true, null);
            }
            catch (Exception ex) { return new GeneralResponse(false, ex.Message); }
        }

        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            if (await FindUserByEmailAsync(model.userEmail) is null) return new GeneralResponse(false, "User not found");
            if (await FindRoleByNameAsync(model.roleName) is null) return new GeneralResponse(false, "Role not found");

            var user = await FindUserByEmailAsync(model.userEmail);
            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);

            var error = CheckReponse(removeOldRole);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);

            var result = await userManager.AddToRoleAsync(user, model.roleName);
            var response = CheckReponse(result);
            if (!string.IsNullOrEmpty(response))
                return new GeneralResponse(false, response);
            else
                return new GeneralResponse(true, "Role changed");
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await userManager.FindByEmailAsync(model.EmailAddress) != null)
                    return new GeneralResponse(false, "Sorry, user is already created.");

                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.Name,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };

                var result = await userManager.CreateAsync(user, model.Password);
                string error = CheckReponse(result);
                if (!string.IsNullOrEmpty(error)) return new GeneralResponse(false, error);

                var (flag, message) = await AssignUserToRole(user, new IdentityRole() { Name = model.Role });
                return new GeneralResponse(true, message);
            }
            catch (Exception ex) { return new GeneralResponse(false, ex.Message); }
        }

        public async Task CreateAdmin()
        {
            try
            {
                if ((await FindRoleByNameAsync(Constant.Role.Admin)) != null) return;
                var admin = new CreateAccountDTO()
                {
                    Name = "Admin",
                    Password = "Admin123@456",
                    EmailAddress = "admin@gmail.com",
                    Role = Constant.Role.Admin
                };

                await CreateAccountAsync(admin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> CreateRoleAsysnc(CreateRoleDTO model)
        {
            try
            {
                if (await FindRoleByNameAsync(model.Name) != null) return new GeneralResponse(false, $"{model.Name} already created.");

                var respone = await roleManager.CreateAsync(new IdentityRole() { Name = model.Name });
                var error = CheckReponse(respone);
                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse(false, error);
                else
                    return new GeneralResponse(true, $"{model.Name} created.");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync()
            => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();

        public async Task<IEnumerable<GetUserWithRoleResponseDTO>> GetUsersWithRolesAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers == null) return null;

            var list = new List<GetUserWithRoleResponseDTO>();
            foreach (var item in allUsers)
            {
                var getUserRole = (await userManager.GetRolesAsync(item)).FirstOrDefault();
                var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == getUserRole.ToLower());
                list.Add(new GetUserWithRoleResponseDTO()
                {
                    Name = item.Name,
                    Email = item.Email,
                    RoleId = getRoleInfo.Id,
                    RoleName = getRoleInfo.Name,
                });
            }
            return list;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);
                if (user == null) return new LoginResponse(false, "User not found");

                SignInResult result = null;
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch
                {
                    return new LoginResponse(false, "Invalid credentials");
                }

                if (!result.Succeeded) return new LoginResponse(false, "Invalid credentials");

                string jwtToken = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                    return new LoginResponse(false, "Error occured while in account, please contact administrator.");
                else
                {
                    //save token after login successfull 
                    var saveResult = await SaveRefreshTokenAsync(user.Id, refreshToken);
                    if (saveResult.flag)
                        return new LoginResponse(true, $"{user.Name} successfully logged in.", jwtToken, refreshToken);
                    else return new LoginResponse();
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse(false, ex.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.Token);
            if (token is null) return new LoginResponse();

            var user = await userManager.FindByIdAsync(token.UserId);
            string newToken = await GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();
            var saveResult = await SaveRefreshTokenAsync(user.Id, newRefreshToken);
            if (saveResult.flag)
                return new LoginResponse(true, $"{user.Name} successfully re-logged in.", newToken, newRefreshToken);
            else
                return new LoginResponse();
        }

        public async Task<GeneralResponse> ChangePass(ChangePassDTO model)
        {
            var user = await FindUserByEmailAsync(model.email);
            if (user == null) return new GeneralResponse(false, "User not found");

            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, model.newPass);

            return new GeneralResponse(true, "Passwork changed");
        }
    }
}
