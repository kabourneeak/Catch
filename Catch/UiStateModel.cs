using System.Numerics;
using Catch.Base;
using CatchLibrary.HexGrid;

namespace Catch
{
    /// <summary>
    /// Contains the shared state between the LevelController, 
    /// FieldController, and OverlayController
    /// </summary>
    public class UiStateModel
    {
        public Vector2 WindowSize { get; set; }

        public HexCoords HoverHexCoords { get; set; }

        public IAgent HoverTower { get; set; }

        public UiStateModel()
        {
            WindowSize = Vector2.Zero;
            HoverHexCoords = HexCoords.CreateFromOffset(-1, -1);
        }

        /// <summary>
        /// Deselect everything
        /// </summary>
        public void Deselect()
        {
            HoverTower = null;
        }
    }
}
