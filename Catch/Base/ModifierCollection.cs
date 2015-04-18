using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    public class ModifierCollection : IEnumerable<Modifier>
    {
        private readonly IAgent _agent;
        private readonly SortedSet<Modifier> _modifiers;
        private bool _needsApply;

        public ModifierCollection(IAgent agent)
        {
            _agent = agent;
            _modifiers = new SortedSet<Modifier>(ModifierComparer.GetComparer());
        }

        public void Update(float ticks)
        {
            foreach (var m in _modifiers)
            {
                m.Update(ticks);
                _needsApply = _needsApply || m.NeedsApply;
            }

            var numRemoved = _modifiers.RemoveWhere(m => !m.IsActive);

            if (numRemoved > 0)
                _needsApply = true;

            if (_needsApply)
                Apply();
        }

        private void Apply()
        {
            _agent.BaseSpecs.Reset();
            _agent.AttackSpecs.Reset();
            _agent.DefenceSpecs.Reset();

            foreach (var m in _modifiers)
            {
                m.Apply();
            }

            _needsApply = false;
        }

        public void Add(Modifier modifier)
        {
            _modifiers.Add(modifier);
            _needsApply = true;
        }

        public void AddRange(IEnumerable<Modifier> collection)
        {
            foreach (var mod in collection)
                _modifiers.Add(mod);

            _needsApply = true;
        }

        public void Remove(Modifier modifier)
        {
            _modifiers.Remove(modifier);

            _needsApply = true;
        }

        public int Count { get { return _modifiers.Count; } }

        public IEnumerator<Modifier> GetEnumerator()
        {
            return _modifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ModifierComparer : IComparer<Modifier>
        {
            private static ModifierComparer _instance;

            public static ModifierComparer GetComparer()
            {
                return _instance ?? (_instance = new ModifierComparer());
            }

            private ModifierComparer()
            {

            }

            public int Compare(Modifier x, Modifier y)
            {
                return x.Priority.CompareTo(y.Priority);
            }
        }
    }
}