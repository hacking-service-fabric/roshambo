using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using Roshambo.Twilio.Implementations;
using System.ComponentModel.DataAnnotations;

namespace Roshambo.Twilio.UnitTesting.Implementations
{
    [TestClass]
    public class ResponseNextMoveSentenceProviderTests
    {
        [TestMethod]
        public void GetResponseSentence_NextMoveReady()
        {
            var target = new ResponseNextMoveSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Paper, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true });

            Assert.AreEqual("Ready for your next move.", result);
        }

        [TestMethod]
        public void GetResponseSentence_NextMoveNotReady_Exception()
        {
            var target = new ResponseNextMoveSentenceProvider();

            var exception = Assert.ThrowsException<ValidationException>(
                () => target.GetResponseSentence(
                    GameOption.Paper, GameOption.Scissor,
                    TurnWinner.Computer, new PlayerTurnResult { NextMoveReady = false }));

            Assert.AreEqual("Next move must be ready.", exception.Message);
        }
    }
}
