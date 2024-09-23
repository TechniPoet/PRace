using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Views.UI
{
    public class MeterWithCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private RectTransform _meter;
        [SerializeField,Range(0,.02f)] private float _cursorMoveSpeed;
        private float _cursorHalfWidth;
        [Header("Debug")]
        [SerializeField, ReadOnly] private float _targetPosition;
        [SerializeField, ReadOnly] private float _currentPosition;

        private Coroutine _lerpRoutine;
        private void Awake()
        {
            _cursorHalfWidth = (_cursor.anchorMax.x - _cursor.anchorMin.x) / 2;
        }

        public void Update()
        {
            var positionDiff = Mathf.Abs(_currentPosition - _targetPosition);
            if ((positionDiff > _cursorHalfWidth))
            {
                var positionDelta = .001f * Time.deltaTime * 
                                    (_cursorMoveSpeed + 
                                     (_targetPosition < _currentPosition ? 1 : -1));
                positionDelta = positionDiff < positionDelta ? positionDiff : positionDelta;
                SetCursorPosition(_currentPosition + positionDelta);
            }
        }

        /// <summary>
        /// Set Cursor position relative to it's parent
        /// </summary>
        /// <param name="newTarget">Value 0 to 1</param>
        public void SetCursorTarget(float newTarget)
        {
            _targetPosition = Mathf.Clamp(newTarget, _cursorHalfWidth, 1 - _cursorHalfWidth);
        }
        
        /// <summary>
        /// Set Cursor position relative to it's parent
        /// </summary>
        /// <param name="progress">Value 0 to 1</param>
        public void SetCursorPosition(float progress)
        {
            _cursor.anchorMin = _cursor.anchorMin.WithX(progress - _cursorHalfWidth);
            _cursor.anchorMax = _cursor.anchorMax.WithX(progress + _cursorHalfWidth);
            _currentPosition = _cursor.anchorMin.x + _cursorHalfWidth;
        }
    }
}