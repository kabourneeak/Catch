using System;
using System.IO;
using Catch.Services;
using CatchLibrary.Serialization;
using Newtonsoft.Json;

namespace Catch
{
    /// <summary>
    /// Takes a running, but empty, environment and gets the game started
    /// </summary>
    public class Bootstrapper
    {
        public static readonly string CfgMapsFolder = ConfigUtils.GetConfigPath(nameof(Bootstrapper), nameof(CfgMapsFolder));
        public static readonly string CfgInitialMap = ConfigUtils.GetConfigPath(nameof(Bootstrapper), nameof(CfgInitialMap));

        private IConfig Config { get; }

        public Bootstrapper()
        {
            Config = new JsonConfig("config.json");
        }

        public void BeginGame(IScreenManager screenManager)
        {
            var mapModel = LoadMapModel(Config.GetString(CfgMapsFolder), Config.GetString(CfgInitialMap));

            var levelController = new LevelController(Config, mapModel);

            screenManager.RequestScreen(levelController);
        }

        private MapSerializationModel LoadMapModel(string mapFolder, string mapName)
        {
            try
            {
                var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var mapsFolder =
                    appInstalledFolder.GetFolderAsync(mapFolder)
                        .GetAwaiter()
                        .GetResult();

                var initialMapPath = Path.Combine(mapsFolder.Path, mapName);
                var initialMapData = File.ReadAllText(initialMapPath);

                var mapModel = JsonConvert.DeserializeObject<MapSerializationModel>(initialMapData);

                return mapModel;
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new IOException($"Could not read map {mapFolder}/{mapName}", e);
            }
        }
    }
}
