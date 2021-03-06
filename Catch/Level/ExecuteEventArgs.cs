﻿using Catch.Base;

namespace Catch.Level
{
    public class ExecuteEventArgs : IExecuteEventArgs
    {
        public ISimulationState Sim { get; set; }

        public ISimulationManager Manager { get; set; }

        public IAgent SelectedAgent { get; set; }

        public IAgent SelectedTileAgent { get; set; }
    }
}