using System.Collections.Generic;
using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.Towers
{
    /// <summary>
    /// This tower has no behaviour nor interactions, but draws a background hexagon. This tower
    /// gives visual definition to a map, and may be part of a MapPath.
    /// </summary>
    public class VoidTower : TowerBase
    {
        public VoidTower(Tile tile, IConfig config) : base(tile)
        {
            Brain = GetSharedBrain();
            Indicators.AddRange(GetSharedIndicators(config));

            var label = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(new LabelIndicator(label));
        }

        public override string GetAgentType()
        {
            return nameof(VoidTower);
        }

        #region Shared Resources

        private static IBehaviourComponent _sharedBrain;
        private static List<IIndicator> _sharedIndicators;

        private static IBehaviourComponent GetSharedBrain()
        {
            return _sharedBrain ?? (_sharedBrain = new NilBehaviour());
        }

        private static IEnumerable<IIndicator> GetSharedIndicators(IConfig config)
        {
            if (_sharedIndicators == null)
            {
                _sharedIndicators = new List<IIndicator>
                {
                    new TowerTileIndicator(config, Colors.DarkRed)
                };
            }

            return _sharedIndicators;
        }

        #endregion
    }
}
