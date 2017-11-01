using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Assets
{
    [DataContract]
    public class ColorModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ColorHex { get; set; }
    }
}
