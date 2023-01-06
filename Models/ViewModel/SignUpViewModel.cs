using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class SignUpViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool isActive { get; set; }

    }

    public static class SignUpExtension
    {
        public static List<SignUpViewModel> SignUpMapperList(this List<ApplicationUser> data)
        {
            List<SignUpViewModel> users = new List<SignUpViewModel>();
            foreach (var item in data)
            {
                SignUpViewModel res = new SignUpViewModel();
                res.FirstName = item.FirstName;
                res.LastName = item.LastName;
                res.Email = item.Email;
                res.UserName = item.UserName;
                res.Password = item.PasswordHash;
                res.PhoneNumber = item.PhoneNumber;
                res.isActive = item.isActive;
                users.Add(res);
            }
            return users;
        }

        public static SignUpViewModel SignUpMapper(this ApplicationUser data)
        {
            SignUpViewModel res = new SignUpViewModel();
            res.FirstName = data.FirstName;
            res.LastName = data.LastName;
            res.Email = data.Email;
            res.UserName = data.UserName;
            res.Password = data.PasswordHash;
            res.PhoneNumber = data.PhoneNumber;
            res.isActive = data.isActive;
            return res;
        }

        public static ApplicationUser SignUpMapper(this SignUpViewModel data)
        {
            ApplicationUser res = new ApplicationUser();
            res.FirstName = data.FirstName;
            res.LastName = data.LastName;
            res.Email = data.Email;
            res.UserName = data.UserName;
            res.PasswordHash = data.Password;
            res.PhoneNumber = data.PhoneNumber;
            res.isActive = data.isActive;
            return res;
        }
    }


}
