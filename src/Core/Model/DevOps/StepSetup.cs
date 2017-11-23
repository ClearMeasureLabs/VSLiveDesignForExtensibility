namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class StepSetup
    {
        public ValidationEnvironment Environment { get; set; }
        public Deployment Deployment { get; set; }
        public KnownDataSet DataSet { get; set; }
    }
}