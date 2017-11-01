using System.IO;
using CatchLibrary.Serialization.Assets;
using CatchLibrary.Serialization.Maps;
using MapWriter.Maps;
using Newtonsoft.Json;

namespace MapWriter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SaveMap(new MapOne());

            ReadAssetModel();
        }

        private static void ReadAssetModel()
        {
            var filename = "assets.json";

            var assetModel = JsonConvert.DeserializeObject<AssetModel>(File.ReadAllText(filename));

        }

        private static void SaveMap(MapSerializationModel map)
        {
            var filename = map.GetType().Name + ".json";

            using (var fs = File.Open(filename, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                var serializer = new JsonSerializer();

                serializer.Serialize(jw, map);
            }

            // test deserialization
            var mapModel = JsonConvert.DeserializeObject<MapSerializationModel>(File.ReadAllText(filename));
        }
    }
}
