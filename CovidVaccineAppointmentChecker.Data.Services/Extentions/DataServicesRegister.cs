using Microsoft.Extensions.DependencyInjection;

namespace CovidVaccineAppointmentChecker.Data.Services.Extentions
{
    public static class DataServicesRegister
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<ISorocabaAppointmentsGateway, Implementations.SorocabaAppointmentsGateway>();
        }
    }
}
