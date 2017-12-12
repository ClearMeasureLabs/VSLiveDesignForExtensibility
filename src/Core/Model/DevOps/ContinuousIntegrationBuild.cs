namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class ContinuousIntegrationBuild : AutomatedBuild
    {
        public StaticAnalysis StaticAnalysis { get; set; }
        public VersionStamp VersionStamp { get; set; }
        public ReleasePackages Packages { get; set; }
    }
}