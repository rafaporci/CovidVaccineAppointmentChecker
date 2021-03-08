using Microsoft.Extensions.DependencyInjection;

namespace CovidVaccineAppointmentChecker.Core.Services.Extentions
{
    public static class CoreServicesRegister
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IAppointmentCheckerService, Implementations.AppointmentCheckerService>()
                .AddTransient<IEmailService, Implementations.EmailService>();
        }
    }
}
