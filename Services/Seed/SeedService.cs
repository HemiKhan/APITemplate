using Data.AppContext;
using Data.DataConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Seed
{
    public class SeedService : ISeedService
    {
        private readonly AppDbContext context;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public SeedService(AppDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<Response<string>> SeederAsync()
        {
            try
            {
                // Add Mail Things
                //List<EmailTemplate> emailTemplates = new List<EmailTemplate>
                //{
                //    new EmailTemplate()
                //    {

                //        Subject=Enums.EmailType.ForgetPassword.ToString(),
                //        EmailType=Enums.EmailType.ForgetPassword.ToString(),
                //        Body="<h3>Follow the instructions to reset your password</h3>" +
                //            "<br><p><b>To reset your password</b></p>",
                //        Link=$"{Constrant.ApiUrl}/ResetPassword",
                //    },
                //    new EmailTemplate()
                //    {

                //        Subject=Enums.EmailType.ConfirmEmail.ToString(),
                //        EmailType=Enums.EmailType.ConfirmEmail.ToString(),
                //        Body="<h3>Welcome to </h3>" +
                //              "<br><p><b>Please confirm your email by</b></p>",
                //        Link=$"{Constrant.ApiUrl}/api/account/confirmemail",
                //    }
                //};

                //foreach (var item in emailTemplates)
                //{
                //    var IsExist = await context.EmailTemplates.FirstOrDefaultAsync(x => x.EmailType == item.EmailType.ToString());
                //    if (IsExist is null)
                //    {
                //        await context.EmailTemplates.AddAsync(item);
                //    }
                //    else
                //    {
                //        IsExist.Body = item.Body;
                //        IsExist.EmailType = item.EmailType.ToString();
                //        IsExist.Subject = item.Subject;
                //        IsExist.Link = item.Link;
                //        context.Entry(IsExist).State = EntityState.Modified;
                //    }
                //}

                // Add Roles
                List<ApplicationRole> roles = new List<ApplicationRole>()
                {
                    new ApplicationRole() { Name="Admin", NormalizedName="Admin", Hidden=false, Deleted = false },
                    new ApplicationRole() { Name="User", NormalizedName="User", Hidden=false, Deleted = false }
                };
                foreach (var role in roles)
                {
                    var IsExist = await roleManager.RoleExistsAsync(role.ToString());
                    if (!IsExist)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    else
                    {
                        var ExistingRole = await roleManager.FindByNameAsync(role.Name);
                        ExistingRole.Name = role.Name;
                        ExistingRole.NormalizedName = role.NormalizedName;
                        await roleManager.UpdateAsync(ExistingRole);
                    }
                }

                //Add Users
                List<ApplicationUser> applicationUsers = new List<ApplicationUser>()
                {
                    new ApplicationUser { FirstName = "Admin", LastName = "Admin", Email = "admin@gmail.com", UserName = "admin", PasswordHash = "Admin.123", PhoneNumber = "03189159007", isActive = true },
                    new ApplicationUser { FirstName = "Hammas", LastName = "Khan", Email = "hammaskhan01@gmail.com", UserName = "hammaskhan01", PasswordHash = "Hemi.4364", PhoneNumber = "03470713121", isActive = true },
                };
                foreach (var user in applicationUsers)
                {
                    var IsExist = await userManager.FindByEmailAsync(user.Email);
                    if (IsExist is null)
                    {
                        await userManager.CreateAsync(user, user.PasswordHash);
                    }
                    else
                    {
                        IsExist.FirstName = user.FirstName;
                        IsExist.LastName = user.LastName;
                        IsExist.Email = user.Email;
                        IsExist.UserName = user.UserName;
                        IsExist.PhoneNumber = user.PhoneNumber;
                        IsExist.isActive = user.isActive;
                        var password = new PasswordHasher<ApplicationUser>();
                        IsExist.PasswordHash = password.HashPassword(null, user.PasswordHash);
                        await userManager.UpdateAsync(IsExist);
                    }
                }
                await context.SaveChangesAsync();

                //Assign Roles
                var adminEmail = await userManager.FindByEmailAsync("admin@gmail.com");
                var hammasEmail = await userManager.FindByEmailAsync("hammaskhan01@gmail.com");
                var adminRole = await roleManager.FindByNameAsync("Admin");
                if (adminRole != null)
                {
                    if (adminEmail != null)
                    {
                        if (!(await userManager.IsInRoleAsync(adminEmail, adminRole.Name)))
                        {
                            await userManager.AddToRoleAsync(adminEmail, adminRole.ToString());
                        }
                    }

                    if (hammasEmail != null)
                    {
                        if (!(await userManager.IsInRoleAsync(hammasEmail, adminRole.Name)))
                        {
                            await userManager.AddToRoleAsync(hammasEmail, adminRole.ToString());
                        }
                    }
                }               

                await context.SaveChangesAsync();
                return new Response<string>
                {
                    Message = "SeedData add or update successfully",
                    Status = true
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
