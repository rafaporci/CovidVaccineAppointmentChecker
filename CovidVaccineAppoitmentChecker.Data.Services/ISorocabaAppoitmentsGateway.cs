using CovidVaccineAppoitmentChecker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccineAppoitmentChecker.Data.Services
{
    
    public interface ISorocabaAppoitmentsGateway
    {
        Task<IEnumerable<SorocabaAppoitmentOffice>> GetAppoitmentOffices();
    }
}
