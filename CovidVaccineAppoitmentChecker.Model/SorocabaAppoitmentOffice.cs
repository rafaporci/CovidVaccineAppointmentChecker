using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CovidVaccineAppoitmentChecker.Core.Services.Tests")]
[assembly: InternalsVisibleTo("CovidVaccineAppoitmentChecker.Data.Services.Tests")]
namespace CovidVaccineAppoitmentChecker.Model
{
    public class SorocabaAppoitmentOffice
    {
        public SorocabaAppoitmentOffice(string name, IEnumerable<string> availbleDates)
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
