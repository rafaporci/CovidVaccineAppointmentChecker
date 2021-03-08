using CovidVaccineAppoitmentChecker.Core.Services;
using CovidVaccineAppoitmentChecker.Core.Services.Extentions;
using CovidVaccineAppoitmentChecker.Data.Services.Extentions;
using CovidVaccineAppoitmentChecker.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace CovidVaccineAppoitmentChecker
{
    class Program
    {
        static int Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();

                using (serviceProvider)
                {
                    serviceProvider.GetRequiredService<IAppoitmentCheckerService>().SendAvailbleDatesInSorocabaReport().Wait();
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var config = MapSettings(serviceCollection);

            serviceCollection.AddLogging(loggingBuilder =>
             {
                 loggingBuilder.ClearProviders();
                 loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                 loggingBuilder.AddNLog(config);
             });

            RegisterSmtpService(serviceCollection, config);
            serviceCollection.AddTransient<HttpClient>();
            serviceCollection.RegisterCoreServices();
            serviceCollection.RegisterDataServices();
        }

        private static IConfiguration MapSettings(IServiceCollection serviceCollection)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            return configuration;
        }

        private static IConfiguration RegisterSmtpService(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var emailSetting = new EmailSettings();
            configuration.GetSection("Email").Bind(emailSetting);

            serviceCollection.AddSingleton(emailSetting);

            var smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = emailSetting.SmtpServer.UseDefaultCredentials;
            smtp.EnableSsl = emailSetting.SmtpServer.EnableSsl;
            smtp.Host = emailSetting.SmtpServer.Host;
            smtp.Port = emailSetting.SmtpServer.Port;
            smtp.Credentials = new NetworkCredential(emailSetting.SmtpServer.UserName, emailSetting.SmtpServer.Password);

            serviceCollection.AddSingleton(smtp);

            return configuration;
        }
    }
}
