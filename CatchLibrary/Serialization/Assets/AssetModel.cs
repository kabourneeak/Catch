using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Assets
{
    /// <summary>
    /// The overall serialization model for an asset file
    /// </summary>
    [DataContract]
    public class AssetModel
    {
        [DataMember]
        public List<ColorModel> Colors { get; set; } = new List<ColorModel>();

        [DataMember]
        public List<StyleModel> Styles { get; set; } = new List<StyleModel>();

        [DataMember]
        public List<ComponentModel> Sprites { get; set; } = new List<ComponentModel>();

        [DataMember]
        public List<ComponentModel> Indicators { get; set; } = new List<ComponentModel>();

        [DataMember]
        public List<ComponentModel> Modifiers { get; set; } = new List<ComponentModel>();

        [DataMember]
        public List<ComponentModel> Behaviours { get; set; } = new List<ComponentModel>();

        [DataMember]
        public List<ComponentModel> Commands { get; set; } = new List<ComponentModel>();

        [DataMember]
        public List<AgentModel> Agents { get; set; } = new List<AgentModel>();
    }
}
