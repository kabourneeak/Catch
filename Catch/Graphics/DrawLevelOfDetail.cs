using System;

namespace Catch.Graphics
{
    [Flags]
    public enum DrawLevelOfDetail
    {
        None = 0,
        Low = 1,
        Normal = 2,
        High = 4,
        NormalHigh = Normal | High,
        All = Low | Normal | High
    }
}