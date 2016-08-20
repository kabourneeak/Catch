using System;
using Catch.Base;
using Catch.Graphics;

namespace Catch.Towers
{
    public class TowerHoverIndicator : IIndicator
    {
        public void Update(float ticks)
        {
            throw new NotImplementedException();
        }

        public void CreateResources(CreateResourcesArgs createArgs)
        {
            throw new NotImplementedException();
        }

        public void DestroyResources()
        {
            throw new NotImplementedException();
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            throw new NotImplementedException();
        }

        public DrawLayer Layer => DrawLayer.Ui;
    }
}
