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
        public string PrimaryBehaviour { get; set; }

        [DataMember]
        public List<string> Indicators { get; set; } = new List<string>();

        [DataMember]
        public List<string> Modifiers { get; set; } = new List<string>();

        [DataMember]
        public List<string> Commands { get; set; } = new List<string>();
    }
}