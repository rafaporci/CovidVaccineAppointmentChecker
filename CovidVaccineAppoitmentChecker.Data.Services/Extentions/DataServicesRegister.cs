using Microsoft.Extensions.DependencyInjection;

namespace CovidVaccineAppoitmentChecker.Data.Services.Extentions
{
    public static class DataServicesRegister
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<ISorocabaAppoitmentsGateway, Implementations.SorocabaAppoitmentsGateway>();
        }
    }
}
