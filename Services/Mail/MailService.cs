using Data.DataConfig;
using Microsoft.Extensions.Configuration;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //public async Task SendEmailAsync(EmailTemplate model)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(model.To);
        //        var from = Constrant.MailFrom;
        //        mail.From = new MailAddress(from);
        //        mail.Subject = model.Subject;
        //        mail.Body = model.Body;
        //        if (model.BCC is not null)
        //        {
        //            mail.Bcc.Add(model.BCC);
        //        }
        //        if (model.CC is not null)
        //        {
        //            mail.CC.Add(model.CC);
        //        }
        //        mail.IsBodyHtml = true;
        //        using var smtp = new SmtpClient();
        //        smtp.Host = Constrant.MailHost;
        //        smtp.Port = Constrant.MailPort;
        //        var password = Constrant.MailPassword;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential(from, password);
        //        smtp.EnableSsl = true;
        //        await smtp.SendMailAsync(mail);
        //        smtp.Dispose();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public async Task SendEmailAsync(string toEmail, string subject, string Content)
        {           
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            var from = configuration["MailSettings:From"];
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            using var smtp = new SmtpClient();
            smtp.Host = configuration["MailSettings:Host"];
            smtp.Port = Convert.ToInt32(configuration["MailSettings:Port"]);
            var password = configuration["MailSettings:Password"];
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
            smtp.Dispose();
        }
    }
}
