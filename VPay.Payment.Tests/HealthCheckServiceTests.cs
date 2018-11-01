using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Tests
{
    public class HealthCheckServiceTests
    {
        
        [Fact]
        public async Task CheckHealth_WithSingleComponentThatReturnsTrue_WillReturnOkForThatComponent()
        {
            var item = CreateHealthCheckComponent("COMP1", false);

            var ops = new HealthCheckService(new List<IHealthCheck> { item.Item1.Object });

            var expected = new List<ServiceComponentStatus>()
            {
                item.Item2
            };

            var actual = await ops.CheckHealth();

            actual.Should().BeEquivalentTo(expected);
            item.Item1.Verify();
        }


        [Fact]
        public async Task CheckHealth_WithSingleComponentThatReturnsFalse_WillReturnFailureForThatComponent()
        {
            var item = CreateHealthCheckComponent("COMP1", true);

            var ops = new HealthCheckService(new List<IHealthCheck> { item.Item1.Object });

            var expected = new List<ServiceComponentStatus>()
            {
                item.Item2
            };

            var actual = await ops.CheckHealth();

            actual.Should().BeEquivalentTo(expected);
            item.Item1.Verify();
        }


        [Fact]
        public async Task CheckHealth_WithMultipleComponent_WillReturnCorrectValuesForEachComponent()
        {
            var item1 = CreateHealthCheckComponent("COMP1", true);
            var item2 = CreateHealthCheckComponent("COMP2", false);
            var item3 = CreateHealthCheckComponent("COMP3", false);


            var component = new Mock<IHealthCheck>();

            var ops = new HealthCheckService(new List<IHealthCheck> { item1.Item1.Object, item2.Item1.Object, item3.Item1.Object });

            component.Setup(x => x.IsHealthy()).ReturnsAsync(false).Verifiable();
            component.Setup(x => x.Component).Returns("FAILING").Verifiable();

            var expected = new List<ServiceComponentStatus>()
            {
                item1.Item2,
                item2.Item2,
                item3.Item2
            };

            var actual = await ops.CheckHealth();

            actual.Should().BeEquivalentTo(expected);
            item1.Item1.Verify();
            item2.Item1.Verify();
            item3.Item1.Verify();
        }


        private Tuple<Mock<IHealthCheck>, ServiceComponentStatus> CreateHealthCheckComponent(string name,
            bool isPassing)
        {
            var component = new Mock<IHealthCheck>();

            component.Setup(x => x.IsHealthy()).ReturnsAsync(isPassing).Verifiable();
            component.Setup(x => x.Component).Returns(name).Verifiable();

            var statusResult = new ServiceComponentStatus
            {
                Component = name,
                Status = isPassing ? "OK" : "FAILURE"
            };

            return new Tuple<Mock<IHealthCheck>, ServiceComponentStatus>(component, statusResult);
        }
    }
}
