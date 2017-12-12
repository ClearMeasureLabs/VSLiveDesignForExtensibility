namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class ProductionStage : PipelineStage
    {
        public ProductionStage()
        {
            Name = "3-Promote to Production";
            Artifacts = new[] { new ReleasePackages() };
            Validations = new ValidationStep[]
            {
                new SuccessfulBackup(),
                new InPlaceUpgrade(),
                new SmokeTestSuite()
            };
        }
    }
}