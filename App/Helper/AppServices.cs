using Services.Account;
using Services.Mail;
using Services.Seed;
using Services.StoredProcedures;

namespace App.Helper
{
    public static class AppServices
    {
        public static IServiceCollection AddAppServices(this IServiceCollection appservices)
        {
            appservices.AddTransient<IStoredProcedureService, StoredProcedureService>();
            appservices.AddTransient<IAccountService, AccountService>();
            appservices.AddTransient<ISeedService, SeedService>();
            appservices.AddTransient<IMailService, MailService>();
            appservices.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            return appservices;
        }
    }
}
