using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    public class TouchInput : MonoBehaviour
    {
#if UNITY_EDITOR
        // Used to calculate mouse delta position
        Vector3 _lastMousePos;

        Vector3 mouseDelta
        {
            get
            {
                return Input.mousePosition - _lastMousePos;
            }
        }
#endif

        // Scene script references
        PlayerCameraController _cameraController;
        Camera _mainCamera;
        Player _player;

        public void Awake()
        {
            // Get reference to player camera controller
            _cameraController = FindObjectOfType<PlayerCameraController>();
            if (_cameraController == null)
            {
                Debug.LogError("Unable to find player camera controller within scene");
            }

            _mainCamera = _cameraController.GetComponentInChildren<Camera>();


            // Get reference to player
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogError("TouchInput script unable to find player script within scene");
            }
        }

        void Update()
        {
            HandleTranslation();
            HandleExplodeTriggers();
        }

        void HandleTranslation()
        {
            Vector3 translation = Vector3.zero;

#if UNITY_EDITOR
            // Calculate mouse delta
            Vector3 currentMouseDelta = mouseDelta;

            if (Input.GetMouseButton(0))
            {
                // Calculate mouse delta, and apply translation
                translation = new Vector3(mouseDelta.x, 0.0f, mouseDelta.y);
                _cameraController.TranslateCamera(-translation);
            }

            // Update last mouse position for next frame
            _lastMousePos = Input.mousePosition;

            return;
#else
            if (Input.touchCount > 1 ||
                Input.touchCount < 1)
            {
                // Ignore accidental multi-finger gestures
                return;
            }

            // Get touch delta
            Touch touch = Input.GetTouch(0);
            Vector2 deltaTouchPos = -touch.deltaPosition;

            // Apply transaltion
            translation = new Vector3(deltaTouchPos.x, 0.0f, deltaTouchPos.y);
            _cameraController.TranslateCamera(-translation);
#endif
        }

        void HandleExplodeTriggers()
        {
#if UNITY_EDITOR
            // TODO: Fix bug where simulated touch gesture
            // triggers when drag gesture occurs

            if (Input.GetMouseButtonUp(0) &&
                mouseDelta == Vector3.zero)
            {
                _player.TriggerExplosion(Input.mousePosition);
            }
#else
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.tapCount == 1)
                {
                    _player.TriggerExplosion(touch.position);
                }
            }
#endif
        }
    }
}
