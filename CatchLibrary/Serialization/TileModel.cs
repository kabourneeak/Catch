using System.Runtime.Serialization;
using CatchLibrary.HexGrid;

namespace CatchLibrary.Serialization
{
    [DataContract]
    public class TileModel
    {
        /// <summary>
        /// The row coordinate of this tile
        /// </summary>
        [DataMember]
        public HexCoords Coords { get; set; }

        /// <summary>
        /// The agent to place on this tile
        /// </summary>
        [DataMember]
        public string TowerName { get; set; }
    }
}