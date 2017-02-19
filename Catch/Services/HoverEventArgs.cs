using System.Numerics;
using Windows.System;

namespace Catch.Services
{
    public class HoverEventArgs : EventArgsBase
    {
        public Vector2 ViewCoords { get; }
        public VirtualKeyModifiers KeyModifiers { get; }

        public HoverEventArgs(Vector2 viewCoords, VirtualKeyModifiers keyModifiers)
        {
            ViewCoords = viewCoords;
            KeyModifiers = keyModifiers;
        }
    }
}
