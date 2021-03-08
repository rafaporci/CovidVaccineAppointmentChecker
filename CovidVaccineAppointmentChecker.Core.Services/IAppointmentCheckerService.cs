using System.Threading.Tasks;

namespace CovidVaccineAppointmentChecker.Core.Services
{
    public interface IAppointmentCheckerService
    {
        Task SendAvailbleDatesInSorocabaReport(); 
    }
}
