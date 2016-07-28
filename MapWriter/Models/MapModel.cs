using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MapWriter.Models
{
    [Serializable]
    public class MapModel
    {
        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public int Columns { get; set; }

        [DataMember]
        public List<TileModel> Tiles { get; set; } = new List<TileModel>();

        [DataMember]
        public List<EmitScriptEntryModel> EmitScript { get; set; } = new List<EmitScriptEntryModel>();

        [DataMember]
        public List<PathModel> Paths { get; set; } = new List<PathModel>();

        protected void InitializeTiles(string towerName)
        {
            Tiles.Clear();

            for (var row = 0; row < Rows; ++row)
            {
                for (var col = 0; col < Columns; ++col)
                {
                    Tiles.Add(new TileModel {Row = row, Column = col, TowerName = towerName});
                }
            }
        }

        private bool GetCoordsAreValid(int row, int col)
        {
            if (col >= 0 && col < Columns)
            {
                if (row >= 0 + col.Mod(2) && row < Rows)
                {
                    return true;
                }
            }

            return false;
        }

        private int GetListOffset(int row, int col)
        {
            return (col * Rows) - (col / 2) + (row - col.Mod(2));
        }
    }
}
