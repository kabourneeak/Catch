namespace Catch.Base
{
    public interface IHexTile : IGameObject
    {
        // properties
        int Row { get; }

        int Column { get; }

        Tower GetTower();
    }
}
