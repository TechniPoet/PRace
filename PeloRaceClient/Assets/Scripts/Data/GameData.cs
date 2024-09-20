using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Data
{
    [CreateAssetMenu(fileName = "NewGameData", menuName = "PeloRace/GameData", order = 1)]
    public class GameData  : ScriptableObject
    {
        #region Properties

        [SerializeField] private List<RaceData> _raceData;
        private Dictionary<string, RaceData> _dataDict = new();

        #endregion

#if UNITY_EDITOR

        public void Setup(List<RaceData> newRaces)
        {
            _raceData = newRaces;
            LoadData();
        }
        
#endif
        
        // OnEnable runs at instantiation editor & runtime
        private void OnEnable()
        {
            if (_raceData == null)
            {
                // Initialize list and move on to allow for instantiation from code,
                _raceData = new();
                return;
                // The setup below is intended for already instantiated initialization at runtime;
            }

            LoadData();
        }

        /// <summary>
        /// Loads data into quick access dictionary
        /// </summary>
        private void LoadData()
        {
            // Load race data at start of run time 
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