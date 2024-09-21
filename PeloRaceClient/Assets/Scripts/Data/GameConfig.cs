using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "PeloRace/GameConfig", order = 1)]
    public class GameConfig  : ScriptableObject
    {
        #region Properties

        [SerializeField] private List<RaceConfig> _raceConfig;
        private readonly Dictionary<string, RaceConfig> _dataDict = new();

        #endregion

#if UNITY_EDITOR

        public void Setup(List<RaceConfig> newRaces)
        {
            _raceConfig = newRaces;
            LoadConfig();
        }
        
#endif
        
        // OnEnable runs at instantiation editor & runtime
        private void OnEnable()
        {
            if (_raceConfig == null)
            {
                // Initialize list and move on to allow for instantiation from code,
                _raceConfig = new();
                return;
                // The setup below is intended for already instantiated initialization at runtime;
            }

            LoadConfig();
        }

        /// <summary>
        /// Loads data into quick access dictionary
        /// </summary>
        private void LoadConfig()
        {
            // Load race data at start of run time 
            foreach (RaceConfig raceConfig in _raceConfig)
            {
                _dataDict.Add(raceConfig.name, raceConfig);
            }
        }

        /// <summary>
        /// Returns array of race key and display names for all available races
        /// </summary>
        /// <returns></returns>
        public (string, string)[] GetRaceDisplayData()
        {
            return _raceConfig.ConvertAll(data => (data.name, data.DisplayName)).ToArray();
        }

        /// <summary>
        /// Returns race data by name
        /// </summary>
        /// <param name="raceKey">file name of race file</param>
        /// <returns></returns>
        public RaceConfig GetRaceConfig(string raceKey)
        {
            Assert.IsTrue(_dataDict.ContainsKey(raceKey));
            return _dataDict[raceKey];
        }
    }
}