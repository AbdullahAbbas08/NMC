using Moia.BL.AutoMapper;
using Moia.BL.Repositories;
using Moia.BL.Services;
using SmsIntegration;

namespace Moia.Services
{
    public static partial class ServicesRegistration
    {
        public static void RegisterServicesConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
            services.AddScoped<HttpContextAccessor>();
            services.AddScoped<HttpClient>();
            services.AddScoped<IMuslimeRepository, MuslimeRepository>();
            services.AddScoped<IWitnessRepository, WitnessRepository>();
            services.AddScoped<ICommitteeRepository, CommitteeRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ISessionServices, SessionServices>();
            services.AddScoped<IDataProtectRepository, DataProtectRepository>();
            services.AddScoped<ITokenStoreRepository, TokenStoreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IEncryptionServices, EncryptionServices>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddScoped<ISmsIntegrations, SmsIntegrations>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();
            services.AddScoped<IIntegrationRepository, IntegrationRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
        }
    }
}
