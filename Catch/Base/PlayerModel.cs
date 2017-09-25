namespace Catch.Base
{
    public class PlayerModel
    {
        public int Team { get; }

        public int Score { get; set; }

        public int Lives { get; set; }

        public PlayerModel(int team)
        {
            Team = team;
        }
    }
}
