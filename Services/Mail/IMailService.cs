using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    public interface IMailService
    {
        //Task SendEmailAsync(EmailTemplate model);
        Task SendEmailAsync(string toEmail, string subject, string Content);
    }
}
