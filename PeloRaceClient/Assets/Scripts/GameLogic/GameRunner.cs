using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Random = UnityEngine.Random;

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
            public bool SpeedUp = true;
            public float SpeedChangePerSecond;
            public float ScoreTime;
            public float MinSpeed;
            public float MaxSpeed;

            public void Setup(PlayerConfig config)
            {
                Speed = config.StartSpeed;
                SpeedChangePerSecond = config.SpeedChangePerSecond;
                MinSpeed = config.MinSpeed;
                MaxSpeed = config.MaxSpeed;
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
        private UIService _uiService;
        private IGameRules _rules;

        public GameState CurrentState;

        #endregion
        
        
        #region Events
        public event Action StateUpdated = () => { };
        public event Action GameOver = () => { };
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
            _uiService = new UIService(this);
        }

        private void OnDestroy()
        {
            _viewService = null;
            _inputService = null;
            _uiService = null;
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

#endregion
        

        /// <summary>
        /// Starts a new race game, initializing game state and starting the simulation loop.
        /// </summary>
        public void StartGame()
        {
            _rules = new DefaultGameRules();
            CurrentState = new();
            CurrentState.RowerDatas[RowerId.First].Setup(_currentRaceConfig.ControlledPlayer);
            CurrentState.RowerDatas[RowerId.Second].Setup(_currentRaceConfig.BotPlayer);
            _gameTickRoutine = StartCoroutine(RunGameTicks());
        }

        /// <summary>
        /// Ends the current game, stopping the simulation and triggering the GameOver event.
        /// </summary>
        public void EndGame()
        {
            if (_gameTickRoutine != null) StopCoroutine(_gameTickRoutine);
            GameOver?.Invoke();
        }

        /// <summary>
        /// Adjusts the speed of a specified rower by increasing or decreasing it based on user input.
        /// </summary>
        /// <param name="id">ID of the rower to adjust.</param>
        /// <param name="up">True to increase speed, false to decrease speed.</param>
        public void AdjustRowerSpeed(RowerId id, bool up)
        {
            // don't register speed change if game is over or game is paused
            if (CurrentState is { RaceEnded: true } || Time.timeScale == 0) return;
            _rules.AdjustRowerAcceleration(_currentRaceConfig, CurrentState, id, up);
        }

        /// <summary>
        /// Coroutine that manages the simulation ticks of the game, updating game state at each frame.
        /// </summary>
        private IEnumerator RunGameTicks()
        {
            yield return new WaitForSeconds(1f);
            do
            {
                BotBehavior();
                StateUpdated?.Invoke();
                yield return null;
            } while (_rules.SimulationTick(_currentRaceConfig, CurrentState, Time.deltaTime));

            EndGame();
        }

        /// <summary>
        /// Simulates bot behavior, adjusting its speed to match the target speed with some randomness.
        /// </summary>
        private void BotBehavior()
        {
            // some randomness for the fun of it
            if (Random.Range(0,1) < 0.2f) return;
            var bot = CurrentState.RowerDatas[RowerId.Second];
            bot.SpeedUp = bot.Speed < CurrentState.TargetSpeed;
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != "Race") return;
            
            LoadRaceData();
            StartGame();
        }

        /* TODO: setup race selection in main menu
        public void SetRace(string raceKey)
        {
            _currentRaceKey = raceKey;
        }
        */
        
        /// <summary>
        /// Handles scene load events, starting the game when the race scene is loaded.
        /// </summary>
        public void LoadRaceData()
        {
            _currentRaceConfig = gameConfig.GetRaceConfig(_currentRaceKey);
        }

        public float GetScore(RowerId id)
        {
            return _currentRaceConfig == null ? 0 : GameUtils.CalculateScore(_currentRaceConfig, CurrentState.RowerDatas[id]);
        }

        public float GetTotalRaceDistance()
        {
            return _currentRaceConfig == null ? 0 : _currentRaceConfig.RaceDistance;
        }

        public bool IsScoring(RowerId id)
        {
            return _rules != null && _rules.IsInScoreDistance(_currentRaceConfig, CurrentState, CurrentState.RowerDatas[id]);
        }
    }
}