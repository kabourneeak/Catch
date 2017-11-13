namespace Catch.Base
{
    public interface IIndicatorProvider
    {
        IIndicator GetIndicator(string name);

        IIndicator GetIndicator(string name, IExtendedAgent agent);

        IIndicator GetIndicator(string name, IMapTile tile);

        IndicatorCollection CreateIndicatorCollection();
    }
}