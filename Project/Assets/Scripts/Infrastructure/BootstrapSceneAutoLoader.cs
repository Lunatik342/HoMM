using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public static class BootstrapSceneAutoLoader
    {
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void BootstrapSceneCheck()
        {
            if (!ShouldLoadBootstrapScene())
                return;

            Debug.LogError("Non bootstrap scene loading as first detected, forcing bootstrap scene loading");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private static bool ShouldLoadBootstrapScene()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.sceneCountInBuildSettings == SceneManager.GetSceneAt(i).buildIndex)
                {
                    return false;
                }

                if (SceneManager.GetSceneAt(i).buildIndex == 0)
                {
                    return false;
                }
            }

            return true;
        }
#endif
    }
}
