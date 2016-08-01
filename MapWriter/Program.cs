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
        }

        private static void SaveMap(MapModel map)
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
            var mapModel = JsonConvert.DeserializeObject<MapModel>(File.ReadAllText(filename));
        }
    }
}
