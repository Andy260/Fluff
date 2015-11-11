using UnityEngine;
using UnityEngine.UI;

namespace Sheeplosion.GUI.Dialogue
{
    public class Dialogue : MonoBehaviour
    {
        // Reference to Text component
        Text _text;
        
        // Reference to Dialogue System within the scene
        DialogueSystem _dialogueSystem;

        // Full dialogue message
        string _dialogue;

        // Position within the text string, for animating
        int _textPosition;

        public bool fullMessageShown
        {
            get
            {
                return _textPosition >= _dialogue.Length;
            }
        }

        public void Awake()
        {
            // Get reference to Text component
            _text = GetComponent<Text>();
            if (_text == null)
            {
                Debug.LogError(string.Format("({0}) Unable to find Text component on this object", this.name));
            }

            // Get reference to Dialogue System
            _dialogueSystem = FindObjectOfType<DialogueSystem>();
            if (_dialogueSystem == null)
            {
                Debug.LogError(string.Format("({0}) Unable to find Dialogue System within scene", this.name));
            }
        }

        void Start()
        {
            _dialogue = _text.text;

            AnimateText();
        }

        void Update()
        {
            
        }

        void AnimateText()
        {
            Debug.Log("Text animating...");

            // Show text
            _text.text = _dialogue.Substring(0, _textPosition);
            
            if (_textPosition < _dialogue.Length)
            {
                _textPosition++;
            }

            Invoke("AnimateText", _dialogueSystem.textSpeed);
        }

        public void ShowFullMessage()
        {
            _textPosition = _dialogue.Length;

            CancelInvoke();
            AnimateText();
        }
    }
}
