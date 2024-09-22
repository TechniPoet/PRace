using NUnit.Framework;
using GameLogic;
using UnityEngine;
using Data;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class DefaultGameRulesTests
    {
        private DefaultGameRules _gameRules;
        private RaceConfig _config;
        private GameRunner.GameState _state;
        private GameRunner.RowerData _player;

        [SetUp]
        public void Setup()
        {
            _gameRules = new DefaultGameRules();

            // Create a new RaceConfig with relevant properties
            _config = ScriptableObject.CreateInstance<RaceConfig>();
            _config.ScoreSpeedDistanceThresh = 1.5f;
            _config.RaceDistance = 500;
            _config.ControlledPlayer = ScriptableObject.CreateInstance<PlayerConfig>();
            _config.ControlledPlayer.ChangeInterval = 0.1f;
            _config.BotPlayer = ScriptableObject.CreateInstance<PlayerConfig>();

            _state = new GameRunner.GameState();
            _player = new GameRunner.RowerData { Speed = 5f };

            _state.RowerDatas = new Dictionary<GameRunner.RowerId, GameRunner.RowerData>
            {
                { GameRunner.RowerId.First, _player }
            };
        }

        [Test]
        public void IsInScoreDistance_PlayerWithinDistance_ReturnsTrue()
        {
            _state.TargetSpeed = 5.0f;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInScoreDistance_PlayerOutsideDistance_ReturnsFalse()
        {
            _state.TargetSpeed = 10.0f;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsEndConditionMet_RaceEnded_ReturnsTrue()
        {
            _state.RaceEnded = true;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsEndConditionMet_PlayerExceedsRaceDistance_ReturnsTrue()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SimPosition = 600;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsTrue(result);
        }

        [Test]
        public void AdjustRowerTargetSpeed_IncreasesSpeedCorrectly()
        {
            // Set Min and Max speed to ensure proper clamping range
            _config.ControlledPlayer.MinSpeed = 1f;
            _config.ControlledPlayer.MaxSpeed = 10f;

            // Initial TargetSpeed is 5f
            _state.RowerDatas[GameRunner.RowerId.First].TargetSpeed = 5f;

            // Increase
            _gameRules.AdjustRowerTargetSpeed(_config, _state, GameRunner.RowerId.First, true);

            Assert.AreEqual(5f + _config.ControlledPlayer.ChangeInterval,
                _state.RowerDatas[GameRunner.RowerId.First].TargetSpeed);
        }

        [Test]
        public void AdjustRowerTargetSpeed_DecreasesSpeedCorrectly()
        {
            // Set Min and Max speed to ensure proper clamping range
            _config.ControlledPlayer.MinSpeed = 1f;
            _config.ControlledPlayer.MaxSpeed = 10f;

            // Initial TargetSpeed is 5f
            _state.RowerDatas[GameRunner.RowerId.First].TargetSpeed = 5f;

            // Decrease speed
            _gameRules.AdjustRowerTargetSpeed(_config, _state, GameRunner.RowerId.First, false);

            // The expected target speed should decrease by ChangeInterval (0.1f)
            Assert.AreEqual(5f - _config.ControlledPlayer.ChangeInterval, 
                _state.RowerDatas[GameRunner.RowerId.First].TargetSpeed);
        }

        [Test]
        public void SimulationTick_ReturnsFalseIfRaceEnded()
        {
            _state.RaceEnded = true;

            var result = _gameRules.SimulationTick(_config, _state, 0.016f);

            Assert.IsFalse(result);
        }

        // Additional Tests for GameConfig class
        [Test]
        public void GameConfig_LoadsDataCorrectly()
        {
            var gameConfig = ScriptableObject.CreateInstance<GameConfig>();
            var race1 = ScriptableObject.CreateInstance<RaceConfig>();
            race1.name = "Race1";
            race1.DisplayName = "First Race";

            var race2 = ScriptableObject.CreateInstance<RaceConfig>();
            race2.name = "Race2";
            race2.DisplayName = "Second Race";

            gameConfig.Setup(new List<RaceConfig> { race1, race2 });

            var raceData = gameConfig.GetRaceDisplayData();

            Assert.AreEqual(2, raceData.Length);
            Assert.AreEqual("Race1", raceData[0].Item1);
            Assert.AreEqual("First Race", raceData[0].Item2);
            Assert.AreEqual("Race2", raceData[1].Item1);
            Assert.AreEqual("Second Race", raceData[1].Item2);
        }

        [Test]
        public void GameConfig_ReturnsCorrectRaceConfig()
        {
            var gameConfig = ScriptableObject.CreateInstance<GameConfig>();
            var race1 = ScriptableObject.CreateInstance<RaceConfig>();
            race1.name = "Race1";
            race1.DisplayName = "First Race";

            gameConfig.Setup(new List<RaceConfig> { race1 });

            var raceConfig = gameConfig.GetRaceConfig("Race1");

            Assert.AreEqual("First Race", raceConfig.DisplayName);
        }
    }
}