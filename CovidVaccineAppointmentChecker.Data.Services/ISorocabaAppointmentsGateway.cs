using CovidVaccineAppointmentChecker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccineAppointmentChecker.Data.Services
{
    
    public interface ISorocabaAppointmentsGateway
    {
        Task<IEnumerable<SorocabaAppointmentOffice>> GetAppoitmentOffices();
    }
}
