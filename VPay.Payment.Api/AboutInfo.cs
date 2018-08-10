namespace VPay.Payment.Api
{
    public class AboutInfo
    {
        // More build variables available upon request

        public string BuildName { get; internal set; }
        public string GitRevision { get; internal set; }
        public string BuildTime { get; internal set; }
        public string Environment { get; internal set; }
        public string VersionInfo { get; internal set; }

        #region For build process
        // Do not modify this region without updating build plan
        public static AboutInfo GetBuildAboutInfo()
        {
            AboutInfo infoForTheBuildPlanToModify = new AboutInfo();
            infoForTheBuildPlanToModify.BuildName = "[[BuildName]]";
            infoForTheBuildPlanToModify.GitRevision = "[[GitRevision]]";
            infoForTheBuildPlanToModify.BuildTime = "[[BuildTime]]";
            infoForTheBuildPlanToModify.Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            infoForTheBuildPlanToModify.VersionInfo = "[[VersionInfo]]";

            return infoForTheBuildPlanToModify;
        }

        #endregion

    }
}
