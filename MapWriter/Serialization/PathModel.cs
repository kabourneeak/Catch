using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MapWriter.Serialization
{
    [Serializable]
    public class PathModel
    {
        [DataMember]
        public string PathName { get; set; }

        [DataMember]
        public List<PathStepModel> PathSteps { get; set; } = new List<PathStepModel>();
    }
}