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
        public static GameRunner Instance => _instance;
#endregion

#region Containers

        /// <summary>
        /// Container class that holds game state
        /// </summary>
        [Serializable]
        public class GameState
        {
            public bool RaceEnded;
            public float RaceTimeElapsed;

            public float LastTargetSpeedChange;

            public float TargetSpeed;
            // Score time is time in score zone. For multiplayer, we'd want to incorporate this in a per rower data
            
            public Dictionary<RowerId, RowerData> RowerDatas = new()
            {
                {RowerId.First, new RowerData()},
                {RowerId.Second, new RowerData()},
            };
        }

        [Serializable]
        public class RowerData
        {
            public float SimPosition;
            public float Speed;
            public float TargetSpeed;
            public float LerpToSpeedInterval;
            public float ScoreTime;

            public void Setup(PlayerConfig config)
            {
                Speed = config.StartSpeed;
                TargetSpeed = Speed;
                LerpToSpeedInterval = config.LerpToSpeedInterval;
            }
        }


#endregion

        public enum RowerId
        {
            First = 0,
            Second = 1
        }


#region Properties

        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private string _currentRaceKey;
        private RaceConfig _currentRaceConfig;
        
        private Coroutine _gameTickRoutine;

        // Service instance holding here is for cleanup but moving forward,
        // I would not want GameRunner to hold the service references.
        private RowerViewService _viewService;
        private InputService _inputService;
        private IGameRules _rules;

        public GameState CurrentState;

        #endregion
        
        
        #region Events
        public event Action StateUpdated = () => { };
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
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            _viewService = new RowerViewService(this);
            _inputService = new InputService(this);
        }

        private void OnDestroy()
        {
            _viewService = null;
            _inputService = null;
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

#endregion
        

        public void StartGame()
        {
            _rules = new DefaultGameRules();
            CurrentState = new();
            CurrentState.RowerDatas[RowerId.First].Setup(_currentRaceConfig.ControlledPlayer);
            CurrentState.RowerDatas[RowerId.Second].Setup(_currentRaceConfig.BotPlayer);
            _gameTickRoutine = StartCoroutine(RunGameTicks());
        }

        public void EndGame()
        {
            if (_gameTickRoutine != null) StopCoroutine(_gameTickRoutine);
        }

        public void AdjustRowerSpeed(RowerId id, bool up)
        {
            if (CurrentState is { RaceEnded: true }) return;
            _rules.AdjustRowerTargetSpeed(_currentRaceConfig, CurrentState, id, up);
        }

        private IEnumerator RunGameTicks()
        {
            yield return new WaitForSeconds(2f);
            do
            {
                // Not worrying about bot ai atm.
                // Todo: better bot ai
                CurrentState.RowerDatas[RowerId.Second].TargetSpeed = CurrentState.TargetSpeed;
                StateUpdated?.Invoke();
                yield return null;
            } while (_rules.SimulationTick(_currentRaceConfig, CurrentState, Time.deltaTime));
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Race") return;
            
            LoadRaceData();
            StartGame();
        }

        public void SetRace(string raceKey)
        {
            _currentRaceKey = raceKey;
        }

        public void LoadRaceData()
        {
            _currentRaceConfig = gameConfig.GetRaceConfig(_currentRaceKey);
        }
        
        
    }
}