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
            // empty constructor
        }

        public PathStepModel(HexCoords hc)
        {
            Coords = hc;
        }

        public PathStepModel(TileModel tileModel)
        {
            Coords = tileModel.Coords;
        }
    }
}