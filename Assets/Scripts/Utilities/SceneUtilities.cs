using UnityEngine;

namespace Sheeplosion.Utilities
{
    public class SceneUtilities : MonoBehaviour
    {
        public void ChangeScenes(string a_sceneName)
        {
            Application.LoadLevel(a_sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
