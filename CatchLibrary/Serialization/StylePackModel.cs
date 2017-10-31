using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CatchLibrary.Serialization
{
    [DataContract]
    public class StylePackModel
    {
        [DataMember]
        public List<ColorModel> Colors { get; set; } = new List<ColorModel>();

        [DataMember]
        public List<StyleModel> Styles { get; set; } = new List<StyleModel>();
    }
}
