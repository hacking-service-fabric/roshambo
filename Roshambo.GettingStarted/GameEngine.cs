using System;
using System.Collections.Generic;
using System.Text;

namespace Roshambo.GettingStarted
{
    class GameEngine
    {
        private static readonly Random _random = new Random();

        public (GameOptions Computer1, GameOptions Computer2, string Winner) PlayRound()
        {
            var computer1 = (GameOptions) _random.Next(0, 3);
            var computer2 = (GameOptions) _random.Next(0, 3);

            if (CheckIfTied(computer1, computer2))
            {
                return (computer1, computer2, "Tie");
            }

            return (computer1, computer2, CheckIfWon(computer1, computer2)
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
