using Data.AppContext;
using Data.DataConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Model;
using Models.ViewModel;
using Services.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IMailService mailService;

        public AccountService(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IMailService mailService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mailService = mailService;
        }

        public async Task<Response<SignUpViewModel>> GetUserAsync()
        {
            try
            {
                var result = await userManager.Users.ToListAsync();
                if (result != null)
                {
                    return new Response<SignUpViewModel>
                    {
                        Message = "Record Found Successfully",
                        Status = true,
                        List = result.SignUpMapperList()

                    };
                }
                return new Response<SignUpViewModel>
                {
                    Message = "Record Not Found",
                    Status = false
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<SignUpViewModel>> SignupAsync(SignUpViewModel register)
        {
            try
            {
                if (register == null)
                {
                    throw new NullReferenceException("Something is Wrong!");
                }
                if (register.Password != register.ConfirmPassword)
                {
                    return new Response<SignUpViewModel>
                    {
                        Message = "Confirm Password is Not Matched with Password",
                        Status = false
                    };
                }
                var user = new ApplicationUser
                {
                    Email = register.Email,
                    UserName = register.Email
                };
                var result = await userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    var ConfirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedEmailToken = Encoding.UTF8.GetBytes(ConfirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                    //var emailtemplate = context.EmailTemplates.FirstOrDefaultAsync(x => x.EmailType == Enums.EmailType.ConfirmEmail.ToString()).Result;
                    //if (emailtemplate is null)
                    //    return new Response<SignUpViewModel>
                    //    {
                    //        Message = "No Email Template available",
                    //        Status = false
                    //    };
                    //emailtemplate.Link = $"{emailtemplate.Link}?userId={user.Id}&token={validEmailToken}";
                    //emailtemplate.To = user.Email;
                    //emailtemplate.Body = emailtemplate.Body + $"<a href='{emailtemplate.Link}'><b>Click Here on Link For Conformation</b></a>";
                    //await mailService.SendEmailAsync(emailtemplate);
                    return new Response<SignUpViewModel>
                    {
                        Message = "User is Created Successfully",
                        Status = true
                    };
                }
                return new Response<SignUpViewModel>
                {
                    Message = "User Creation Failed",
                    Status = false
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<string>> LoginAync(LoginViewModel login)
        {
            try
            {
                if (login is null)
                {
                    throw new NullReferenceException("Something is Worng in Model!");
                }
                //Email Existing Checking
                var user = await userManager.FindByEmailAsync(login.Email);
                
                if (user is null)
                {
                    //User Existing Checking
                    user = await userManager.FindByNameAsync(login.Email);

                    if (user is null)
                    {
                        return new Response<string>
                        {
                            Message = "Data Does Not Exists",
                            Status = false
                        };
                    }
                }
                var Role = await userManager.GetRolesAsync(user);
                var rolename = Role[0];
                //Email and Password Checking
                var pass = await userManager.CheckPasswordAsync(user, login.Password);
                if (!pass)
                {
                    return new Response<string>
                    {
                        Message = "Login Password is Worng...!",
                        Status = false
                    };
                }
                else
                {
                    //Generate Token & Token Working
                    var claims = new[]
                    {
                        new Claim("Email",user.Email),
                        new Claim("FirstName",user.FirstName!),
                        new Claim("LastName",user.LastName!),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("UserRole", rolename),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constrant.Authenticatekey));
                    var token = new JwtSecurityToken(
                        issuer: Constrant.AuthenticateIssuer,
                        audience: Constrant.AuthenticateAudience,
                        expires: DateTime.Now.AddDays(7),
                        claims: claims,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );

                    var stringtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    //Session Log working
                    SessionLog sessionLog = new SessionLog();
                    sessionLog.UserId = user.Id;
                    sessionLog.LoginTime = DateTime.Now;
                    await context.SessionLog.AddAsync(sessionLog);
                    await context.SaveChangesAsync();

                    return new Response<string>
                    {
                        Message = "Login Successfully...!",
                        Status = true,
                        Expiredate = token.ValidTo,
                        Data = stringtoken
                    };
                }                
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new Response<string>
                {
                    Message = "User Id must be required",
                    Status = false
                };
            if (string.IsNullOrWhiteSpace(token))
                return new Response<string>
                {
                    Message = "Token must be required",
                    Status = false
                };
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new Response<string>
                {
                    Message = "User Not Found",
                    Status = false
                };
            }
            var DecodeEmailToken = WebEncoders.Base64UrlDecode(token);
            var ValidEmailToken = Encoding.UTF8.GetString(DecodeEmailToken);
            var result = await userManager.ConfirmEmailAsync(user, ValidEmailToken);
            if (result.Succeeded)
            {
                return new Response<string>
                {
                    Message = "Email Confirm Successfully",
                    Status = true
                };
            }
            else
            {
                return new Response<string>
                {
                    Message = "Email Did Not Confirm",
                    Status = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }
        }

        public async Task<Response<string>> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new NullReferenceException("Forget Password Email is Empty");
            var result = await userManager.FindByEmailAsync(email);
            if (result is null)
            {
                return new Response<string>
                {
                    Message = "No User With This Email Address",
                    Status = false
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(result);
            var EncodeEmailToken = Encoding.UTF8.GetBytes(token);
            var ValidEmailToken = WebEncoders.Base64UrlEncode(EncodeEmailToken);

            string url = $"{configuration["AppUrl"]}/ResetPassword?email={result.Email}&token={ValidEmailToken}";
            await mailService.SendEmailAsync(result.Email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" + $"<p>To reset your password <a href='{url}'>Click Here</a></p>");

            //var emailtemplate = await context.EmailTemplates.FirstOrDefaultAsync(x => x.EmailType == Enums.EmailType.ForgetPassword.ToString());
            //if (emailtemplate is null)
            //    return new Response<string>
            //    {
            //        Message = "No Email Template available",
            //        Status = false
            //    };
            //emailtemplate.Link = $"{emailtemplate.Link}?email={result.Email}&token={ValidEmailToken}";
            //emailtemplate.To = result.Email;
            //emailtemplate.Body = emailtemplate.Body + $"<a href='{emailtemplate.Link}'><b>please click here</b></a>";
            //await mailService.SendEmailAsync(emailtemplate);

            return new Response<string>
            {
                Message = "Reset Password URL has been sent",
                Status = true
            };
        }

        public async Task<Response<ResetPasswordViewModel>> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {
                if (model is null)
                    throw new NullReferenceException("Reset Model is empty");
                if (model.NewPassword != model.ConfirmPassword)
                    return new Response<ResetPasswordViewModel>
                    {
                        Message = "Confirm password does not match with password",
                        Status = false
                    };
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return new Response<ResetPasswordViewModel>
                    {
                        Message = "Email must be required for password reset",
                        Status = false
                    };
                var decodedtoken = WebEncoders.Base64UrlDecode(model.Token);
                var token = Encoding.UTF8.GetString(decodedtoken);
                var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                    return new Response<ResetPasswordViewModel>
                    {
                        Message = "Password successfully has been reset",
                        Status = true
                    };
                return new Response<ResetPasswordViewModel>
                {
                    Message = "Something went wrong",
                    Status = false,
                    Errors = result.Errors.Select(x => x.Description)
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<ChangePasswordViewModel>> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            try
            {
                if (model is null)
                    throw new NullReferenceException("Something is wrong with model");
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is null)
                    return new Response<ChangePasswordViewModel>
                    {
                        Message = "User does not exist with this email Address",
                        Status = false
                    };
                var result = await userManager.CheckPasswordAsync(user, model.OldPassword);
                if (!result)
                    return new Response<ChangePasswordViewModel>
                    {
                        Message = "Password does not match with account detail",
                        Status = false
                    };
                if (model.NewPassword != model.ConfirmPassword)
                    return new Response<ChangePasswordViewModel>
                    {
                        Message = "ConfirmPassword does not match with Newpassword",
                        Status = false
                    };
                var updateresult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!updateresult.Succeeded)
                    return new Response<ChangePasswordViewModel>
                    {
                        Message = "Password not change successfully.",
                        Status = false
                    };
                return new Response<ChangePasswordViewModel>
                {
                    Message = "Password change successfully.",
                    Status = true
                };
            }
            catch
            {
                throw;
            }

        }


        public async Task<Response<string>> AssignUserRoles(UserRolesViewModel userRoles)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(userRoles.UserEmail);
                if (user is null)
                    return new Response<string>
                    {
                        Message = "No user exist with this email address",
                        Status = false
                    };
                if (userRoles.Roles!.Count() > 0)
                {
                    var RoleNoExist = string.Empty;
                    foreach (var role in userRoles.Roles!)
                    {
                        var roleresult = await roleManager.FindByNameAsync(role.RoleName);
                        if (roleresult != null)
                        {
                            await userManager.AddToRoleAsync(user, roleresult.ToString());
                        }
                        else
                        {
                            continue;
                        }
                    }
                    await context.SaveChangesAsync();
                    return new Response<string>
                    {
                        Message = "Role successfully assign to user",
                        Status = true
                    };
                }
                return new Response<string>
                {
                    Message = "No Role Select",
                    Status = false
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
