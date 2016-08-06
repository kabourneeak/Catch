namespace CatchLibrary.HexGrid
{
    public delegate T HexGridPopulator<T>(HexCoords hc, T curVal);
}