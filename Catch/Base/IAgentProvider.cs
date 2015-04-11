using Catch.Models;

namespace Catch.Base
{
    public interface IAgentProvider
    {
        Tower CreateTower(string name, Tile tile);

        Mob CreateMob(string name, MapPath mapPath);

        IModifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
