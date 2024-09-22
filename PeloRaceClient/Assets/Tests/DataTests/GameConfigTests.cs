using System.Collections.Generic;
using Data;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class GameConfigTests
    {
        private GameConfig m_GameConfig;
        private RaceConfig _raceData1;
        //private RaceData _raceData2;

        private const string FileNamePrefix = "File_";
        private const string Race1DisplayName = "Test1";
        
        [SetUp]
        public void Setup()
        {
            _raceData1 = ScriptableObject.CreateInstance<RaceConfig>();
            //_raceData2 = ScriptableObject.CreateInstance<RaceData>();
            m_GameConfig = ScriptableObject.CreateInstance<GameConfig>();

            _raceData1.name = $"{FileNamePrefix}{Race1DisplayName}";
            _raceData1.RaceDistance = 1;
            _raceData1.DisplayName = Race1DisplayName;
            m_GameConfig.Setup(new List<RaceConfig>() {_raceData1});
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_raceData1);
            Object.DestroyImmediate(m_GameConfig);
        }

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

        [Test]
        public void GameConfig_GetRaceConfig_ShouldThrowExceptionForUnknownRace()
        {
            var gameConfig = ScriptableObject.CreateInstance<GameConfig>();
            var race1 = ScriptableObject.CreateInstance<RaceConfig>();
            race1.name = "Race1";
            race1.DisplayName = "First Race";

            gameConfig.Setup(new List<RaceConfig> { race1 });

            Assert.Throws<UnityEngine.Assertions.AssertionException>(() =>
            {
                gameConfig.GetRaceConfig("UnknownRace");
            });
        }
    }
}