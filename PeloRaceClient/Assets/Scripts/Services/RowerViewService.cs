using System;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using Utils;

namespace Services
{
    public class RowerViewService
    {
        private class RowerInstanceData
        {
            public Vector3 CurrentPosition;
            public Vector3 StartPosition;
        }
        #region Static

        // I don't love singletons usually and usually opt for a service locator pattern,
        // but I'm using this for the sake of expediency
        public static RowerViewService Instance => _instance;
        private static RowerViewService _instance;

        #endregion

        private GameRunner _gameRunner;
        private Dictionary<GameRunner.RowerId, RowerInstanceData> _rowerData = new();
        
        #region Events
        
        public Action StateUpdated = () => { };

        #endregion

        public RowerViewService(GameRunner runner)
        {
            _instance = this;
            _gameRunner = runner;
            _gameRunner.StateUpdated += GameStateUpdated;
            GameStateUpdated();
        }

        ~RowerViewService()
        {
            if (_gameRunner != null) _gameRunner.StateUpdated -= GameStateUpdated;
            _instance = null;
            _gameRunner = null;
        }

        /// <summary>
        /// Updates ViewModel Service data according to game state changes.
        /// Broadcast that changes have been made.
        /// </summary>
        private void GameStateUpdated()
        {
            foreach (var view in _rowerData)
            {
                view.Value.CurrentPosition = 
                    GameUtils.SimToViewPosition(_gameRunner.CurrentState.RowerDatas[view.Key].SimPosition,
                        view.Value.StartPosition, Vector3.forward);
            }
            StateUpdated?.Invoke();
        }

        /// <summary>
        /// Join view with id and position data so their positions can be updated
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startPosition"></param>
        public void Join(GameRunner.RowerId id, Vector3 startPosition)
        {
            _rowerData.Add(id, new RowerInstanceData(){StartPosition = startPosition, CurrentPosition = startPosition});
        }

        public void Leave(GameRunner.RowerId id)
        {
            _rowerData.Remove(id);
        }

        public Vector3 GetPosition(GameRunner.RowerId id)
        {
            return _rowerData[id].CurrentPosition;
        }
    }
}