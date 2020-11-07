using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using ServiceFabric.Mocks;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Moq;

namespace Roshambo.Twilio.UnitTesting
{
    [TestClass]
    public class TwilioTests
    {
        [DataTestMethod]
        [DataRow("rock"), DataRow("Rock")]
        public async Task GetUserInputAsync_Rock(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default, null);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Rock, result);
        }

        [DataTestMethod]
        [DataRow("paper"), DataRow("Paper")]
        public async Task GetUserInputAsync_Paper(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default, null);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Paper, result);
        }

        [DataTestMethod]
        [DataRow("scissor"), DataRow("Scissor"), DataRow("scissors"), DataRow("Scissors")]
        public async Task GetUserInputAsync_Scissor(string input)
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default, null);

            var result = await target.GetUserInputAsync(input);
            Assert.AreEqual(GameOption.Scissor, result);
        }

        [TestMethod]
        public async Task GetUserInputAsync_InvalidInput()
        {
            var target = new Twilio(MockStatelessServiceContextFactory.Default, null);

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
                async () => await target.GetUserInputAsync("Garbage"));

            Assert.AreEqual("Input Garbage not recognized.", exception.Message);
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Sentences_JoinedBySpace()
        {
            var firstSentenceProvider = new Mock<IResponseSentenceProvider>();
            firstSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Returns("A");

            var secondSentenceProvider = new Mock<IResponseSentenceProvider>();
            secondSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Returns("B");

            var target = new Twilio(MockStatelessServiceContextFactory.Default,
                new [] { firstSentenceProvider.Object, secondSentenceProvider.Object});

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Paper, GameOption.Scissor,
                TurnWinner.Tie, null);

            Assert.AreEqual("A B", result);
        }

        [DataTestMethod]
        [DataRow(""), DataRow(null)]
        public async Task GetTextMessageBodyAsync_Sentence_NullOrEmpty_Ignored(string nullOrEmpty)
        {
            var firstSentenceProvider = new Mock<IResponseSentenceProvider>();
            firstSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Returns("A");

            var secondSentenceProvider = new Mock<IResponseSentenceProvider>();
            secondSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Returns(nullOrEmpty);

            var target = new Twilio(MockStatelessServiceContextFactory.Default,
                new[] { firstSentenceProvider.Object, secondSentenceProvider.Object });

            var result = await target.GetTextMessageBodyAsync(
                GameOption.Paper, GameOption.Scissor,
                TurnWinner.Tie, null);

            Assert.AreEqual("A", result);
        }

        [TestMethod]
        public async Task GetTextMessageBodyAsync_Exception_BubblesUp()
        {
            var firstSentenceProvider = new Mock<IResponseSentenceProvider>();
            firstSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Returns("A");

            var secondSentenceProvider = new Mock<IResponseSentenceProvider>();
            secondSentenceProvider.Setup(p => p.GetResponseSentence(
                    It.IsAny<GameOption>(), It.IsAny<GameOption>(),
                    It.IsAny<TurnWinner>(), It.IsAny<PlayerTurnResult>()))
                .Throws(new ValidationException("Some exception."));

            var target = new Twilio(MockStatelessServiceContextFactory.Default,
                new[] { firstSentenceProvider.Object, secondSentenceProvider.Object });

            var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
                async () => await target.GetTextMessageBodyAsync(
                    GameOption.Paper, GameOption.Scissor,
                    TurnWinner.Tie, null));

            Assert.AreEqual("Some exception.", exception.Message);
        }
    }
}
