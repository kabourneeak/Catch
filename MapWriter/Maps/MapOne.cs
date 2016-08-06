﻿using CatchLibrary.HexGrid;
using CatchLibrary.Serialization;

namespace MapWriter.Maps
{
    public class MapOne : MapModel
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
            Tiles.Populate((hc, v) => new TileModel {Coords = hc, TowerName = "VoidTower"});

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

                hex = Tiles.GetNeighbour((HexCoords) hex, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.SouthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.SouthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.NorthEast);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.North);
                testPath.PathSteps.Add(new PathStepModel(hex));

                hex = Tiles.GetNeighbour((HexCoords)hex, HexDirection.SouthEast);
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
                AgentTypeName = "BlockMob"
            });
        }
    }
}
