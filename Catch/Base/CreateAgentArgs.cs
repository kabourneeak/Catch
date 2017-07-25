﻿using Catch.Map;

namespace Catch.Base
{
    public class CreateAgentArgs
    {
        public int Team { get; set; }

        public Tile Tile { get; set; }

        public MapPath Path { get; set; }
    }
}