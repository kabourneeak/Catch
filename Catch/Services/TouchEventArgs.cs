using System.Numerics;
using Windows.System;

namespace Catch.Services
{
    public class TouchEventArgs : EventArgsBase
    {
        public Vector2 ViewCoords { get; }
        public VirtualKeyModifiers KeyModifiers { get; }

        public TouchEventArgs(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            ViewCoords = viewCoords;
            KeyModifiers = keyModifiers;
        }
    }
}
