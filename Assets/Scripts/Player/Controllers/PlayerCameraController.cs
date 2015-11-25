using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Speed at which the player is able to move the camere")]
        [SerializeField]
        float _speed;

        [Tooltip("Controls the max bleed speed once a touch has ended")]
        [SerializeField]
        float _maxSpeed;

        [SerializeField]
        float _zoomSpeed;

        [Tooltip("Higher mass will increase the amount of time until the camera stops")]
        [SerializeField]
        float _mass;

        [SerializeField]
        float _minZoomHeight;
        [SerializeField]
        float _maxZoomHeight;

        // Current velocity
        Vector3 _velocity;

        // Cached GameObject properties
        [HideInInspector]
        Transform _transform;

        // Child camera
        [HideInInspector]
        Camera _camera;
        [HideInInspector]
        Transform _camTransform;

        public void Awake()
        {
            // Cache game object properties
            _transform = transform;

            // Get reference to camera child component
            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
            {
                Debug.Log(string.Format("({0}) Unable to find Child camera", this.name));
            }

            // Cache child camera transform property
            _camTransform = _camera.transform;

            // Ensure zoom settings are valid
            if (_minZoomHeight < 0.1f)
            {
                Debug.LogWarning(string.Format("({0}) Invalid Minimum zoom height, setting to 0.1", this.name));
                _minZoomHeight = 1.0f;
            }
        }

        void Update()
        {
            // Translate camera
            _transform.Translate(_velocity * Time.deltaTime);

            // Bleed velocity
            _velocity -= (_velocity / _mass) * Time.deltaTime;
        }

        public void TranslateCamera(Vector3 a_translation)
        {
            if (!enabled)
            {
                // Ignore function calls, if this script
                // is disabled
                return;
            }

            // Ensure no translation on the Y axis happens
            // for lateral movement
            if (a_translation.y > 0)
            {
                a_translation.y = 0.0f;
            }

            _transform.Translate(a_translation * _speed);

            // Set velocity to desired translation
            _velocity = a_translation.normalized * _maxSpeed;
        }

        public void ZoomCamera(float a_amount)
        {
            if (_camera.orthographic)
            {
                // Zoom camera
                _camera.orthographicSize += a_amount * _zoomSpeed;

                // Ensure camera zoom is within limits
                _camera.orthographicSize = Mathf.Max(_camera.orthographicSize, _minZoomHeight);
                _camera.orthographicSize = Mathf.Min(_camera.orthographicSize, _maxZoomHeight);
            }
            else
            {
                // Zoom camera
                _camTransform.Translate(Vector3.forward * (a_amount * _zoomSpeed));

                // Ensure camera zoom is within limits
                _camTransform.localPosition = Vector3.Max(_camTransform.localPosition, Vector3.up * _minZoomHeight);
                _camTransform.localPosition = Vector3.Min(_camTransform.localPosition, Vector3.up * _maxZoomHeight);
            }
        }
    }
}
