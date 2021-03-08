
using CovidVaccineAppointmentChecker.Data.Services;
using CovidVaccineAppointmentChecker.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccineAppointmentChecker.Core.Services.Implementations
{
    public class AppointmentCheckerService : IAppointmentCheckerService
    {
        private readonly ISorocabaAppointmentsGateway _sorocabaAppoitmentsGateway;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        
        public AppointmentCheckerService(ISorocabaAppointmentsGateway sorocabaAppoitmentsGateway, IEmailService emailService, ILogger<AppointmentCheckerService> logger)
        {
            this._sorocabaAppoitmentsGateway = sorocabaAppoitmentsGateway;
            this._emailService = emailService;
            this._logger = logger;
        }

        public async Task SendAvailbleDatesInSorocabaReport()
        {
            var offices = await this._sorocabaAppoitmentsGateway.GetAppoitmentOffices();

            if (this.SendResumeEmail(offices))
            {
                this._logger.LogInformation("Report by e-mail sent sucessfully");
            }
        }

        private bool SendResumeEmail(IEnumerable<SorocabaAppointmentOffice> offices)
        {
            try
            {
                this._emailService.Send("Relatório de vagas em Sorocaba", this.GenerateEmailBody(offices));
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error sending report by e-mail", new object[0]);
            }

            return false;
        }

        private string GenerateEmailBody(IEnumerable<SorocabaAppointmentOffice> offices)
        {
            StringBuilder stb = new StringBuilder();

            stb.Append("<h1>Relatório de vagas em Sorocaba</h1>");

            if (offices.Any(a => a.HasAvaibleDate()))
            {
                stb.Append("<h3>Há datas disponíveis em pelo menos um escritório!!!</h3>");
            }

            stb.Append($"<ul>");

            foreach (var o in offices)
            {
                stb.Append($"<li>{o.Name} - {o.AvailbleDates.Count()}</li>");
            }

            stb.Append($"</ul>");

            return stb.ToString();
        }
    }
}
