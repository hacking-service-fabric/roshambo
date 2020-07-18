namespace Roshambo.GettingStarted.Interfaces
{
    public class GameResult
    {
        public GameOptions PlayerOption { get; set; }
        public GameOptions ComputerOption { get; set; }
        public WinOptions Result { get; set; }
    }
}
