using System.Collections;
using System.Collections.Generic;

namespace Catch.Base
{
    public class ModifierCollection : IEnumerable<Modifier>
    {
        private readonly IAgent _agent;
        private readonly List<Modifier> _modifiers;
        private readonly ModifierComparer _comparer;
        private bool _needsApplyToBase;

        public ModifierCollection(IAgent agent)
        {
            _agent = agent;

            _modifiers = new List<Modifier>();
            _comparer = new ModifierComparer();
        }

        public void Update(float ticks)
        {
            foreach (var m in _modifiers)
            {
                //m.Update(ticks);
                _needsApplyToBase = _needsApplyToBase || m.NeedsApplyToBase;
            }

            var numRemoved = _modifiers.RemoveAll(m => !m.IsActive);

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
            _agent.Stats.Reset();

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
                _modifiers.Sort(_comparer);

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

        public int Count => _modifiers.Count;

        #region IEnumerable

        public IEnumerator<Modifier> GetEnumerator()
        {
            return _modifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private class ModifierComparer : IComparer<Modifier>
        {
            public int Compare(Modifier x, Modifier y)
            {
                return x.Priority.CompareTo(y.Priority);
            }
        }
    }
}