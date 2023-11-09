using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public static class BootstrapLoader
    {
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void BootstrapSceneCheck()
        {
            if (IsBootstrapLoaded())
                return;

            Debug.LogError("Non bootstrap scene loading as first detected, forcing bootstrap scene loading");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private static bool IsBootstrapLoaded()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).buildIndex == 0)
                    return true;
            }

            return false;
        }
#endif
    }
}
