namespace Catch.Base
{
    public interface IPredicate
    {
        string Name { get; }
    }

    public interface IFact
    {
        string Name { get; }
    }

    public interface ILabel
    {
        IPredicate Predicate { get; }

        IFact Fact { get; }
    }

    public interface ILabelProvider
    {
        IPredicate GetPredicate(string predicateName);

        IFact GetFact(string factName);

        ILabel GetLabel(IPredicate predicate, IFact fact);

        ILabel GetLabel(string predicateName, string factName);
    }
}
