using System.Collections.Generic;
using Windows.UI;
using Catch.Base;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class EmptyTowerSharedResources : IGraphicsResource
    {
        private readonly Dictionary<string, LabelIndicator> _labels;

        public IndicatorCollection Indicators { get; }

        public EmptyTowerSharedResources(IConfig config)
        {
            Indicators = new IndicatorCollection
            {
                new TowerTileIndicator(config, Colors.DarkRed)
            };

            _labels = new Dictionary<string, LabelIndicator>();
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            Indicators.CreateResources(args);

            foreach (var label in _labels.Values)
                label.CreateResources(args);
        }

        public void DestroyResources()
        {
            Indicators.DestroyResources();
        }

        public LabelIndicator GetLabel(string label)
        {
            if (!_labels.ContainsKey(label))
                _labels.Add(label, new LabelIndicator(label));

            return _labels[label];
        }
    }
}
