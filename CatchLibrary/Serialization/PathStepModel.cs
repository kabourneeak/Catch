using System.Runtime.Serialization;

namespace CatchLibrary.Serialization
{
    [DataContract]
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

        public PathStepModel(TileModel tileModel)
        {
            this.Row = tileModel.Row;
            this.Column = tileModel.Column;
        }
    }
}