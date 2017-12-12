namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class AcceptanceStage : PipelineStage
    {
        public AcceptanceStage()
        {
            Name = "2-Deploy to Test";
            Artifacts = new[] {new ReleasePackages()};
            Validations = new ValidationStep[]
            {
                new SmokeTestSuite(),
                new AcceptanceTestSuite(),
                new SystemIntegrationTestSuite(),
                new CapacityTestSuite()
            };
        }
    }
}