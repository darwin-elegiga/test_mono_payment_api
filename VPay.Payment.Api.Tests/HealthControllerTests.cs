using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VPay.Payment.Api.Controllers;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Api.Tests
{
    public class HealthControllerTests
    {

        [Fact]
        public async Task CheckHealth_WithAllServicesOK_ReturnsOkResult()
        {
            //Arrange
            var param = new List<ServiceComponentStatus>
            {
                new ServiceComponentStatus
                {
                    Component = "COMP1",
                    Status = "OK"
                },
                new ServiceComponentStatus
                {
                    Component = "COMP2",
                    Status = "OK"
                }
            };

            var pdfMock = new Mock<IHealthCheckService>();
            pdfMock.Setup(x => x.CheckHealth()).ReturnsAsync(param).Verifiable();
            var controller = new HealthController(pdfMock.Object);


            //Act
            var result = await controller.CheckHealth();

            //Assert
            pdfMock.VerifyAll();

            result.Should().BeOfType<ActionResult<List<ServiceComponentStatus>>>()
                .Which.Value.Should().BeEquivalentTo(param);
        }


        [Fact]
        public async Task CheckHealth_WithAtLeastServicesNotOk_Returns500ErrorResult()
        {
            //Arrange
            var param = new List<ServiceComponentStatus>
            {
                new ServiceComponentStatus
                {
                    Component = "COMP1",
                    Status = "FALIURE"
                },
                new ServiceComponentStatus
                {
                    Component = "COMP2",
                    Status = "OK"
                }
            };

            var pdfMock = new Mock<IHealthCheckService>();
            pdfMock.Setup(x => x.CheckHealth()).ReturnsAsync(param).Verifiable();
            var controller = new HealthController(pdfMock.Object);


            //Act
            var result = await controller.CheckHealth();

            //Assert
            pdfMock.VerifyAll();

            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);


            result.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeEquivalentTo(param);
        }
    }
}
