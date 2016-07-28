using System;
using System.Runtime.Serialization;

namespace MapWriter.Models
{
    [Serializable]
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
    }
}