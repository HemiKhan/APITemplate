using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Account;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var data = await accountService.GetUserAsync();
            if (data.Status)
                return Ok(data);
            return BadRequest(data);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpViewModel register)
        {
            if (ModelState.IsValid)
            {
                var data = await accountService.SignupAsync(register);
                if (data.Status)
                    return Ok(data);
                return BadRequest(data);
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var data = await accountService.LoginAync(login);
                if (data.Status)
                    return Ok(data);
                return BadRequest(data);
            }
            return BadRequest();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (!string.IsNullOrWhiteSpace(userId) || !string.IsNullOrWhiteSpace(token))
            {
                var data = await accountService.ConfirmEmailAsync(userId, token);
                if (data.Status)
                    return Ok(data);
                return BadRequest(data);
            }
            return BadRequest();
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var data = await accountService.ForgetPasswordAsync(email);
                if (data.Status)
                    return Ok(data);
                return BadRequest(data);
            }
            return BadRequest();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);
            var data = await accountService.ResetPasswordAsync(model);
            if (data.Status)
                return Ok(data);
            return BadRequest(data);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);
            var data = await accountService.ChangePasswordAsync(model);
            if (data.Status)
                return Ok(data);
            return Ok(data);
        }

        [HttpPost("AssignUserRoles")]
        public async Task<IActionResult> AssignUserRoles(UserRolesViewModel model)
        {
            if (model is null)
                throw new NullReferenceException("Something is wrong with model");
            var result = await accountService.AssignUserRoles(model);
            if (result.Status)
                return Ok(result);
            return BadRequest(result);
        }
    }
}