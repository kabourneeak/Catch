﻿using Catch.Base;

namespace Catch.Level
{
    public class UpdateReadinessEventArgs : IUpdateReadinessEventArgs
    {
        public ISimulationState Sim { get; set; }
    }
}