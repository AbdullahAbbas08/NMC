namespace Moia.Services
{
    public static partial class ServicesRegistration
    {
        public static void ConfigureHangfire(this IServiceCollection services, string connectionString)
        {

            services.AddHangfire(configuration => configuration
                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseSqlServerStorage(connectionString,
                            new SqlServerStorageOptions
                            {
                                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                QueuePollInterval = TimeSpan.Zero,
                                UseRecommendedIsolationLevel = true,
                                DisableGlobalLocks = true
                            })
                            .UseSerializerSettings(new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }

        public static void RunBackgroundService(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IRecurringJobManager recurringJobManager = serviceProvider.GetService<IRecurringJobManager>();

            //recurringJobManager.AddOrUpdate(nameof(backgroundJobsService.HourlyTask_UpdateBranchCurrencyRates), () => backgroundJobsService.HourlyTask_UpdateBranchCurrencyRates(), Cron.Hourly(), TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
        }

    }
}
