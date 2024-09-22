using System.Collections.Generic;
using GameLogic;

namespace Services
{
    public class UIService
    {
        public static UIService Instance => _instance;
        private static UIService _instance;
        public Dictionary<GameRunner.RowerId, float> RowerScores = new();
        public float RaceTime;
        public float PlayerScore; 
        
        private GameRunner _gameRunner;
        
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
            throw new System.NotImplementedException();
        }

        

        public void AdjustRowerSpeed(GameRunner.RowerId id, bool up)
        {
            _gameRunner.AdjustRowerSpeed(id, up);
        }
    }
}