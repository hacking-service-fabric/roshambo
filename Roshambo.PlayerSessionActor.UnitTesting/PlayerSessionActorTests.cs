using Microsoft.ServiceFabric.Actors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roshambo.Common.Models;
using ServiceFabric.Mocks;
using System;
using System.Threading.Tasks;

namespace Roshambo.PlayerSessionActor.UnitTesting
{
    [TestClass]
    public class PlayerSessionActorTests
    {
        private PlayerSessionActor _target;

        [TestInitialize]
        public void Initialize()
        {
            var svc = MockActorServiceFactory.CreateActorServiceForActor<PlayerSessionActor>(
                (service, actorId) => new PlayerSessionActor(service, actorId));
            _target = svc.Activate(new ActorId(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Human_PreviousWinner_None()
        {
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Human);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(0, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Human_PreviousWinner_Human()
        {
            await SetStateAsync(TurnWinner.Human, 4);
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Human);

            Assert.AreEqual(5, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsFalse(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie_ThenWinner_Human_PreviousWinner_Human()
        {
            await SetStateAsync(TurnWinner.Human, 4);

            var tieResult = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(4, tieResult.CurrentStreak);
            Assert.AreEqual(4, tieResult.PreviousStreak);
            Assert.IsTrue(tieResult.NextMoveReady);

            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Human);

            Assert.AreEqual(5, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Human_PreviousWinner_Computer()
        {
            await SetStateAsync(TurnWinner.Computer, 4);
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Human);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie_ThenWinner_Human_PreviousWinner_Computer()
        {
            await SetStateAsync(TurnWinner.Computer, 4);

            var tieResult = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(4, tieResult.CurrentStreak);
            Assert.AreEqual(4, tieResult.PreviousStreak);
            Assert.IsTrue(tieResult.NextMoveReady);

            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Human);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Computer_PreviousWinner_None()
        {
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Computer);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(0, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Computer_PreviousWinner_Computer()
        {
            await SetStateAsync(TurnWinner.Computer, 4);
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Computer);

            Assert.AreEqual(5, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie_ThenWinner_Computer_PreviousWinner_Computer()
        {
            await SetStateAsync(TurnWinner.Computer, 4);

            var tieResult = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(4, tieResult.CurrentStreak);
            Assert.AreEqual(4, tieResult.PreviousStreak);
            Assert.IsTrue(tieResult.NextMoveReady);

            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Computer);

            Assert.AreEqual(5, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Winner_Computer_PreviousWinner_Human()
        {
            await SetStateAsync(TurnWinner.Human, 4);
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Computer);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie_ThenWinner_Computer_PreviousWinner_Human()
        {
            await SetStateAsync(TurnWinner.Human, 4);

            var tieResult = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(4, tieResult.CurrentStreak);
            Assert.AreEqual(4, tieResult.PreviousStreak);
            Assert.IsTrue(tieResult.NextMoveReady);

            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Computer);

            Assert.AreEqual(1, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie()
        {
            await SetStateAsync(TurnWinner.Human, 4);
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(4, result.CurrentStreak);
            Assert.AreEqual(4, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        [TestMethod]
        public async Task StoreTurnOutcomeAsync_Tie_PreviousWinner_None()
        {
            var result = await _target.StoreTurnOutcomeAsync(TurnWinner.Tie);

            Assert.AreEqual(0, result.CurrentStreak);
            Assert.AreEqual(0, result.PreviousStreak);
            Assert.IsTrue(result.NextMoveReady);
        }

        private async Task SetStateAsync(TurnWinner previousWinner, int previousStreak)
        {
            await _target.StateManager.SetStateAsync("previousWinner", previousWinner);
            await _target.StateManager.SetStateAsync("previousStreak", previousStreak);
        }
    }
}
