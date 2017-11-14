using System;
using Catch.Base;
using Catch.Components;
using Catch.Services;
using CatchLibrary.Serialization.Maps;

namespace Catch.Map
{
    public class MapLoader
    {
        private readonly IConfig _config;
        private readonly IIndicatorProvider _indicatorProvider;
        private readonly ISimulationManager _simulationManager;

        public MapLoader(IConfig config, IIndicatorProvider indicatorProvider, ISimulationManager simulationManager)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _indicatorProvider = indicatorProvider ?? throw new ArgumentNullException(nameof(indicatorProvider));
            _simulationManager = simulationManager ?? throw new ArgumentNullException(nameof(simulationManager));
        }

        public void InitializeMap(MapModel mapModel, MapSerializationModel serializationModel)
        {
            CreateMapTileModels(mapModel, serializationModel);

            ConfigureMapTiles(mapModel, serializationModel);

            ConfigurePaths(mapModel, serializationModel);

            InitializeEmitScript(mapModel, serializationModel);
        }

        private void CreateMapTileModels(MapModel mapModel, MapSerializationModel serializationModel)
        {
            mapModel.Initialize(serializationModel.Rows, serializationModel.Columns);
            mapModel.Populate((hc, v) =>  new MapTileModel(hc, _config, _indicatorProvider));
        }

        private void ConfigureMapTiles(MapModel mapModel, MapSerializationModel serializationModel)
        {
            foreach (var tileModel in serializationModel.TileList)
            {
                var mapTile = mapModel.GetTileModel(tileModel.Coords);

                foreach (var indicatorName in tileModel.IndicatorNames)
                {
                    var indicator = _indicatorProvider.GetIndicator(indicatorName, mapTile);
                    indicator.Position = mapTile.Position;
                    mapTile.Indicators.Add(indicator);
                }

                if (!string.IsNullOrWhiteSpace(tileModel.TowerName))
                {
                    var agentArgs = new CreateAgentArgs()
                    {
                        Tile = mapTile,
                        Team = tileModel.Team
                    };

                    var agent = _simulationManager.CreateAgent(tileModel.TowerName, agentArgs);
                    _simulationManager.Register(agent);
                    _simulationManager.Site(agent);
                }
            }            
        }

        private void ConfigurePaths(MapModel mapModel, MapSerializationModel serializationModel)
        {
            foreach (var pathModel in serializationModel.Paths)
            {
                var mapPath = new MapPathModel();
                mapPath.Name = pathModel.PathName;

                foreach (var pathStep in pathModel.PathSteps)
                {
                    var mapTileModel = mapModel.GetTileModel(pathStep.Coords);
                    mapPath.Add(mapTileModel);
                }

                mapModel.AddPath(mapPath);
            }
        }

        private void InitializeEmitScript(MapModel mapModel, MapSerializationModel serializationModel)
        {
            foreach (var emitScriptEntry in serializationModel.EmitScript)
            {
                for (var i = 0; i < emitScriptEntry.Count; ++i)
                {
                    var agentArgs = new CreateAgentArgs()
                    {
                        Path = mapModel.GetPath(emitScriptEntry.PathName),
                        Tile = mapModel.OffMapTileModel,
                        Team = emitScriptEntry.Team
                    };

                    var offset = emitScriptEntry.BeginTime + (i * emitScriptEntry.DelayTime);
                    var task = new SpawnAgentTask(offset, emitScriptEntry.AgentTypeName, agentArgs);

                    _simulationManager.Register(task);
                }
            }
        }
    }
}
