using CovidVaccineAppointmentChecker.Core.Services.Implementations;
using CovidVaccineAppointmentChecker.Data.Services;
using CovidVaccineAppointmentChecker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccineAppointmentChecker.Core.Services.Tests
{
    [TestClass()]
    public class AppointmentCheckerServiceTests
    {
        [TestMethod()]
        public async Task AllOfficesWithoutAvaibleDatesTest()
        {
            // Arrange
            var sorocabaAppoitmentsGatewayMock = BuildSorocabaAppoitmentsGatewayMock(BuildSorocabaOfficesList());
            var emailServiceMock = BuildEmailServiceMock();
            var target = new AppointmentCheckerService(sorocabaAppoitmentsGatewayMock, emailServiceMock.Object, Mock.Of<ILogger<AppointmentCheckerService>>());
            var expected = ReadResource("sorocabaAgendaOfficesWithoutDates.html");

            // Act
            await target.SendAvailbleDatesInSorocabaReport();

            // Assert
            emailServiceMock.Verify(a => a.Send(It.IsAny<string>(), It.Is<string>(m => m == expected)), Times.Once);
        }

        [TestMethod()]
        public async Task AtLeastOneOfficesWithAvaibleDatesTest()
        {
            // Arrange
            var offices = BuildSorocabaOfficesList();
            offices.FirstOrDefault(a => a.Name == "USF São Bento - Vacinação COVID-19").AvailbleDates = new List<string>() { "2020-01-01" };
            var sorocabaAppoitmentsGatewayMock = BuildSorocabaAppoitmentsGatewayMock(offices);
            var emailServiceMock = BuildEmailServiceMock();
            var target = new AppointmentCheckerService(sorocabaAppoitmentsGatewayMock, emailServiceMock.Object, Mock.Of<ILogger<AppointmentCheckerService>>());
            var expected = ReadResource("sorocabaAgendaOfficesAndSaoBentoWithDates.html");
            
            // Act
            await target.SendAvailbleDatesInSorocabaReport();

            // Assert
            emailServiceMock.Verify(a => a.Send(It.IsAny<string>(), It.Is<string>(m => m == expected)), Times.Once);
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

        private ISorocabaAppointmentsGateway BuildSorocabaAppoitmentsGatewayMock(IEnumerable<SorocabaAppointmentOffice> offices)
        {
            var gatewayMock = new Mock<ISorocabaAppointmentsGateway>();
            gatewayMock.Setup(a => a.GetAppoitmentOffices()).ReturnsAsync(offices);
            return gatewayMock.Object;
        }

        private Mock<IEmailService> BuildEmailServiceMock()
        {
            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<string>()));
            return emailServiceMock;
        }

        private string ReadResource(string resourceName)
        {
            Stream resource = typeof(AppointmentCheckerServiceTests).Assembly.GetManifestResourceStream($"CovidVaccineAppointmentChecker.Core.Services.Tests.Resources.{resourceName}");
            return new StreamReader(resource, Encoding.UTF8).ReadToEnd();
        }
    }
}