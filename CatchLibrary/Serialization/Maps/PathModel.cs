using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatchLibrary.Serialization.Maps
{
    [DataContract]
    public class PathModel
    {
        [DataMember]
        public string PathName { get; set; }

        [DataMember]
        public List<PathStepModel> PathSteps { get; set; } = new List<PathStepModel>();
    }
}