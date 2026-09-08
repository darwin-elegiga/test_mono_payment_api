using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Api.Tests.MigrationGuard
{
    [Trait("Category", "MigrationGuard")]
    public class MigrationGuardTests : IClassFixture<MigrationGuardTests.GuardFactory>
    {
        private readonly GuardFactory _factory;

        public MigrationGuardTests(GuardFactory factory)
        {
            _factory = factory;
        }

        public class GuardFactory : WebApplicationFactory<Program>
        {
            public GuardFactory()
            {
                EnsureEntryAssemblyXmlDocExists();
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.UseEnvironment("Development");

                builder.ConfigureLogging(logging => logging.ClearProviders());
            }

            private static void EnsureEntryAssemblyXmlDocExists()
            {
                var name = Assembly.GetEntryAssembly()?.GetName().Name;
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }

                var path = Path.Combine(AppContext.BaseDirectory, name + ".xml");
                if (!File.Exists(path))
                {
                    File.WriteAllText(
                        path,
                        "<?xml version=\"1.0\"?><doc><assembly><name>" + name + "</name></assembly><members></members></doc>");
                }
            }
        }

        [Theory]
        [InlineData("GET", "/api/Main/PanNumber?transNumber=1")]
        [InlineData("GET", "/api/Main/TransactionDetails?transNumber=1")]
        [InlineData("GET", "/api/Main/ReasonCodes?transNumber=1")]
        [InlineData("GET", "/api/Main/BalanceRequest?transNumber=1")]
        [InlineData("POST", "/api/Legacy/GetPanNumber")]
        [InlineData("POST", "/api/Legacy/GetTransactionDetails")]
        [InlineData("POST", "/api/Legacy/LoadPan")]
        public async Task Protected_endpoint_without_credentials_returns_401(string method, string url)
        {
            using (var client = _factory.CreateClient())
            using (var request = BuildRequest(method, url))
            {
                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(
                    HttpStatusCode.Unauthorized,
                    "authorization must challenge before any controller logic runs");
            }
        }

        [Fact]
        public async Task Credentials_with_wrong_segment_count_are_rejected_with_401()
        {
            using (var client = _factory.CreateClient())
            using (var request = BuildRequest("GET", "/api/Main/PanNumber?transNumber=1"))
            {
                var twoParts = Convert.ToBase64String(Encoding.UTF8.GetBytes("id:passphrase"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", twoParts);

                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [Theory]
        [InlineData("GET", "/api/about")]
        [InlineData("GET", "/api/Legacy/version")]
        [InlineData("GET", "/api/Legacy/xsd")]
        [InlineData("POST", "/api/Legacy/echo")]
        public async Task Anonymous_endpoint_stays_reachable_without_credentials(string method, string url)
        {
            using (var client = _factory.CreateClient())
            using (var request = BuildRequest(method, url))
            {
                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Theory]
        [InlineData("/api/Legacy/wsdl")]
        [InlineData("/api/TradingPost/wsdl")]
        public async Task Wsdl_documents_are_served_from_static_content(string url)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.GetAsync(url);

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var body = await response.Content.ReadAsStringAsync();
                body.Should().Contain("definitions", "the endpoint must return the WSDL document");
            }
        }

        [Fact]
        public async Task Swagger_document_lists_the_versioned_api_surface()
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.GetAsync("/swagger/v1.0/swagger.json");

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var document = JObject.Parse(await response.Content.ReadAsStringAsync());
                var paths = document["paths"] as JObject;

                paths.Should().NotBeNull();
                paths.Properties().Should().NotBeEmpty(
                    "an empty paths section means the version metadata lookup broke");
                paths.ContainsKey("/api/Main/PanNumber").Should().BeTrue();
                paths.ContainsKey("/api/Legacy/GetPanNumber").Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(typeof(IAuthService))]
        [InlineData(typeof(ITransactionService))]
        [InlineData(typeof(ILegacyTransactionService))]
        [InlineData(typeof(ILegacyValidationService))]
        [InlineData(typeof(ITradingPostService))]
        [InlineData(typeof(IUserInfo))]
        public void Core_service_resolves_from_the_container(Type serviceType)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService(serviceType);

                service.Should().NotBeNull();
            }
        }

        private static HttpRequestMessage BuildRequest(string method, string url)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), url);
            if (method == "POST")
            {
                request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}
