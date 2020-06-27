using Roshambo.Shared;
using System;

namespace Roshambo.GettingStarted
{
    class GameEngine
    {
        private static readonly Random _random = new Random();

        public (GameOptions Computer1, GameOptions Computer2, string Winner) PlayRound()
            =>PlayRound((GameOptions) _random.Next(0, 3));

        public (GameOptions Computer1, GameOptions Computer2, string Winner) PlayRound(GameOptions player)
        {
            var computer2 = (GameOptions)_random.Next(0, 3);

            if (CheckIfTied(player, computer2))
            {
                return (player, computer2, "Tie");
            }

            return (player, computer2, CheckIfWon(player, computer2)
                ? "Computer 1"
                : "Computer 2");
        }

        static bool CheckIfTied(GameOptions userPlayed, GameOptions computerPlayed)
            => userPlayed == computerPlayed;

        static bool CheckIfWon(GameOptions userPlayed, GameOptions computerPlayed)
        {
            return (userPlayed == GameOptions.Rock && computerPlayed == GameOptions.Scissor)
                || (userPlayed == GameOptions.Paper && computerPlayed == GameOptions.Rock)
                || (userPlayed == GameOptions.Scissor && computerPlayed == GameOptions.Paper);
        }
    }
}
