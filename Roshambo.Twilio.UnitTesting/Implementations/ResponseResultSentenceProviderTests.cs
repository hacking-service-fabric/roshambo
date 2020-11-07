using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using Roshambo.Twilio.Implementations;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Roshambo.Twilio.UnitTesting.Implementations
{
    [TestClass]
    public class ResponseResultSentenceProviderTests
    {
        [DataTestMethod]
        [DataRow(GameOption.Rock, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Rock)]
        public void GetResponseSentence_Winner_Human(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new ResponseResultSentenceProvider();

            var result = target.GetResponseSentence(
                playerMove, computerMove, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, CurrentStreak = 1 });
            Assert.AreEqual($"{playerMove} beat {computerMove}.", result);
        }

        [DataTestMethod]
        [DataRow(GameOption.Scissor, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Scissor)]
        public void GetResponseSentence_Winner_Computer(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new ResponseResultSentenceProvider();

            var result = target.GetResponseSentence(
                playerMove, computerMove, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, CurrentStreak = 1 });
            Assert.AreEqual($"{playerMove} lost to {computerMove}.", result);
        }

        [DataTestMethod]
        [DataRow(GameOption.Scissor, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Scissor)]
        public void GetResponseSentence_Tie(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new ResponseResultSentenceProvider();

            var result = target.GetResponseSentence(
                playerMove, computerMove, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true });
            Assert.AreEqual("Tie!", result);
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Invalid()
        {
            var target = new ResponseResultSentenceProvider();

            var exception = Assert.ThrowsException<ValidationException>(
                () => target.GetResponseSentence(
                    GameOption.Paper, GameOption.Scissor,
                    (TurnWinner)99, new PlayerTurnResult { NextMoveReady = true }));

            Assert.AreEqual("Input 99 not recognized.", exception.Message);
        }
    }
}
