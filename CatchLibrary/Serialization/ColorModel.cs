using System.Runtime.Serialization;

namespace CatchLibrary.Serialization
{
    [DataContract]
    public class ColorModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte A { get; set; } = 0xFF;

        [DataMember]
        public byte R { get; set; }

        [DataMember]
        public byte G { get; set; }

        [DataMember]
        public byte B { get; set; }
    }
}
