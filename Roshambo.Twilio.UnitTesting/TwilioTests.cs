using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using ServiceFabric.Mocks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace Roshambo.Twilio.UnitTesting
{
    [TestClass]
    public class TwilioTests
    {
        [DataTestMethod]
        [DataRow("rock"), DataRow("Rock")]
        public async Task GetUserInputAsync_Rock(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Rock, result);
        }

        [DataTestMethod]
        [DataRow("paper"), DataRow("Paper")]
        public async Task GetUserInputAsync_Paper(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Paper, result);
        }

        [DataTestMethod]
        [DataRow("scissor"), DataRow("Scissor"), DataRow("scissors"), DataRow("Scissors")]
        public async Task GetUserInputAsync_Scissor(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Scissor, result);
        }

        [TestMethod]
        public async Task GetUserInputAsync_InvalidInput()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
                async () => await target.GetUserInputAsync("Garbage"));

            Assert.AreEqual("Input Garbage not recognized.", exception.Message);
        }

        [DataTestMethod]
        [DataRow(GameOption.Rock, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Rock)]
        public async Task GetTextMessageBodyAsync_Winner_Human(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                playerMove, computerMove, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, CurrentStreak = 1 });
            Assert.IsTrue(result.StartsWith($"{playerMove} beat {computerMove}."));
        }

        [DataTestMethod]
        [DataRow(GameOption.Scissor, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Scissor)]
        public async Task GetTextMessageBodyAsync_Winner_Computer(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                playerMove, computerMove, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, CurrentStreak = 1 });
            Assert.IsTrue(result.StartsWith($"{playerMove} lost to {computerMove}."));
        }

        [DataTestMethod]
        [DataRow(GameOption.Scissor, GameOption.Paper)]
        [DataRow(GameOption.Paper, GameOption.Scissor)]
        public async Task GetTextMessageBodyAsync_Tie(
            GameOption playerMove, GameOption computerMove)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                playerMove, computerMove, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true });
            Assert.IsTrue(result.StartsWith("Tie!"));
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Winner_Invalid()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
                async () => await target.GetTextMessageBodyAsync(
                    GameOption.Paper, GameOption.Scissor,
                    (TurnWinner)99, new PlayerTurnResult { NextMoveReady = true }));

            Assert.AreEqual("Input 99 not recognized.", exception.Message);
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Winner_Human_StreakIncrease()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Rock, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 6, CurrentStreak = 7 });
            Assert.IsTrue(result.Contains("Your streak increased to 7."));
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Winner_Human_StreakReset()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Rock, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 1 });
            Assert.IsTrue(result.Contains("Your previous streak was 7."));
        }

        [TestMethod, ExpectedException(typeof(InvalidConstraintException))]
        public async Task GetTextMessageBodyAsync_Winner_Human_StreakMaintained()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            await target.GetTextMessageBodyAsync(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Human,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Winner_Computer_StreakIncrease()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Rock, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 6, CurrentStreak = 7 });
            Assert.IsTrue(result.Contains("Your streak increased to 7."));
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Winner_Computer_StreakReset()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Rock, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 1 });
            Assert.IsTrue(result.Contains("Your previous streak was 7."));
        }

        [TestMethod, ExpectedException(typeof(InvalidConstraintException))]
        public async Task GetTextMessageBodyAsync_Winner_Computer_StreakMaintained()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            await target.GetTextMessageBodyAsync(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Computer,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Tie_StreakMaintained()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 7, CurrentStreak = 7 });
            Assert.IsTrue(result.Contains("Your current streak is 7."), result);
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Streak_InvalidState()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var exception = await Assert.ThrowsExceptionAsync<InvalidConstraintException>(
                async () => await target.GetTextMessageBodyAsync(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true, PreviousStreak = 2, CurrentStreak = 0 }));
            Assert.AreEqual("Streak did not update in a consistent manner.", exception.Message);
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_NextMoveReady()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Paper, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true });

            Assert.IsTrue(result.EndsWith("Ready for your next move."));
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_NextMoveNotReady_Exception()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
                async () => await target.GetTextMessageBodyAsync(
                    GameOption.Paper, GameOption.Scissor,
                    TurnWinner.Computer, new PlayerTurnResult { NextMoveReady = false }));

            Assert.AreEqual("Next move must be ready.", exception.Message);
        }
    }
}
