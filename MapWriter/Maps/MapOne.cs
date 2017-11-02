using CatchLibrary.HexGrid;
using CatchLibrary.Serialization.Maps;

namespace MapWriter.Maps
{
    public class MapOne : MapSerializationModel
    {
        public MapOne()
        {
            Rows = 10;
            Columns = 19;
            InitializeHexGrid();

            AddTiles();
            AddPaths();
            AddScript();
        }

        private void AddTiles()
        {
            Tiles.Populate((hc, v) => new TileModel {Coords = hc, TowerName = "SocketTower", Team = 0});

            Tiles.GetHex(HexCoords.CreateFromOffset(4, 5)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(5, 5)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(4, 4)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(6, 5)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(5, 6)).TowerName = "GunTower";

            Tiles.GetHex(HexCoords.CreateFromOffset(4, 15)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(5, 15)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(4, 14)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(6, 15)).TowerName = "GunTower";
            Tiles.GetHex(HexCoords.CreateFromOffset(5, 16)).TowerName = "GunTower";

            Tiles.GetHex(HexCoords.CreateFromOffset(2, 1)).TowerName = "GunTower";
        }

        private void AddPaths()
        {
            var testPath = new PathModel();
            {
                testPath.PathName = nameof(testPath);

                var hex = Tiles.GetHex(HexCoords.CreateFromAxial(0, 0));
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.SouthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.SouthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour(hex.Coords, HexDirection.SouthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                Paths.Add(testPath);
            }
        }

        private void AddScript()
        {
            EmitScript.Add(new EmitScriptEntryModel
            {
                BeginTime = 120,
                Count = 10,
                DelayTime = 60,
                PathName = "testPath",
                AgentTypeName = "BlockMob",
                Team = 1
            });
        }
    }
}
