using Catch.Models;

namespace Catch.Base
{
    public interface IAgentProvider
    {
        ITower CreateTower(string name, IHexTile tile);

        IMob CreateMob(string name, MapPath mapPath);

        IModifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
