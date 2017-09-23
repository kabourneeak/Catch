using System;
using System.Collections.Generic;
using Catch.Base;

namespace Catch.Services
{
    public class LabelProvider : ILabelProvider
    {
        private readonly Dictionary<string, IPredicate> _predicates = new Dictionary<string, IPredicate>();
        private readonly Dictionary<string, IFact> _facts = new Dictionary<string, IFact>();
        private readonly Dictionary<string, ILabel> _labels = new Dictionary<string, ILabel>();

        public LabelProvider()
        {
            
        }

        public IPredicate GetPredicate(string predicateName)
        {
            if (!_predicates.ContainsKey(predicateName))
                _predicates.Add(predicateName, new PredicateImpl(predicateName));

            return _predicates[predicateName];
        }

        public IFact GetFact(string factName)
        {
            if (!_facts.ContainsKey(factName))
                _facts.Add(factName, new FactImpl(factName));

            return _facts[factName];
        }

        public ILabel GetLabel(IPredicate predicate, IFact fact)
        {
            var key = LabelImpl.GetKey(predicate, fact);

            if (!_labels.ContainsKey(key))
                _labels.Add(key, new LabelImpl(predicate, fact));

            return _labels[key];
        }

        public ILabel GetLabel(string predicateName, string factName) => GetLabel(GetPredicate(predicateName), GetFact(factName));

        #region Interface implementations

        private class PredicateImpl : IPredicate
        {
            private readonly int _hashCode;

            public string Name { get; }

            public PredicateImpl(string predicateName)
            {
                Name = predicateName ?? throw new ArgumentNullException(nameof(predicateName));

                _hashCode = GetKey(Name).GetHashCode();
            }

            public override string ToString() => GetKey(Name);

            public override bool Equals(object obj) => ReferenceEquals(this, obj);

            public override int GetHashCode() => _hashCode;

            private static string GetKey(string predicateName) => $"Predicate:{predicateName}";
        }

        private class FactImpl : IFact
        {
            private readonly int _hashCode;

            public string Name { get; }

            public FactImpl(string factName)
            {
                Name = factName ?? throw new ArgumentNullException(nameof(factName));

                _hashCode = GetKey(Name).GetHashCode();
            }

            public override string ToString() => GetKey(Name);

            public override bool Equals(object obj) => ReferenceEquals(this, obj);

            public override int GetHashCode() => _hashCode;

            private static string GetKey(string factName) => $"Fact:{factName}";
        }

        private class LabelImpl : ILabel
        {
            private readonly int _hashCode;

            public IPredicate Predicate { get; }

            public IFact Fact { get; }

            public LabelImpl(IPredicate predicate, IFact fact)
            {
                Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
                Fact = fact ?? throw new ArgumentNullException(nameof(fact));

                _hashCode = GetKey(Predicate, Fact).GetHashCode();
            }

            public override string ToString() => GetKey(this.Predicate, this.Fact);

            public override bool Equals(object obj) => ReferenceEquals(this, obj);

            public override int GetHashCode() => _hashCode;

            public static string GetKey(IPredicate predicate, IFact fact) => $"{predicate.Name}:{fact.Name}";
        }

        #endregion
    }
}