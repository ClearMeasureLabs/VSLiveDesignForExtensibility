namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class StagingStage : PipelineStage
    {
        public StagingStage()
        {
            Name = "3-Promote to Staging";
            Artifacts = new[] {new ReleasePackages()};
            Validations = new ValidationStep[]
            {
                new InPlaceUpgrade(),
                new SmokeTestSuite()
            };
        }
    }
}