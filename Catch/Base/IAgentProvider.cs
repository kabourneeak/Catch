namespace Catch.Base
{
    public interface IAgentProvider
    {
        IAgent CreateAgent(string name);

        ITower CreateTower(string name, IHexTile tile);

        IMob CreateMob(string name, IPath path);

        IModifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
