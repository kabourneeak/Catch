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
        public int Row { get; set; }

        /// <summary>
        /// The column coordinate of this tile
        /// </summary>
        [DataMember]
        public int Column { get; set; }

        /// <summary>
        /// The agent to place on this tile
        /// </summary>
        [DataMember]
        public string TowerName { get; set; }

        public static explicit operator HexCoords(TileModel t)
        {
            return new HexCoords {Row = t.Row, Column = t.Column};
        }
    }
}