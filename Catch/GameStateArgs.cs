using System;
using CatchLibrary.Serialization;

namespace Catch
{
    public class GameStateArgs : EventArgs
    {
        public GameState State { get; }

        public MapModel MapModel { get; }

        public GameStateArgs(GameState state, MapModel mapModel)
        {
            State = state;
            MapModel = mapModel;
        }
    }
}