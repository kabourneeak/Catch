namespace CatchLibrary.HexGrid
{
    public delegate T HexGridPopulator<T>(int row, int column, T curVal);
}