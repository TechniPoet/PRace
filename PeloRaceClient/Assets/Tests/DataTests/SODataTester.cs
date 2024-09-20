using System.Collections.Generic;
using Data;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SODataTester
    {
        private GameData _gameData;
        private RaceData _raceData1;
        //private RaceData _raceData2;

        private const string FileNamePrefix = "File_";
        private const string Race1DisplayName = "Test1";
        
        [SetUp]
        public void Setup()
        {
            _raceData1 = ScriptableObject.CreateInstance<RaceData>();
            //_raceData2 = ScriptableObject.CreateInstance<RaceData>();
            _gameData = ScriptableObject.CreateInstance<GameData>();

            _raceData1.name = $"{FileNamePrefix}{Race1DisplayName}";
            _raceData1.RaceDistance = 1;
            _raceData1.DisplayName = Race1DisplayName;
            _gameData.Setup(new List<RaceData>() {_raceData1});
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_raceData1);
            Object.DestroyImmediate(_gameData);
        }

        [Test]
        public void GetRaceDisplayDataTest()
        {
            var displayData = _gameData.GetRaceDisplayData();
            Assert.IsTrue(displayData.Length == 1);
            Assert.IsTrue(displayData[0].Item1 == $"{FileNamePrefix}{Race1DisplayName}");
            Assert.IsTrue(displayData[0].Item2 == Race1DisplayName);
        }

        public void GetRaceDataTest()
        {
            
        }
    }
}