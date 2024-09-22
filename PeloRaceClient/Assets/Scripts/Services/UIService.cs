using System;
using System.Collections.Generic;
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
        
        private GameRunner _gameRunner;
        
        
        #region Events
        
        public Action StateUpdated = () => { };

        #endregion
        
        public UIService(GameRunner runner)
        {
            _instance = this;
            _gameRunner = runner;
            _gameRunner.StateUpdated += GameStateUpdated;
            GameStateUpdated();
        }
        
        ~UIService()
        {
            
            if (_gameRunner != null) _gameRunner.StateUpdated -= GameStateUpdated;
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
            StateUpdated?.Invoke();
        }
    }
}