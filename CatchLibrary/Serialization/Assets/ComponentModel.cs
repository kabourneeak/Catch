using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Assets
{
    [DataContract]
    public class ComponentModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Base { get; set; }

        [DataMember]
        public Dictionary<string, string> Config { get; set; } = new Dictionary<string, string>();
    }
}