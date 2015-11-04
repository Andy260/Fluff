using UnityEngine;

namespace Sheeplosion.GUI
{
    public class GUISystem : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        GameObject _rangeDisplayPrefab;

        [Header("References")]
        [SerializeField]
        GameObject _winMenu;
        [SerializeField]
        GameObject _lossMenu;
        [SerializeField]
        GameObject _pauseMenu;

        float _unpausedTimeScale = 0.0f;
        
        public void Awake()
        {
            // Ensure range display prefab is not null
            if (_rangeDisplayPrefab)
            {
                Debug.LogError("Range Display prefab is not set for GUI System");
            }
            else
            {
                // TODO: Instantiate Range Display prefab
            }

            // Ensure win menu reference is not null
            if (_winMenu == null)
            {
                Debug.LogError("Win Menu reference not set for GUI System");
            }

            // Ensure loss menu reference is not null
            if (_lossMenu == null)
            {
                Debug.LogError("Loss Menu reference not set for GUI System");
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void PauseGame()
        {
            // Save current time scale, then pause game
            _unpausedTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = _unpausedTimeScale;
        }
    }
}
