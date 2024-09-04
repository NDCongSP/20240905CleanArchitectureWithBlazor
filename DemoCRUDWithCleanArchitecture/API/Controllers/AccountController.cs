using Application.Contracts;
using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpPost("indentity/create")]
        public async Task<ActionResult<GeneralResponse>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.CreateAccountAsync(model));
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<GeneralResponse>> LoginAccount(LoginDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.LoginAccountAsync(model));
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<GeneralResponse>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.RefreshTokenAsync(model));
        }

        [HttpPost("identity/role/create")]
        public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.CreateRoleAsysnc(model));
        }

        [HttpGet("identity/role/list")]
        public async Task<ActionResult<IEnumerable<GeneralResponse>>> GetRole()
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.GetRolesAsync());
        }

        [HttpPost("setting")]
        public async Task<ActionResult> CreateAdmin()
        {
            await account.CreateAdmin();
            return Ok();
        }

        [HttpGet("identity/user-with-role")]
        public async Task<ActionResult<IEnumerable<GeneralResponse>>> GetUserWithRole()
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.GetRolesAsync());
        }

        [HttpPost("identity/change-role")]
        public async Task<ActionResult<GeneralResponse>> GetRole(ChangeUserRoleRequestDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.ChangeUserRoleAsync(model));
        }

        [HttpPost("identity/change-pass")]
        public async Task<ActionResult<GeneralResponse>> ChangePass(ChangePassDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("Model cannot be null");

            return Ok(await account.ChangePass(model));
        }
    }
}
