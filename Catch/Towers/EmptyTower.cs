using System.Collections.Generic;
using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Map;
using Catch.Services;

namespace Catch.Towers
{
    /// <summary>
    /// This tower is empty, but provides Commands for placing new towers.
    /// </summary>
    public class EmptyTower : TowerBase
    {
        public EmptyTower(Tile tile, IConfig config) : base(tile)
        {
            Brain = GetSharedBrain();
            Indicators.AddRange(GetSharedIndicators(config));

            var label = string.Format("{0},{1}", tile.Coords.Q, tile.Coords.R);
            Indicators.Add(new LabelIndicator(label));

            DisplayName = "Empty Socket";
            DisplayStatus = string.Empty;
            DisplayInfo = string.Empty;
        }

        public override string GetAgentType()
        {
            return nameof(EmptyTower);
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
