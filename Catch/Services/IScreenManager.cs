namespace Catch.Services
{
    /// <summary>
    /// Allows IScreenController instances to request new subscreens, and 
    /// to close themselves
    /// </summary>
    public interface IScreenManager
    {
        void RequestScreen(GameStateArgs args);

        void CloseScreen(IScreenController screen);
    }
}
