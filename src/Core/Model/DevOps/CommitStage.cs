namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class CommitStage : PipelineStage
    {
        public CommitStage()
        {
            Name = "1-Integration-Build-hosted";
            Artifacts = new[] {new SourceRepository()};
            Validations = new ValidationStep[]
            {
                new PrivateBuild(),
                new ContinuousIntegrationBuild()
            };
        }
    }
}