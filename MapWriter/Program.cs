using System.Collections.Generic;
using System.IO;
using CatchLibrary.Serialization;
using MapWriter.Maps;
using Newtonsoft.Json;

namespace MapWriter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SaveMap(new MapOne());
            // SaveMap( another map instance );
            // etc

            var stylePack = new StylePackModel();

            stylePack.Colors.Add(new ColorModel
            {
                Name = "PathTileColor",
                B = 0xFF
            });

            stylePack.Styles.Add(new StyleModel
            {
                Name = "PathTileStyle",
                BrushType = "Solid",
                BrushOpacity = 1.0f,
                StrokeWidth = 1,
                ColorName = "PathTileColor"
            });

            SaveStylePack(stylePack);
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

        private static void SaveStylePack(StylePackModel stylePack)
        {
            var filename = "styles.json";

            using (var fs = File.Open(filename, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                var serializer = new JsonSerializer();

                serializer.Serialize(jw, stylePack);
            }

            // test deserialization
            var deserializedStylePack = JsonConvert.DeserializeObject<StylePackModel>(File.ReadAllText(filename));
        }
    }
}
