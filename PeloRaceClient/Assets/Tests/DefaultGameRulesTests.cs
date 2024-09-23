using NUnit.Framework;
using GameLogic;
using UnityEngine;
using Data;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class DefaultGameRulesTests
    {
        private DefaultGameRules _gameRules;
        private RaceConfig _config;
        private GameRunner.GameState _state;
        private GameRunner.RowerData _player1;
        private GameRunner.RowerData _player2;

        #region File-level Constants

        // Constants for Player 1
        private const float Player1StartSpeed = 5f;
        private const float Player1LerpToSpeedInterval = 0.5f;

        // Constants for Player 2
        private const float Player2StartSpeed = 6f;
        private const float Player2LerpToSpeedInterval = 0.3f;

        // General Race Config Constants
        private const float ScoreSpeedDistanceThreshold = 1.5f;
        private const float RaceDistance = 500f;
        private const float SpeedChangeIntervalPlayer1 = 0.1f;
        private const float MinSpeedPlayer1 = 0f;
        private const float MaxSpeedPlayer1 = 20f;
        private const float SpeedChangeIntervalPlayer2 = 0.2f;
        private const float MinSpeedPlayer2 = 0f;
        private const float MaxSpeedPlayer2 = 15f;

        // Simulation and Tick Constants
        private const float DeltaTime = 0.016f;  // Simulated delta time
        private const float TargetSpeedChangeInterval = 1f;

        // Test-specific Constants
        private const float PlayerSimPositionBeyondRace = 600f;
        private const float PlayerSimPositionBelowRace = 400f;
        private const float TargetSpeedOutsideDistance = 10.0f;
        private const float OutOfScoreZoneSpeed = 10f;

        #endregion

        [SetUp]
        public void Setup()
        {
            _gameRules = new DefaultGameRules();

            // Create a new RaceConfig with relevant properties
            _config = ScriptableObject.CreateInstance<RaceConfig>();
            _config.ScoreSpeedDistanceThresh = ScoreSpeedDistanceThreshold;
            _config.RaceDistance = RaceDistance;
            _config.ControlledPlayer = ScriptableObject.CreateInstance<PlayerConfig>();
            _config.ControlledPlayer.MinSpeed = MinSpeedPlayer1;
            _config.ControlledPlayer.MaxSpeed = MaxSpeedPlayer1;
            _config.ControlledPlayer.StartSpeed = Player1StartSpeed;
            _config.ControlledPlayer.SpeedChangePerSecond = Player1LerpToSpeedInterval;
            _config.BotPlayer = ScriptableObject.CreateInstance<PlayerConfig>();
            _config.BotPlayer.MinSpeed = MinSpeedPlayer2;
            _config.BotPlayer.MaxSpeed = MaxSpeedPlayer2;
            _config.BotPlayer.StartSpeed = Player2StartSpeed;
            _config.BotPlayer.SpeedChangePerSecond = Player2LerpToSpeedInterval;

            // Setup player rower data using the Setup method
            _player1 = new GameRunner.RowerData();
            _player1.Setup(_config.ControlledPlayer);

            _player2 = new GameRunner.RowerData();
            _player2.Setup(_config.BotPlayer);

            _state = new GameRunner.GameState();
            _state.RowerDatas = new Dictionary<GameRunner.RowerId, GameRunner.RowerData>
            {
                { GameRunner.RowerId.First, _player1 },
                { GameRunner.RowerId.Second, _player2 }
            };
        }

        #region IsInScoreDistance

        [Test]
        public void IsInScoreDistance_PlayerWithinDistance_ReturnsTrue()
        {
            _state.TargetSpeed = Player1StartSpeed;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player1);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInScoreDistance_PlayerOutsideDistance_ReturnsFalse()
        {
            _state.TargetSpeed = TargetSpeedOutsideDistance;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player1);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsInScoreDistance_NegativeSpeedDifference_ReturnsFalse()
        {
            const float negativeSpeed = -5f;  // Specific test case for negative speed
            _player1.Speed = negativeSpeed;
            _state.TargetSpeed = 0f;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player1);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsInScoreDistance_SecondPlayerWithinDistance_ReturnsTrue()
        {
            _state.TargetSpeed = Player2StartSpeed;

            var result = _gameRules.IsInScoreDistance(_config, _state, _player2);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInScoreDistance_NullPlayerData_Throws()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.IsInScoreDistance(_config, _state, null));
        }

        #endregion

        #region IsEndConditionMet

        [Test]
        public void IsEndConditionMet_RaceEnded_ReturnsTrue()
        {
            _state.RaceEnded = true;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsEndConditionMet_OnlyOnePlayerExceedsRaceDistance_ReturnsFalse()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SimPosition = PlayerSimPositionBeyondRace;
            _state.RowerDatas[GameRunner.RowerId.Second].SimPosition = PlayerSimPositionBelowRace;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsEndConditionMet_BothPlayersExceedRaceDistance_ReturnsTrue()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SimPosition = PlayerSimPositionBeyondRace;
            _state.RowerDatas[GameRunner.RowerId.Second].SimPosition = PlayerSimPositionBeyondRace;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsEndConditionMet_PlayerDoesNotExceedRaceDistance_ReturnsFalse()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SimPosition = PlayerSimPositionBelowRace;

            var result = _gameRules.IsEndConditionMet(_config, _state);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsEndConditionMet_NullState_Throws()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.IsEndConditionMet(_config, null));
        }

        #endregion

        #region AdjustScores

        [Test]
        public void AdjustScores_PlayerInScoreZone_IncreasesScoreTime()
        {
            _state.RowerDatas[GameRunner.RowerId.First].Speed = Player1StartSpeed;
            _state.TargetSpeed = Player1StartSpeed;
            float initialScoreTime = _state.RowerDatas[GameRunner.RowerId.First].ScoreTime;

            _gameRules.AdjustScores(_config, _state, DeltaTime);

            Assert.Greater(_state.RowerDatas[GameRunner.RowerId.First].ScoreTime, initialScoreTime);
        }

        [Test]
        public void AdjustScores_PlayerOutOfScoreZone_DoesNotIncreaseScoreTime()
        {
            _state.RowerDatas[GameRunner.RowerId.First].Speed = OutOfScoreZoneSpeed;
            _state.TargetSpeed = Player1StartSpeed;
            float initialScoreTime = _state.RowerDatas[GameRunner.RowerId.First].ScoreTime;

            _gameRules.AdjustScores(_config, _state, DeltaTime);

            Assert.AreEqual(initialScoreTime, _state.RowerDatas[GameRunner.RowerId.First].ScoreTime);
        }

        [Test]
        public void AdjustScores_PlayerHasFinishedRace_DoesNotIncreaseScoreTime()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SimPosition = PlayerSimPositionBeyondRace;
            float initialScoreTime = _state.RowerDatas[GameRunner.RowerId.First].ScoreTime;

            _gameRules.AdjustScores(_config, _state, DeltaTime);

            Assert.AreEqual(initialScoreTime, _state.RowerDatas[GameRunner.RowerId.First].ScoreTime);
        }

        [Test]
        public void AdjustScores_NullState_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.AdjustScores(_config, null, DeltaTime));
        }

        [Test]
        public void AdjustScores_NullConfig_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.AdjustScores(null, _state, DeltaTime));
        }

        #endregion

        #region AdjustRowerSpeeds

        [Test]
        public void AdjustRowerSpeeds_PlayerSpeedIncreasesWithAcceleration()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SpeedUp = true;
            float initialSpeed = _state.RowerDatas[GameRunner.RowerId.First].Speed;

            _gameRules.AdjustRowerSpeeds(_state, DeltaTime);
            Debug.Log($"Final {_state.RowerDatas[GameRunner.RowerId.First].Speed}");
            Assert.Greater(_state.RowerDatas[GameRunner.RowerId.First].Speed, initialSpeed);
        }

        [Test]
        public void AdjustRowerSpeeds_PlayerSpeedDecreasesWithoutAcceleration()
        {
            _state.RowerDatas[GameRunner.RowerId.First].SpeedUp = false;
            float initialSpeed = _state.RowerDatas[GameRunner.RowerId.First].Speed;

            _gameRules.AdjustRowerSpeeds(_state, DeltaTime);

            Assert.Less(_state.RowerDatas[GameRunner.RowerId.First].Speed, initialSpeed);
        }

        [Test]
        public void AdjustRowerSpeeds_NullState_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.AdjustRowerSpeeds(null, DeltaTime));
        }

        #endregion

        #region AdjustRowerPositions

        [Test]
        public void AdjustRowerPositions_PlayerPositionIncreasesWithSpeed()
        {
            _state.RowerDatas[GameRunner.RowerId.First].Speed = Player1StartSpeed;
            float initialPosition = _state.RowerDatas[GameRunner.RowerId.First].SimPosition;

            _gameRules.AdjustRowerPositions(_state, DeltaTime);

            Assert.Greater(_state.RowerDatas[GameRunner.RowerId.First].SimPosition, initialPosition);
        }

        [Test]
        public void AdjustRowerPositions_PlayerPositionRemainsIfSpeedZero()
        {
            _state.RowerDatas[GameRunner.RowerId.First].Speed = 0f;
            float initialPosition = _state.RowerDatas[GameRunner.RowerId.First].SimPosition;

            _gameRules.AdjustRowerPositions(_state, DeltaTime);

            Assert.AreEqual(initialPosition, _state.RowerDatas[GameRunner.RowerId.First].SimPosition);
        }

        [Test]
        public void AdjustRowerPositions_NullState_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.AdjustRowerPositions(null, DeltaTime));
        }

        #endregion

        #region SetNewTargetSpeed

        [Test]
        public void SetNewTargetSpeed_ChangesTargetSpeedAfterInterval()
        {
            _config.TargetSpeedChangeInterval = TargetSpeedChangeInterval;
            _config.MinTargetSpeed = 5f;  // Set a minimum target speed value
            _config.MaxTargetSpeed = 15f; // Set a maximum target speed value
    
            _state.RaceTimeElapsed = 2.0f; // Set elapsed time to ensure it is greater than interval
            _state.LastTargetSpeedChange = 0.5f; // Last change happened before the interval

            _gameRules.SetNewTargetSpeed(_config, _state);

            // Verify target speed has been changed and is not equal to default 0.0f
            Assert.AreNotEqual(0f, _state.TargetSpeed, "Expected target speed to be changed but it was not.");
        }

        [Test]
        public void SetNewTargetSpeed_DoesNotChangeTargetSpeedBeforeInterval()
        {
            _config.TargetSpeedChangeInterval = TargetSpeedChangeInterval;
            _state.RaceTimeElapsed = 0.5f;
            _state.LastTargetSpeedChange = 0f;

            _gameRules.SetNewTargetSpeed(_config, _state);

            // Verify target speed has not been changed
            Assert.AreEqual(0f, _state.TargetSpeed, "Expected target speed to remain 0.0f but it was changed.");
        }

        [Test]
        public void SetNewTargetSpeed_SetsTargetSpeedWithinRange()
        {
            _config.TargetSpeedChangeInterval = TargetSpeedChangeInterval;
            _state.RaceTimeElapsed = 1.5f;
            _state.LastTargetSpeedChange = 0f;
            _config.MinTargetSpeed = 5f;
            _config.MaxTargetSpeed = 15f;

            _gameRules.SetNewTargetSpeed(_config, _state);

            // Verify target speed is within the defined range
            Assert.GreaterOrEqual(_state.TargetSpeed, 5f);
            Assert.LessOrEqual(_state.TargetSpeed, 15f);
        }

        [Test]
        public void SetNewTargetSpeed_NullConfig_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.SetNewTargetSpeed(null, _state));
        }

        [Test]
        public void SetNewTargetSpeed_NullState_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _gameRules.SetNewTargetSpeed(_config, null));
        }

        #endregion
    }
}
