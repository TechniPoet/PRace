using System.Collections.Generic;
using Data;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SODataTester
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
        public void GetRaceDisplayDataTest()
        {
            var displayData = m_GameConfig.GetRaceDisplayData();
            Assert.IsTrue(displayData.Length == 1);
            Assert.IsTrue(displayData[0].Item1 == $"{FileNamePrefix}{Race1DisplayName}");
            Assert.IsTrue(displayData[0].Item2 == Race1DisplayName);
        }

        public void GetRaceDataTest()
        {
            
        }
    }
}