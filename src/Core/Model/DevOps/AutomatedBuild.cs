namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public class AutomatedBuild : ValidationStep
    {
        public DatabaseMigration DatabaseMigration { get; set; }
        public Compile Compile { get; set; }
        public TestSuite UnitTestSuite { get; set; }
        public TestSuite ComponentIntegrationTestSuite { get; set; }
    }
}