using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Views.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseParent;
        [SerializeField] private Button _startingSelectedButton;
        [SerializeField] private InputActionReference _pauseInput;
        
        private bool _pauseState = false;
        private float _pauseTime = 0;

        private void Start()
        {
            _pauseInput.action.performed += PauseActionPerformed;
        }

        private void OnDestroy()
        {
            _pauseInput.action.performed -= PauseActionPerformed;
        }

        private void PauseActionPerformed(InputAction.CallbackContext obj)
        {
            Pause();
        }

        [UsedImplicitly]
        public void Pause()
        {
            if(_pauseTime + 0.2f > Time.realtimeSinceStartup)
            {
                return;
            }

            _pauseTime = Time.realtimeSinceStartup;
            _pauseState = !_pauseState;
            _pauseParent.SetActive(_pauseState);
            Time.timeScale = _pauseState ? 0.0f : 1.0f;
            if (_pauseState)
            {
                _startingSelectedButton.Select();
            }
        }
    }
}