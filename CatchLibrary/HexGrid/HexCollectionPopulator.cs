namespace CatchLibrary.HexGrid
{
    public delegate T HexCollectionPopulator<T>(HexCoords hc, T curVal);
}