using System.Collections;
using Services;
using TMPro;
using UnityEngine;

namespace Views
{
    public class GameUIView : MonoBehaviour
    {
        private UIService _service;
        [SerializeField] private TextMeshProUGUI _gameTime;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _targetSpeed;
        [SerializeField] private TextMeshProUGUI _currentSpeed;
        [SerializeField] private TextMeshProUGUI _currentProgress;
        private IEnumerator Start()
        {
            // Wait for RowerViewService to instantiate to avoid race conditions
            while (UIService.Instance == null)
            {
                yield return null;
            }

            _service = UIService.Instance;
            _service.StateUpdated += StateUpdated;
            StateUpdated();
        }

        private void StateUpdated()
        {
            _gameTime.text = $"Time: {_service.RaceTime}";
            _score.text = $"Score: {_service.PlayerScore}";
            _currentSpeed.text = $"Speed: {_service.PlayerSpeed}";
            _targetSpeed.text = $"Target Speed: {_service.TargetSpeed}";
            _currentProgress.text = $"Progress: {_service.PlayerPosition:0.00} / {_service.RaceDistance} Meters";
        }
    }
}