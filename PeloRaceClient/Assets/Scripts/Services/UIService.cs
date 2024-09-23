using System;
using System.Collections.Generic;
using Codice.CM.Common;
using GameLogic;

namespace Services
{
    public class UIService
    {
        public static UIService Instance => _instance;
        private static UIService _instance;
        
        public float RaceTime;
        public float PlayerScore;
        public float TargetSpeed;
        public float PlayerSpeed;
        public float RaceDistance;
        public float PlayerPosition;
        public bool IsScoring;
        public bool Accelerating;
        
        private GameRunner _gameRunner;
        
        
        #region Events
        
        public Action StateUpdated = () => { };
        public Action GameOver = () => { };

        #endregion
        
        public UIService(GameRunner runner)
        {
            _instance = this;
            _gameRunner = runner;
            _gameRunner.StateUpdated += GameStateUpdated;
            _gameRunner.GameOver += GameOver;
            GameStateUpdated();
        }
        
        ~UIService()
        {
            if (_gameRunner != null)
            {
                _gameRunner.GameOver -= GameOver;
                _gameRunner.StateUpdated -= GameStateUpdated;
            }
            _instance = null;
            _gameRunner = null;
        }
        

        private void GameStateUpdated()
        {
            PlayerScore = _gameRunner.GetScore(GameRunner.RowerId.First);
            RaceTime = _gameRunner.CurrentState.RaceTimeElapsed;
            TargetSpeed = _gameRunner.CurrentState.TargetSpeed;
            PlayerSpeed = _gameRunner.CurrentState.RowerDatas[GameRunner.RowerId.First].Speed;
            RaceDistance = _gameRunner.GetTotalRaceDistance();
            PlayerPosition = _gameRunner.CurrentState.RowerDatas[GameRunner.RowerId.First].SimPosition;
            IsScoring = _gameRunner.IsScoring(GameRunner.RowerId.First);
            Accelerating = _gameRunner.CurrentState.RowerDatas[GameRunner.RowerId.First].SpeedUp;
            StateUpdated?.Invoke();
        }
    }
}