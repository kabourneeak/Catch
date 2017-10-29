using System.Collections.Generic;
using Catch.Graphics;

namespace Catch.Towers
{
    public class EmptyTowerSharedResources : IGraphicsResource
    {
        private readonly Dictionary<string, LabelIndicator> _labels;

        public EmptyTowerSharedResources()
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
