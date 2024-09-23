using System;
using System.Collections;
using GameLogic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.UI;

namespace Views
{
    public class GameOverView : MonoBehaviour
    {
        private UIService _service;
        private GameObject _GameOverScreen;
        
        private IEnumerator Start()
        {
            // Wait for RowerViewService to instantiate to avoid race conditions
            while (UIService.Instance == null)
            {
                yield return null;
            }

            _service = UIService.Instance;
            _service.GameOver += GameEnded;
        }

        private void OnDestroy()
        {
            if (_service != null) _service.GameOver -= GameEnded;
        }

        private void GameEnded()
        {
            _GameOverScreen.SetActive(true);
        }
    }
}