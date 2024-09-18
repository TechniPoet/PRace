using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace PeloRace.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewGameData", menuName = "PeloRace/GameData", order = 1)]
    public class GameData  : ScriptableObject
    {
        #region Properties

        [SerializeField] private List<RaceData> _raceData;
        private Dictionary<string, RaceData> _dataDict = new Dictionary<string, RaceData>();

        #endregion
        
        private void OnEnable()
        {
            // Load race data at start of run time 
            // OnEnable runs at instantiation editor & runtime
            foreach (RaceData raceData in _raceData)
            {
                _dataDict.Add(raceData.name, raceData);
            }
        }

        /// <summary>
        /// Returns array of race key and display names for all available races
        /// </summary>
        /// <returns></returns>
        public (string, string)[] GetRaceDisplayData()
        {
            return _raceData.ConvertAll(data => (data.name, data.DisplayName)).ToArray();
        }

        /// <summary>
        /// Returns race data by name
        /// </summary>
        /// <param name="raceKey">file name of race file</param>
        /// <returns></returns>
        public RaceData GetRaceData(string raceKey)
        {
            Assert.IsTrue(_dataDict.ContainsKey(raceKey));
            return _dataDict[raceKey];
        }
    }
}