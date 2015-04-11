using System;
using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    public class ModifierCollection : IEnumerable<IModifier>
    {
        private readonly SortedSet<IModifier> _modifiers;

        public ModifierCollection()
        {
            _modifiers = new SortedSet<IModifier>(ModifierComparer.GetComparer());
        }

        public IEnumerator<IModifier> GetEnumerator()
        {
            return _modifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Update(float ticks)
        {
            foreach (var m in _modifiers)
                m.Update(ticks);
        }

        private class ModifierComparer : IComparer<IModifier>
        {
            private static ModifierComparer _instance;

            public static ModifierComparer GetComparer()
            {
                return _instance ?? (_instance = new ModifierComparer());
            }

            private ModifierComparer()
            {

            }

            public int Compare(IModifier x, IModifier y)
            {
                throw new NotImplementedException();
            }
        }
    }
}