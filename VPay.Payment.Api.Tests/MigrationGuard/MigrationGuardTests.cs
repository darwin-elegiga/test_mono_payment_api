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
    /// <summary>
    /// Pins the externally observable behavior of the service host so that a runtime or
    /// package migration cannot change it silently. Everything runs in-process against
    /// TestServer: no socket leaves the test host and no downstream system is contacted.
    /// These tests are expected to stay green on the pre-migration baseline AND after
    /// every migration commit; a red test here means the migration changed behavior.
    /// </summary>
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

                // Console/Debug/GELF providers registered by Program would emit log
                // traffic from inside the tests; keep the run hermetic.
                builder.ConfigureLogging(logging => logging.ClearProviders());
            }

            /// <summary>
            /// Swagger setup reads "{EntryAssembly}.xml" from the base directory
            /// (ServiceCollectionExtensions.AddSwaggerGenService). Under the test runner
            /// the entry assembly is the test host, which ships no XML docs, so the app
            /// would fail to start. Provide an empty, well-formed document instead.
            /// </summary>
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

        // ------------------------------------------------------------------
        // Authorization surface: a migration that drops the authentication or
        // authorization wiring would turn these 401s into 200s or 500s.
        // ------------------------------------------------------------------

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
                // The scheme expects a 4-part payload; two parts must fail cleanly.
                var twoParts = Convert.ToBase64String(Encoding.UTF8.GetBytes("id:passphrase"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", twoParts);

                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        // ------------------------------------------------------------------
        // Anonymous surface: these must remain reachable without credentials.
        // ------------------------------------------------------------------

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

        // ------------------------------------------------------------------
        // Static WSDL content: served from wwwroot through WebRootFileProvider.
        // A migration that changes where static web assets are published breaks
        // these endpoints even though nothing fails at compile time.
        // ------------------------------------------------------------------

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

        // ------------------------------------------------------------------
        // Swagger document: guards two silent failure modes at once - the XML
        // comments file going missing (startup failure) and the API-versioning
        // metadata lookup returning nothing (empty paths section).
        // ------------------------------------------------------------------

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

        // ------------------------------------------------------------------
        // Container wiring: package bumps can silently invalidate registrations.
        // Resolving through a scope constructs the object graph without opening
        // any downstream connection.
        // ------------------------------------------------------------------

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
