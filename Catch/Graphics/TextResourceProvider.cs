using System.Collections.Generic;
using Catch.Base;

namespace Catch.Graphics
{
    public class TextResourceProvider : IProvider, IGraphicsResourceContainer
    {
        private readonly HashSet<TextResource> _labels;

        public TextResourceProvider()
        {
            _labels = new HashSet<TextResource>();
        }

        public TextResource GetLabel(string text)
        {
            // TODO find some way to make sure we return singletons

            var label = new TextResource(text);

            _labels.Add(label);

            return label;
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            foreach (var label in _labels)
                label.CreateResources(args);
        }

        public void DestroyResources()
        {
            foreach (var label in _labels)
                label.DestroyResources();
        }
    }
}
