using CovidVaccineAppointmentChecker.Data.Services.Implementations;
using CovidVaccineAppointmentChecker.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CovidVaccineAppointmentChecker.Data.Services.Tests
{
    [TestClass()]
    public class SorocabaAppoitmentsGatewayTests
    {
        [TestMethod()]
        public async Task AllOfficesWithoutAvaibleDatesTest()
        {
            // Arrange
            var httpClientMock = this.BuildHttpClientMock("sorocabaAgendaOfficesWithoutDates.json");
            var target = new SorocabaAppointmentsGateway(httpClientMock);
            var expected = BuildSorocabaOfficesList();

            // Act
            var result = await target.GetAppoitmentOffices();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod()]
        public async Task AtLeastOneOfficesWithAvaibleDatesTest()
        {
            // Arrange
            var httpClientMock = this.BuildHttpClientMock("sorocabaAgendaOfficesAndSaoBentoWithDates.json");
            var target = new SorocabaAppointmentsGateway(httpClientMock);
            var expected = BuildSorocabaOfficesList();

            expected.FirstOrDefault(a => a.Name == "USF São Bento - Vacinação COVID-19").AvailbleDates = new List<string>() { "2020-01-01" };

            // Act
            var result = await target.GetAppoitmentOffices();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private List<SorocabaAppointmentOffice> BuildSorocabaOfficesList()
        {
            var list = new List<SorocabaAppointmentOffice>();

            list.Add(new SorocabaAppointmentOffice("UBS Éden - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppointmentOffice("UBS Maria do Carmo - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppointmentOffice("USF São Bento - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppointmentOffice("USF Paineiras - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppointmentOffice("UBS Sorocaba I - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppointmentOffice("Drive Thru - Instituto Humberto de Campos", Enumerable.Empty<string>()));

            return list;
        }

        private HttpClient BuildHttpClientMock(string resourceName)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(this.ReadResource(resourceName)),
               })
               .Verifiable();

            return new HttpClient(handlerMock.Object);
        }

        private string ReadResource(string resourceName)
        {
            Stream resource = typeof(SorocabaAppoitmentsGatewayTests).Assembly.GetManifestResourceStream($"CovidVaccineAppointmentChecker.Data.Services.Tests.Resources.{resourceName}");
            return new StreamReader(resource, Encoding.UTF8).ReadToEnd();
        }
    }
}