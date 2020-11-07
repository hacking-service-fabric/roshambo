using Roshambo.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Roshambo.Twilio.Implementations
{
    public class ResponseNextMoveSentenceProvider: IResponseSentenceProvider
    {
        public string GetResponseSentence(GameOption playerMove, GameOption computerMove, TurnWinner winner,
            PlayerTurnResult playerTurnResult)
        {
            if (!playerTurnResult.NextMoveReady)
                throw new ValidationException("Next move must be ready.");

            return "Ready for your next move.";
        }
    }
}
