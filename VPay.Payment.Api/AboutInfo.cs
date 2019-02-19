using System;
using Microsoft.Extensions.Configuration;

namespace VPay.Payment.Api
{
    public class AboutInfo
    {
        public string BuildName { get; set; }
        public string GitRevision { get; set; }
        public string BuildTime { get; set; }
        public string Environment { get; set; }
        public string VersionInfo { get; set; }

        public AboutInfo()
        {
            this.BuildName = "undefined";
            this.BuildTime = DateTime.UnixEpoch.ToString("u");
            this.GitRevision = "undefined";
            this.Environment = "undefined";
            this.VersionInfo = "undefined";
        }

        public AboutInfo(IConfiguration configuration)
        {
            this.BuildName = configuration.GetValue<string>("About:BuildName") ?? "[[BuildName]]";
            this.GitRevision = configuration.GetValue<string>("About:GitRevision") ?? "[[GitRevision]]";
            this.BuildTime = DateTime.UnixEpoch.AddMilliseconds(configuration.GetValue<long>("About:BuildTime")).ToString("u") ?? "[[BuildTime]]";
            this.Environment= System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            this.VersionInfo = configuration.GetValue<string>("About:VersionInfo") ?? "[[VersionInfo]]";
        }
    }
}
