using Roshambo.Common.Models;
using System.Data;

namespace Roshambo.Twilio.Implementations
{
    public class ResponseStreakSentenceProvider: IResponseSentenceProvider
    {
        public string GetResponseSentence(GameOption playerMove, GameOption computerMove, TurnWinner winner,
            PlayerTurnResult playerTurnResult)
        {
            if (winner == TurnWinner.Tie && playerTurnResult.CurrentStreak == playerTurnResult.PreviousStreak)
            {
                if (playerTurnResult.CurrentStreak == 0)
                {
                    return null;
                }

                return $"Your current streak is {playerTurnResult.CurrentStreak}.";
            }

            if (playerTurnResult.CurrentStreak == 1) // reset
            {
                return $"Your previous streak was {playerTurnResult.PreviousStreak}.";
            }

            if (playerTurnResult.CurrentStreak > playerTurnResult.PreviousStreak)
            {
                return $"Your streak increased to {playerTurnResult.CurrentStreak}.";
            }

            throw new InvalidConstraintException("Streak did not update in a consistent manner.");
        }
    }
}
