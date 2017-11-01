using System.Collections.Generic;
using Catch.Graphics;
using Catch.Services;

namespace Catch.Towers
{
    public class EmptyTowerGraphicsProvider : IProvider, IGraphicsProvider
    {
        private readonly Dictionary<string, LabelIndicator> _labels;

        public EmptyTowerGraphicsProvider()
        {
            _labels = new Dictionary<string, LabelIndicator>();
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var label in _labels.Values)
                label.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var label in _labels.Values)
                label.DestroyResources();
        }

        public LabelIndicator GetLabel(string label)
        {
            if (!_labels.ContainsKey(label))
                _labels.Add(label, new LabelIndicator(label));

            return _labels[label];
        }
    }
}
