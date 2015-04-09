using System;
using System.Collections.Generic;

namespace Catch.Base
{
    public interface IIndicatorCollection : IEnumerable<IIndicator>, IGraphicsComponent
    {
        void Add(IIndicator indicator);

        void Remove(IIndicator indicator);

        int Count { get; }

        IIndicator HasIndicator(string indicatorType);
    }
}