﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CatchLibrary.HexGrid;

namespace CatchLibrary.Serialization.Maps
{
    [DataContract]
    public class MapSerializationModel
    {
        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public int Columns { get; set; }

        /// <summary>
        /// The set of TileModels that make up the map. This list is just a vessel
        /// for serialization, and is only valid after deserialization, use the 
        /// HexGrid for interacting with the TileModels.
        /// </summary>
        [DataMember]
        public List<TileModel> TileList { get; set; } = new List<TileModel>();

        /// <summary>
        /// The primary container of the TileModel objects
        /// </summary>
        [IgnoreDataMember]
        public HexGridCollection<TileModel> Tiles { get; private set; }

        [DataMember]
        public List<EmitScriptEntryModel> EmitScript { get; set; } = new List<EmitScriptEntryModel>();

        [DataMember]
        public List<PathModel> Paths { get; set; } = new List<PathModel>();

        protected void InitializeHexGrid()
        {
            Tiles = new HexGridCollection<TileModel>(Rows, Columns);
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            // rewrite the Tiles list when time to serialize
            TileList = Tiles.ToList();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            InitializeHexGrid();
            foreach (var tileModel in TileList)
            {
                Tiles.SetHex(tileModel.Coords, tileModel);
            }
        }
    }
}
