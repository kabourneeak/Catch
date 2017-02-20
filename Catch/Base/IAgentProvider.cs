using Catch.Map;
using Catch.Mobs;
using Catch.Towers;

namespace Catch.Base
{
    public interface IAgentProvider
    {
        TowerBase CreateTower(string name, Tile tile, ILevelStateModel level);

        MobBase CreateMob(string name, MapPath mapPath, ILevelStateModel level);

        Modifier CreateModifier(string name);

        IIndicator CreateIndicator(string name);
    }
}
