using Catch.Services;

namespace Catch
{
    public class PlayerModel
    {
        private const int StartLives = 3;
        private const int StartScore = 0;
        private const int ScoreIncrement = 10;

        public int Score { get; set; }
        public int Lives { get; set; }

        private readonly IConfig _config;

        public PlayerModel(IConfig config)
        {
            _config = config;

            Score = StartScore;
            Lives = StartLives;
        }
    }
}
