using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using ServiceFabric.Mocks;
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
                new PlayerTurnResult { NextMoveReady = true });
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
                new PlayerTurnResult { NextMoveReady = true });
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
        public Task GetTextMessageBodyAsync_Win_StreakIncrease()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public Task GetTextMessageBodyAsync_Win_StreakReset()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public Task GetTextMessageBodyAsync_Lose_StreakIncrease()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public Task GetTextMessageBodyAsync_Lose_StreakReset()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Tie_StreakMaintained()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default);

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Scissor, GameOption.Paper, TurnWinner.Tie,
                new PlayerTurnResult { NextMoveReady = true, CurrentStreak = 7 });
            Assert.AreEqual("Tie! Your streak remains at 7.", result);
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
