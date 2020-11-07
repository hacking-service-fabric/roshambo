using Roshambo.Common.Models;

namespace Roshambo.Twilio
{
    public interface IResponseSentenceProvider
    {
        string GetResponseSentence(
            GameOption playerMove, GameOption computerMove,
            TurnWinner winner, PlayerTurnResult playerTurnResult);
    }
}
