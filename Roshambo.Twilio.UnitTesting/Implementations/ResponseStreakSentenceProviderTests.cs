using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using Roshambo.Twilio.Implementations;
using System.Data;
using Castle.Core.Internal;

namespace Roshambo.Twilio.UnitTesting.Implementations
{
    [TestClass]
    public class ResponseStreakSentenceProviderTests
    {
        [TestMethod]
        public void GetResponseSentence_Winner_Human_StreakIncrease()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Rock, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 6, CurrentStreak = 7 });
            Assert.AreEqual("Your streak increased to 7.", result);
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Human_StreakReset()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Rock, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 1 });
            Assert.AreEqual("Your previous streak was 7.", result);
        }

        [TestMethod, ExpectedException(typeof(InvalidConstraintException))]
        public void GetResponseSentence_Winner_Human_StreakMaintained()
        {
            var target = new ResponseStreakSentenceProvider();

            target.GetResponseSentence(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Human_Streak_InvalidState()
        {
            var target = new ResponseStreakSentenceProvider();

            var exception = Assert.ThrowsException<InvalidConstraintException>(
                () => target.GetResponseSentence(
                    GameOption.Scissor, GameOption.Paper, TurnWinner.Human,
                    new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 2, CurrentStreak = 0 }));
            Assert.AreEqual("Streak did not update in a consistent manner.", exception.Message);
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Computer_StreakIncrease()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Rock, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 6, CurrentStreak = 7 });
            Assert.AreEqual("Your streak increased to 7.", result);
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Computer_StreakReset()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Rock, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 1 });
            Assert.AreEqual("Your previous streak was 7.", result);
        }

        [TestMethod, ExpectedException(typeof(InvalidConstraintException))]
        public void GetResponseSentence_Winner_Computer_StreakMaintained()
        {
            var target = new ResponseStreakSentenceProvider();

            target.GetResponseSentence(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
        }

        [TestMethod]
        public void GetResponseSentence_Winner_Computer_Streak_InvalidState()
        {
            var target = new ResponseStreakSentenceProvider();

            var exception = Assert.ThrowsException<InvalidConstraintException>(
                () => target.GetResponseSentence(
                    GameOption.Scissor, GameOption.Paper, TurnWinner.Computer,
                    new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 2, CurrentStreak = 0 }));
            Assert.AreEqual("Streak did not update in a consistent manner.", exception.Message);
        }

        [TestMethod]
        public void GetResponseSentence_Tie_StreakMaintained()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
            Assert.AreEqual("Your current streak is 7.", result);
        }

        [TestMethod]
        public void GetResponseSentence_Tie_StreakMaintained_Zero()
        {
            var target = new ResponseStreakSentenceProvider();

            var result = target.GetResponseSentence(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 0, CurrentStreak = 0 });
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GetResponseSentence_Tie_Streak_InvalidState()
        {
            var target = new ResponseStreakSentenceProvider();

            var exception = Assert.ThrowsException<InvalidConstraintException>(
                () => target.GetResponseSentence(
                    GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                    new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 2, CurrentStreak = 0 }));
            Assert.AreEqual("Streak did not update in a consistent manner.", exception.Message);
        }
    }
}
