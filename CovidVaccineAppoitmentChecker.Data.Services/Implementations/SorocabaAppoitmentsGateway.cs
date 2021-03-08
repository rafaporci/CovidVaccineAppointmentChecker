using CovidVaccineAppoitmentChecker.Model;
using GuardNet;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidVaccineAppoitmentChecker.Data.Services.Implementations
{
    public class SorocabaAppoitmentsGateway : ISorocabaAppoitmentsGateway
    {
        private readonly HttpClient _httpClient;

        public SorocabaAppoitmentsGateway(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IEnumerable<SorocabaAppoitmentOffice>> GetAppoitmentOffices()
        {
            var rawResponse = await this._httpClient.GetStringAsync("https://servicos.sorocaba.sp.gov.br/agenda_servicos/api/unidade/3/agenda/");

            Guard.NotNullOrEmpty(rawResponse, "SorocabAgenda");

            Agenda agenda = JsonConvert.DeserializeObject<Agenda>(rawResponse);

            return agenda.setores.Select(a => new SorocabaAppoitmentOffice(a.nome, a.datas));
        }

        public class Setor
        {
            public int id { get; set; }
            public string nome { get; set; }
            public string observacao { get; set; }
            public List<string> datas { get; set; }
        }

        public class Agenda
        {
            public int id { get; set; }
            public string nome { get; set; }
            public string sigla { get; set; }
            public IEnumerable<Setor> setores { get; set; }
        }
    }
}
