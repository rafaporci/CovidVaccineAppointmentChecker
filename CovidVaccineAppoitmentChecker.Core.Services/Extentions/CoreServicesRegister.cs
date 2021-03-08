using Microsoft.Extensions.DependencyInjection;

namespace CovidVaccineAppoitmentChecker.Core.Services.Extentions
{
    public static class CoreServicesRegister
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IAppoitmentCheckerService, Implementations.AppoitmentCheckerService>()
                .AddTransient<IEmailService, Implementations.EmailService>();
        }
    }
}
