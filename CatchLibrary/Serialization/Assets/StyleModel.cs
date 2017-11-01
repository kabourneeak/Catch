using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Assets
{
    [DataContract]
    public class StyleModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ColorName { get; set; }

        [DataMember]
        public string ColorHex { get; set; }

        [DataMember]
        public string BrushType { get; set; }

        [DataMember]
        public float BrushOpacity { get; set; }

        [DataMember]
        public int StrokeWidth { get; set; }
    }
}
