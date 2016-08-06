using System.Runtime.Serialization;
using CatchLibrary.HexGrid;

namespace CatchLibrary.Serialization
{
    [DataContract]
    public class PathStepModel
    {
        [DataMember]
        public HexCoords Coords { get; set; }

        public PathStepModel()
        {
            // empty constructor for construction
        }

        public PathStepModel(HexCoords hc)
        {
            this.Coords = hc;
        }

        public PathStepModel(TileModel tileModel)
        {
            this.Coords = tileModel.Coords;
        }
    }
}