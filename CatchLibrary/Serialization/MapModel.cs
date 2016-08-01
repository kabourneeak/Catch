using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatchLibrary.Serialization
{
    [DataContract]
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

            // TODO call populate
        }
    }
}
