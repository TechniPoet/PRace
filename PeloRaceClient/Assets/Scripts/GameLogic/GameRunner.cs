using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    /// <summary>
    /// For the sake of this project,
    /// GameRunner will act as our API and handle passing game state data to and from services while holding simulation logic
    /// </summary>
    public class GameRunner : MonoBehaviour
    {
#region Static

        // I don't love singletons usually and usually opt for a service locator pattern,
        // but I'm using this for the sake of expediency
        private static GameRunner _instance;

#endregion

#region Events
        public event Action StateUpdated = () => { };

#endregion
        public enum RowerId
        {
            First = 0,
            Second = 1
        }


#region Properties

        [SerializeField] private GameData _gameData;
        [SerializeField] private string _currentRaceKey;
        [SerializeField] private RaceData _currentRaceData;

        private readonly Dictionary<RowerId, float> _rowerPositions = new();
        public Dictionary<RowerId, float> RowerPositions => _rowerPositions;

        private Coroutine _gameTickRoutine;

        // Service instance holding here is for cleanup but moving forward,
        // I would not want GameRunner to hold the service references.
        private RowerViewService _viewService;
        

        #endregion

#region Setup Teardown

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            _viewService = new RowerViewService(this);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
        }

        #endregion
        

        public void StartGame()
        {
            _gameTickRoutine = StartCoroutine(RunGameTicks());
        }

        public void EndGame()
        {
            if (_gameTickRoutine != null) StopCoroutine(_gameTickRoutine);
        }

        private IEnumerator RunGameTicks()
        {
            yield return null;
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Race") return;
            LoadRaceData();
        }

        public void SetRace(string raceKey)
        {
            _currentRaceKey = raceKey;
        }

        public void LoadRaceData()
        {
            _currentRaceData = _gameData.GetRaceData(_currentRaceKey);
        }
        
        
    }
}