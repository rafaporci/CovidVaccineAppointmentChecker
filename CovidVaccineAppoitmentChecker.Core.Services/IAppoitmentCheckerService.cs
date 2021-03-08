using System.Threading.Tasks;

namespace CovidVaccineAppoitmentChecker.Core.Services
{
    public interface IAppoitmentCheckerService
    {
        Task SendAvailbleDatesInSorocabaReport(); 
    }
}
