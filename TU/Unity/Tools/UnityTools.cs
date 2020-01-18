using System;
using UnityEngine.SceneManagement;

namespace TU.Unity.Tools
{
    public static class UnityTools
    {
        public static class Scene
        {
            public static int? GetLoadedSceneID(Func<string, bool> predict)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var currentLevelScene = SceneManager.GetSceneAt(i);
                    if (predict.Invoke(currentLevelScene.name)) return currentLevelScene.buildIndex;
                }

                return null;
            }

            public static bool IsSceneLoaded(Func<string, bool> predict)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var currentLevelScene = SceneManager.GetSceneAt(i);
                    if (predict.Invoke(currentLevelScene.name)) return true;
                }

                return false;
            }


            public static void RestartScene(Func<string, bool> predict)
            {
                if (SceneManager.sceneCount == 1)
                {
                    SceneManager.LoadScene(GetLoadedSceneID(predict).Value);
                }
                else
                {
                    var currentLevelSceneID = GetLoadedSceneID(predict).Value;
                    SceneManager.sceneUnloaded += ReloadScene;
                    SceneManager.UnloadSceneAsync(currentLevelSceneID);
                }
            }

            private static void ReloadScene(UnityEngine.SceneManagement.Scene scene)
            {
                SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Additive);
                SceneManager.sceneUnloaded -= ReloadScene;
            }


            public static bool IsMainScene(string sceneName) => sceneName.ToLower().Contains("main");
        }
    
    
    }
}