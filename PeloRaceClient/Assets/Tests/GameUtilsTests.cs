using System;
using Data;
using GameLogic;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests
{
    public class GameUtilsTests
    {
        [Test]
        public void SimToViewPosition_ShouldCalculateCorrectPosition()
        {
            var viewStart = new Vector3(0f, 0f, 0f);
            var direction = new Vector3(1f, 0f, 0f);
            var result = GameUtils.SimToViewPosition(5f, viewStart, direction);
            Assert.AreEqual(new Vector3(5f, 0f, 0f), result);
        }

        [Test]
        public void SimToViewPosition_ShouldHandleNegativeSimPosition()
        {
            var viewStart = new Vector3(0f, 0f, 0f);
            var direction = new Vector3(1f, 0f, 0f);
            var result = GameUtils.SimToViewPosition(-5f, viewStart, direction);
            Assert.AreEqual(new Vector3(-5f, 0f, 0f), result);
        }

        [Test]
        public void CalculateScore_ShouldReturnZero_WhenRowerDataIsNull()
        {
            var raceConfig = ScriptableObject.CreateInstance<RaceConfig>();
            raceConfig.ScorePerSecond = 10f;

            Assert.Throws<NullReferenceException>(() => GameUtils.CalculateScore(raceConfig, null));
            
        }

        [Test]
        public void CalculateScore_ShouldReturnZero_WhenScoreTimeIsZero()
        {
            var raceConfig = ScriptableObject.CreateInstance<RaceConfig>();
            raceConfig.ScorePerSecond = 10f;

            var rowerData = new GameRunner.RowerData();
            rowerData.ScoreTime = 0f;

            var score = GameUtils.CalculateScore(raceConfig, rowerData);
            Assert.AreEqual(0f, score);
        }

        [Test]
        public void CalculateScore_ShouldHandleNegativeScoreTime()
        {
            var raceConfig = ScriptableObject.CreateInstance<RaceConfig>();
            raceConfig.ScorePerSecond = 10f;

            var rowerData = new GameRunner.RowerData();
            rowerData.ScoreTime = -5f;

            var score = GameUtils.CalculateScore(raceConfig, rowerData);
            Assert.AreEqual(-50f, score);
        }
    }
}