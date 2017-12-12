using System;

namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public abstract class ValidationStep
    {
        public StepSetup Setup { get; set; }
        public TestExecution Test { get; set; }
        public StepTeardown Teardown { get; set; }
        }
}
