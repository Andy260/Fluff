using UnityEngine;
using Sheeplosion.Events;
using UnityEngine.UI;

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
        [SerializeField]
        GameObject _pauseButton;

        [SerializeField]
        GameObject _explosionsCount;
        [SerializeField]
        GameObject _sheepCount;
        [SerializeField]
        GameObject _generatorCount;

        // Instantiated prefabs
        GameObject _rangeDisplay;

        // Reference to scene manager within scene
        SceneManager _sceneManager;

        // Reference to player within the scene
        Player _player;

        // Used to preserve any time-scale effects when unpausing
        float _unpausedTimeScale = 0.0f;

        // References to text within overlay
        Text _explosionsCountText;
        Text _sheepCountText;
        Text _generatorCountText;

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

            // Ensure loss menu reference is not null
            if (_lossMenu == null)
            {
                Debug.LogError("Loss Menu reference not set for GUI System");
            }

            // Ensure pause menu reference is not null
            if (_pauseMenu == null)
            {
                Debug.LogError("Pause Menu reference not set for GUI System");
            }

            // Get reference to scene manager
            _sceneManager = FindObjectOfType<SceneManager>();
            if (_sceneManager == null)
            {
                Debug.LogError(string.Format("'{0}' is unable to find Scene Manager within the scene", this.name));
            }

            // Get reference to player within scene
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogError(string.Format("'{0}' is unable to find Player within the scene", this.name));
            }

            // Get references to overlay objects
            if (_explosionsCount    == null ||
                _sheepCount         == null ||
                _generatorCount     == null)
            {
                Debug.LogError(string.Format("({0}) Overlay GUI reference not set correctly", this.name));
            }
            else
            {
                _explosionsCountText    = _explosionsCount.GetComponentInChildren<Text>();
                _sheepCountText         = _sheepCount.GetComponentInChildren<Text>();
                _generatorCountText     = _generatorCount.GetComponentInChildren<Text>();

                if (_explosionsCountText    == null ||
                    _sheepCountText         == null ||
                    _generatorCountText     == null)
                {
                    Debug.LogError(string.Format("({0}) Unable to find text components of GUI overlay. Please ensure they are parented to the references objects and aren't disabled.", this.name));
                }
            }
        }

        void Start()
        {
            // Setup menu
            _winMenu.SetActive(false);
            _lossMenu.SetActive(false);
            _pauseMenu.SetActive(false);
            _pauseButton.SetActive(true);

            ShowOverlay(true);
        }

        void Update()
        {
            UpdateOverlay();
        }

        void UpdateOverlay()
        {
            if (_sceneManager           == null ||
                _player                 == null ||
                _explosionsCountText    == null ||
                _sheepCountText         == null ||
                _generatorCountText     == null)
            {
                // Don't update if errors were present during awake
                return;
            }

            // Update overlay
            _explosionsCountText.text   = _player.explosionCount.ToString();
            _sheepCountText.text        = _sceneManager.sheepCount.ToString();
            _generatorCountText.text    = _sceneManager.generatorsCount.ToString();
        }

        void ShowOverlay(bool a_value)
        {
            _explosionsCount.SetActive(a_value);

            // Only show sheep count, if any are present within the level
            if (_sceneManager.sheepCount > 0)
            {
                _sheepCount.SetActive(a_value);
            }

            // Only show generator count, if any are present within the level
            if (_sceneManager.generatorsCount > 0)
            {
                _generatorCount.SetActive(a_value);
            }
        }

        public void PauseGame()
        {
            // Hide overlay
            ShowOverlay(false);

            // Save current time scale, then pause game
            _unpausedTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
        }

        public void ResumeGame()
        {
            ShowOverlay(true);
            Time.timeScale = _unpausedTimeScale;
        }

        /// <summary>
        /// Called when player has either ran out of explosion triggers
        /// or detonated a nuclear sheep
        /// </summary>
        public void OnPlayerFailed(PlayerLoseState a_state)
        {
            // Show loss prompt, and hide pause button
            _lossMenu.SetActive(true);
            _pauseButton.SetActive(false);
        }
        
        /// <summary>
        /// Called when player has either destroyed all generators in the level
        /// or destroyed all sheep
        /// </summary>
        public void OnPlayerWon(PlayerWinState a_state)
        {
            // Show win prompt, and hide pause button
            _winMenu.SetActive(true);
            _pauseButton.SetActive(false);
        }

        public void LoadLevel(string a_levelName)
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel(a_levelName);
        }

        public void ReloadLevel()
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel(Application.loadedLevel);
        }

        public void LoadNextLevel()
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
