using System.Collections;
using Services;
using TMPro;
using UnityEngine;

namespace Views
{
    public class GameOverView : MonoBehaviour
    {
        private UIService _service;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private TextMeshProUGUI _endGameText;
        
        private IEnumerator Start()
        {
            // Wait for RowerViewService to instantiate to avoid race conditions
            while (UIService.Instance == null)
            {
                yield return null;
            }
            
            _service = UIService.Instance;
            _service.GameOverEvent += GameEnded;
        }

        private void OnDestroy()
        {
            if (_service != null) _service.GameOverEvent -= GameEnded;
        }

        private void GameEnded()
        {
            Debug.Log("GameOver");
            _gameOverScreen.SetActive(true);
            _endGameText.text = $"Congratulations!\n \nScore:{_service.PlayerScore:0.0}";
        }
    }
}