using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Assets
{
    [DataContract]
    public class AgentModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PrimaryBehaviourName { get; set; }

        [DataMember]
        public List<string> IndicatorNames { get; set; } = new List<string>();

        [DataMember]
        public List<string> ModifierNames { get; set; } = new List<string>();

        [DataMember]
        public List<string> CommandNames { get; set; } = new List<string>();
    }
}