using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradingPostController : ControllerBase
    {

        private readonly IHttpContextAccessor _accessor;
        private readonly IFileProvider _fileProvider;

        private readonly ILogger _logger;

        public TradingPostController(IHostingEnvironment fileProvider, IHttpContextAccessor accessor, ILogger<LegacyController> logger)
        {
            _fileProvider = fileProvider.WebRootFileProvider;
            _accessor = accessor;
            _logger = logger;

        }

        [HttpGet("wsdl")]
        [Produces("text/xml")]
        [AllowAnonymous]
        public IActionResult GetWsdl([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                var request = _accessor.HttpContext.Request;
                var location = new Uri($"{request.Scheme}://{request.Host}{request.Path}");

                url = location.AbsoluteUri;
            }
            else
            {
                var uri = new Uri(url);
                url = uri.GetLeftPart(UriPartial.Path);
            }
            var doc = XDocument.Load(_fileProvider.GetFileInfo("SEcureCardServices.xml").PhysicalPath);
            XNamespace nsSoap = "http://schemas.xmlsoap.org/wsdl/soap/";
            XNamespace nsSoap12 = "http://schemas.xmlsoap.org/wsdl/soap12/";

            var addressElement = doc.Descendants(nsSoap + "address").ToList();

            foreach (var xElement in addressElement)
            {
                var location = xElement.Attribute("location");
                if (location != null)
                {
                    location.Value = url;
                }
            }

            addressElement = doc.Descendants(nsSoap12 + "address").ToList();

            foreach (var xElement in addressElement)
            {
                var location = xElement.Attribute("location");
                if (location != null)
                {
                    location.Value = url;
                }
            }

            var settings = new XmlWriterSettings { OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using (var memoryStream = new MemoryStream())
            using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                return File(memoryStream.ToArray(), "text/xml");
            }
        }

    }
}
