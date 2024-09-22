using System;
using System.Collections;
using GameLogic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Services
{
    public class PlayerInputProvider : MonoBehaviour
    {
        [SerializeField] private InputActionReference _upInput;
        [SerializeField] private InputActionReference _downInput;

        [SerializeField] private GameRunner.RowerId _id;

        private InputService _service;

        private IEnumerator Start()
        {
            while (InputService.Instance == null)
            {
                yield return null;
            }

            _service = InputService.Instance;
            _upInput.action.performed += UpActionOnperformed;
            _downInput.action.performed += DownActionOnperformed;
        }

        private void DownActionOnperformed(InputAction.CallbackContext obj)
        {
            _service.AdjustRowerSpeed(_id, false);
        }
        
        private void UpActionOnperformed(InputAction.CallbackContext obj)
        {
            _service.AdjustRowerSpeed(_id, true);
        }

        private void OnDestroy()
        {
            _upInput.action.performed -= UpActionOnperformed;
            _downInput.action.performed -= DownActionOnperformed;
            _service = null;
        }
    }
}