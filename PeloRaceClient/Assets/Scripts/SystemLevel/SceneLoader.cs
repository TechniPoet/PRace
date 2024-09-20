using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SystemLevel
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad;
        private bool _debounce;
        
        [UsedImplicitly]
        public void LoadScene()
        {
            if (_debounce) return;
            _debounce = true;
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}