using CovidVaccineAppoitmentChecker.Core.Services.Implementations;
using CovidVaccineAppoitmentChecker.Data.Services;
using CovidVaccineAppoitmentChecker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccineAppoitmentChecker.Core.Services.Tests
{
    [TestClass()]
    public class AppoitmentCheckerServiceTests
    {
        [TestMethod()]
        public async Task AllOfficesWithoutAvaibleDatesTest()
        {
            // Arrange
            var sorocabaAppoitmentsGatewayMock = BuildSorocabaAppoitmentsGatewayMock(BuildSorocabaOfficesList());
            var emailServiceMock = BuildEmailServiceMock();
            var target = new AppoitmentCheckerService(sorocabaAppoitmentsGatewayMock, emailServiceMock.Object, Mock.Of<ILogger>());
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
            var target = new AppoitmentCheckerService(sorocabaAppoitmentsGatewayMock, emailServiceMock.Object, Mock.Of<ILogger>());
            var expected = ReadResource("sorocabaAgendaOfficesAndSaoBentoWithDates.html");
            
            // Act
            await target.SendAvailbleDatesInSorocabaReport();

            // Assert
            emailServiceMock.Verify(a => a.Send(It.IsAny<string>(), It.Is<string>(m => m == expected)), Times.Once);
        }

        private List<SorocabaAppoitmentOffice> BuildSorocabaOfficesList()
        {
            var list = new List<SorocabaAppoitmentOffice>();

            list.Add(new SorocabaAppoitmentOffice("UBS Éden - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppoitmentOffice("UBS Maria do Carmo - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppoitmentOffice("USF São Bento - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppoitmentOffice("USF Paineiras - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppoitmentOffice("UBS Sorocaba I - Vacinação COVID-19", Enumerable.Empty<string>()));
            list.Add(new SorocabaAppoitmentOffice("Drive Thru - Instituto Humberto de Campos", Enumerable.Empty<string>()));

            return list;
        }

        private ISorocabaAppoitmentsGateway BuildSorocabaAppoitmentsGatewayMock(IEnumerable<SorocabaAppoitmentOffice> offices)
        {
            var gatewayMock = new Mock<ISorocabaAppoitmentsGateway>();
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
            Stream resource = typeof(AppoitmentCheckerServiceTests).Assembly.GetManifestResourceStream($"CovidVaccineAppoitmentChecker.Core.Services.Tests.Resources.{resourceName}");
            return new StreamReader(resource, Encoding.UTF8).ReadToEnd();
        }
    }
}