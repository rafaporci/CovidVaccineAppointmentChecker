using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CovidVaccineAppointmentChecker.Core.Services.Tests")]
[assembly: InternalsVisibleTo("CovidVaccineAppointmentChecker.Data.Services.Tests")]
namespace CovidVaccineAppointmentChecker.Model
{
    public class SorocabaAppointmentOffice
    {
        public SorocabaAppointmentOffice(string name, IEnumerable<string> availbleDates)
        {
            this.AvailbleDates = availbleDates;
            this.Name = name;
        }

        public string Name { get; internal set; }

        public IEnumerable<string> AvailbleDates { get; internal set; }

        public bool HasAvaibleDate() 
        {
            return AvailbleDates.Any();
        }
    }

}
