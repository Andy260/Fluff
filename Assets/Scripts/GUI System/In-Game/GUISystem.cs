using UnityEngine;
using Sheeplosion.Events;

namespace Sheeplosion.GUI
{
    public class GUISystem : MonoBehaviour, ISceneManagerEvents
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

        // Instantiated prefabs
        GameObject _rangeDisplay;

        // Reference to scene manager within scene
        SceneManager _sceneManager;

        // Used to preserve any time-scale effects when unpausing
        float _unpausedTimeScale = 0.0f;

        // Cached GameObject properties
        Transform _transform;
        
        public void Awake()
        {
            // Cache GameObject properties
            _transform = transform;

            // Ensure range display prefab is not null
            if (_rangeDisplayPrefab == null)
            {
                Debug.LogError("Range Display prefab is not set for GUI System");
            }
            else
            {
                // Instantiate Range Display
                _rangeDisplay       = Instantiate(_rangeDisplayPrefab) as GameObject;
                _rangeDisplay.name  = "Range Display";

                // Disable it, and parent it to this object
                _rangeDisplay.SetActive(false);
                _rangeDisplay.transform.SetParent(_transform);
            }

            // Ensure win menu reference is not null
            if (_winMenu == null)
            {
                Debug.LogError("Win Menu reference not set for GUI System");
            }
            else
            {
                _winMenu.SetActive(false);
            }

            // Ensure loss menu reference is not null
            if (_lossMenu == null)
            {
                Debug.LogError("Loss Menu reference not set for GUI System");
            }
            else
            {
                _lossMenu.SetActive(false);
            }

            // Ensure pause menu reference is not null
            if (_pauseMenu == null)
            {
                Debug.LogError("Pause Menu reference not set for GUI System");
            }
            else
            {
                _pauseMenu.SetActive(false);
            }

            // Get reference to scene manager
            _sceneManager = FindObjectOfType<SceneManager>();
            if (_sceneManager == null)
            {
                Debug.LogError(string.Format("'{0}' is unable to find Scene Manager within the scene", this.name));
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

        /// <summary>
        /// Called when player has either ran out of explosion triggers
        /// or detonated a nuclear sheep
        /// </summary>
        public void OnPlayerFailed(PlayerLoseState a_state)
        {
            _lossMenu.SetActive(true);
        }
        
        /// <summary>
        /// Called when player has either destroyed all generators in the level
        /// or destroyed all sheep
        /// </summary>
        public void OnPlayerWon(PlayerWinState a_state)
        {
            _winMenu.SetActive(true);
        }

        public void ShowDeveloperConsole()
        {
            Debug.developerConsoleVisible = true;
        }
    }
}
