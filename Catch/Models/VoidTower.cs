using System.Collections.Generic;
using Windows.UI;
using Catch.Base;
using Catch.Services;

namespace Catch.Models
{
    /// <summary>
    /// This tower has no behaviour nor interactions, but draws a background hexagon. This tower
    /// gives visual definition to a map, and may be part of a MapPath.
    /// </summary>
    public class VoidTower : Tower
    {
        public VoidTower(Tile tile, IConfig config) : base(tile)
        {
            Brain = GetSharedBrain();
            Indicators.AddRange(GetSharedIndicators(config));
            Indicators.Add(new LabelIndicator(tile, string.Format("{0},{1}", tile.Row, tile.Column)));
        }

        public override string GetAgentType()
        {
            return typeof(NilTower).Name;
        }

        #region Shared Resources

        private static IBehaviourComponent _sharedBrain;
        private static List<IIndicator> _sharedIndicators;

        private static IBehaviourComponent GetSharedBrain()
        {
            return _sharedBrain ?? (_sharedBrain = new EmptyBrain());
        }

        private static IEnumerable<IIndicator> GetSharedIndicators(IConfig config)
        {
            if (_sharedIndicators == null)
            {
                _sharedIndicators = new List<IIndicator>();

                var radius = config.GetFloat("TileRadius");
                var inset = config.GetFloat("TileRadiusInset");
                var style = new StyleArgs() { BrushType = BrushType.Solid, Color = Colors.DarkRed, StrokeWidth = 3 };

                _sharedIndicators.Add(new HexagonGraphics(radius - inset, style));
            }

            return _sharedIndicators;
        }

        #endregion
    }
}
