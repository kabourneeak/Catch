using System;
using System.Runtime.Serialization;

namespace MapWriter.Serialization
{
    [Serializable]
    public class PathStepModel
    {
        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int Column { get; set; }

        public PathStepModel()
        {
            // empty constructor for construction
        }

        public PathStepModel(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
    }
}