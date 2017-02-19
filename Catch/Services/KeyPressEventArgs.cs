using Windows.System;

namespace Catch.Services
{
    public class KeyPressEventArgs : EventArgsBase
    {
        public VirtualKey Key { get; }

        public KeyPressEventArgs(VirtualKey key)
        {
            Key = key;
        }
    }
}
