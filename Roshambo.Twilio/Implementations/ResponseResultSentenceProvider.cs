using Roshambo.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Roshambo.Twilio.Implementations
{
    public class ResponseResultSentenceProvider: IResponseSentenceProvider
    {
        public string GetResponseSentence(GameOption playerMove, GameOption computerMove, TurnWinner winner,
            PlayerTurnResult playerTurnResult)
        {
            var winSentence = winner switch
            {
                TurnWinner.Human => $"{playerMove} beat {computerMove}.",
                TurnWinner.Computer => $"{playerMove} lost to {computerMove}.",
                TurnWinner.Tie => $"Tie!",
                _ => throw new ValidationException(
                    $"Input {winner} not recognized.")
            };

            return winSentence;
        }
    }
}
