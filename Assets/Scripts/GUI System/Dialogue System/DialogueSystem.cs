using UnityEngine;

namespace Sheeplosion.GUI.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        [Header("References")]
        [Tooltip(@"Children of this object will be treated as dialogue. 
                    They will appear using the order within the hierachy")]
        [SerializeField]
        GameObject _dialogueList;

        [Header("Configuration")]
        [SerializeField]
        float _textSpeed = 0.025f;

        // Dialogue messages to display
        Dialogue[] _dialogueArray;

        // Reference to player within the scene
        Player _player;
        // Reference to player controller within the scene
        PlayerCameraController _playerCameraController;

        // Current dialogue message, within the array
        int _currentMessageItr;

        // Cached GameObject properties
        GameObject _gameObject;

        public float textSpeed
        {
            get
            {
                return _textSpeed;
            }
        }

        public void Awake()
        {
            // Cache GameObject properties
            _gameObject = gameObject;

            // Find Player within the scene
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogWarning(string.Format("({0}) Unable to find any Player within the scene", this.name));
            }

            // Find Player Camera Controller within the scene
            _playerCameraController = FindObjectOfType<PlayerCameraController>();
            if (_playerCameraController == null)
            {
                Debug.LogWarning(string.Format("({0}) Unable to find any Player Camera Controller within the scene", this.name));
            }

            // Initialise dialogue list array
            if (_dialogueList == null ||
                _dialogueList.transform.childCount < 1)
            {
                Debug.LogError(string.Format(@"({0}) Dialogue list not set correctly. 
                    Please ensure there is at least 1 child of the dialogue object",
                    this.name));
            }
            else
            {
                // Get array of Dialogue
                _dialogueArray = _dialogueList.GetComponentsInChildren<Dialogue>();
                if (_dialogueArray == null ||
                    _dialogueArray.Length <= 0)
                {
                    Debug.LogWarning(string.Format("({0}) Unable to find any dialogue messages", this.name));
                }

                // Disable all dialogue messages, except the first message
                for (int i = 1; i < _dialogueArray.Length; ++i)
                {
                    _dialogueArray[i].gameObject.SetActive(false);
                }
            }
        }

        void Update()
        {

        }

        public void OnDisable()
        {
            // Enable player when all messages have been shown
            _player.enabled                 = true;
            _playerCameraController.enabled = true;
        }

        public void OnEnable()
        {
            // Disable player, until messages have all shown
            _player.enabled                 = false;
            _playerCameraController.enabled = false;
        }

        public void ShowNextMessage()
        {
            // Ignore if this component is disabled
            if (!this.enabled)
            {
                return;
            }

            Debug.Log("Showing next message");

            // Show full message of current message, if still being animated
            if (!_dialogueArray[_currentMessageItr].fullMessageShown)
            {
                _dialogueArray[_currentMessageItr].ShowFullMessage();
                return;
            }

            // Hide current message
            _dialogueArray[_currentMessageItr].gameObject.SetActive(false);

            _currentMessageItr++;

            if (_currentMessageItr >= _dialogueArray.Length)
            {
                // End of messages, hide Dialogue system
                _gameObject.SetActive(false);
            }
            else
            {
                // Show next message
                _dialogueArray[_currentMessageItr].gameObject.SetActive(true);
            }
        }
    }
}
