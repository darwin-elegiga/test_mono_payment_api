using System.Threading.Tasks;
using VPay.Payment.Api.Controllers;
using Xunit;

namespace VPay.Payment.Api.Tests.Controllers
{
    public class AboutControllerTests
    {
        [Fact]
        public void WhenCalled_OkReturnedWithAboutInfoData()
        {
            var controller = new AboutController();
            var aboutInfo = controller.GetDetail();

            Assert.NotNull(aboutInfo);
            Assert.Equivalent(AboutInfo.GetBuildAboutInfo(), aboutInfo);
        }
    }
}
