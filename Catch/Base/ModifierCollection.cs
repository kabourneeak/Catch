using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    public class ModifierCollection : IEnumerable<Modifier>
    {
        private readonly IAgent _agent;
        private readonly SortedSet<Modifier> _modifiers;
        private bool _needsApplyToBase;

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
                _needsApplyToBase = _needsApplyToBase || m.NeedsApplyToBase;
            }

            var numRemoved = _modifiers.RemoveWhere(m => !m.IsActive);

            if (numRemoved > 0)
                _needsApplyToBase = true;

            if (_needsApplyToBase)
                ApplyToBase();
        }

        public bool ApplyToAttack(AttackModel outgoingAttack)
        {
            var apply = true;

            foreach (var m in _modifiers)
            {
                apply = apply && m.ApplyToAttack(outgoingAttack);
            }

            return apply;
        }

        public bool ApplyToHit(AttackModel incomingAttack)
        {
            var apply = true;

            foreach (var m in _modifiers)
            {
                apply = apply && m.ApplyToHit(incomingAttack);
            }

            return apply;
        }

        private void ApplyToBase()
        {
            _agent.BaseSpecs.Reset();

            foreach (var m in _modifiers)
            {
                m.ApplyToBase();
            }

            _needsApplyToBase = false;
        }

        public void Add(Modifier modifier)
        {
            var apply = true;

            foreach (var m in _modifiers)
            {
                apply = apply && m.ApplyToModifier(modifier);
            }

            if (apply)
            {
                _modifiers.Add(modifier);
                _needsApplyToBase = true;
            }
        }

        public void AddRange(IEnumerable<Modifier> collection)
        {
            foreach (var mod in collection)
                _modifiers.Add(mod);

            _needsApplyToBase = true;
        }

        public void Remove(Modifier modifier)
        {
            _modifiers.Remove(modifier);

            _needsApplyToBase = true;
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