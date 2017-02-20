namespace Catch.Services
{
    /// <summary>
    /// Allows IScreenController instances to request new subscreens, and 
    /// to close themselves
    /// </summary>
    public interface IScreenManager
    {
        void RequestScreen(IScreenController screen);

        void CloseScreen(IScreenController screen);
    }
}
