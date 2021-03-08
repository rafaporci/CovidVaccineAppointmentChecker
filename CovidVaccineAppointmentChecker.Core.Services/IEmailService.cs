namespace CovidVaccineAppointmentChecker.Core.Services
{
    public interface IEmailService
    {
        bool Send(string subject, string body);
    }
}
