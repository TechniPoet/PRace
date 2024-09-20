using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace SystemLevel
{
    public class ExitGame : MonoBehaviour
    {
        [UsedImplicitly]
        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
        }
    }
}