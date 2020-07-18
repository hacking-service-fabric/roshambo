using Roshambo.GettingStarted.Interfaces;
using System;

namespace Roshambo.GettingStarted
{
    class GameEngine
    {
        private static readonly Random _random = new Random();

        public GameResult PlayRound()
            => PlayRound((GameOptions) _random.Next(0, 3));

        public GameResult PlayRound(GameOptions player)
        {
            var result = new GameResult
            {
                PlayerOption = player,
                ComputerOption = (GameOptions) _random.Next(0, 3)
            };

            if (CheckIfTied(result.PlayerOption, result.ComputerOption))
            {
                result.Result = WinOptions.Tied;
            }
            else
            {
                result.Result = CheckIfWon(result.PlayerOption, result.ComputerOption)
                    ? WinOptions.Won
                    : WinOptions.Lost;
            }

            return result;
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
