using System.Collections;
using GameLogic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.UI;

namespace Views
{
    public class GameUIView : MonoBehaviour
    {
        private UIService _service;
        [SerializeField] private TextMeshProUGUI _gameTime;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _targetSpeed;
        [SerializeField] private TextMeshProUGUI _currentSpeed;
        [SerializeField] private MeterWithCursor _speedTargetMeter;
        [SerializeField] private TextMeshProUGUI _currentProgress;
        [SerializeField] private Image _progressMeter;
        [SerializeField] private Image _scoringIndicator;
        [SerializeField] private GameObject _upImage;
        [SerializeField] private GameObject _downImage;
        
        
        [Header("Configurable Settings")]
        [SerializeField] private float _speedTargetDisplayBounds = 5;
        [SerializeField] private float _updateFrequency = .1f;
        
        // TODO: These should be moved to a scriptable object for easier config & testing
        [SerializeField] private Color _scoringColor;
        [SerializeField] private Color _notScoringColor;
        
        private Coroutine _updateRoutine;
        private bool _updateAvailable;
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

        IEnumerator StateUpdateWait()
        {
            yield return new WaitForSeconds(_updateFrequency);
            if (_updateAvailable)
            {
                StateUpdated();
            }
            _updateRoutine = null;
        }

        private void StateUpdated()
        {
            
            if (_updateRoutine != null)
            {
                _updateAvailable = true;
                return;
            }
            
            _gameTime.text = $"Time: {_service.RaceTime:0.0}";
            _score.text = $"Score: {_service.PlayerScore:0.0}";
            _currentSpeed.text = $"Speed: {_service.PlayerSpeed:0.0}";
            _targetSpeed.text = $"Target Speed: {_service.TargetSpeed:0.0}";
            _currentProgress.text = $"Progress: {_service.PlayerPosition:0} / {_service.RaceDistance:0} Meters";
            _speedTargetMeter.SetCursorPosition(Mathf.InverseLerp(_service.TargetSpeed - _speedTargetDisplayBounds, _service.TargetSpeed + _speedTargetDisplayBounds, _service.PlayerSpeed));
            _scoringIndicator.color = _service.IsScoring ? _scoringColor : _notScoringColor;
            _progressMeter.fillAmount = _service.PlayerPosition / _service.RaceDistance;
            _upImage.SetActive(_service.Accelerating);
            _downImage.SetActive(!_service.Accelerating);
            _updateRoutine = StartCoroutine(StateUpdateWait());
            _updateAvailable = false;
        }
    }
}