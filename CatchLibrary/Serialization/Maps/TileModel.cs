using System.Runtime.Serialization;
using CatchLibrary.HexGrid;

namespace CatchLibrary.Serialization.Maps
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

        /// <summary>
        /// The team of the agent which is placed on this tile
        /// </summary>
        [DataMember]
        public int Team { get; set; }
    }
}