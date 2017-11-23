using System.Threading;
using System.Xml.Schema;

namespace ClearMeasure.Bootcamp.Core.Model.DevOps
{
    public abstract class PipelineStage
    {
        public string Name { get; set; }
        public Artifact[] Artifacts { get; set; }
        public ValidationStep[] Validations { get; set; }
        public bool Succeeded { get; set; }
    }
}

    

    

    