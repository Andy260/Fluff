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

        Camera _camera;
        Transform _camTransform;

        public void Awake()
        {
            _transform = transform;

            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
            {
                Debug.Log(string.Format("({0}) Unable to find Child camera", this.name));
            }
        }

        public void FixedUpdate()
        {
            // Translate camera
            _transform.Translate(_velocity * Time.fixedDeltaTime);

            // Bleed velocity
            _velocity -= (_velocity / _mass) * Time.fixedDeltaTime;
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

            // Set velocity to desired translation
            _velocity = a_translation * _speed;
        }

        public void ZoomCamera(float a_amount)
        {
            if (_camera.orthographic)
            {
                _camera.orthographicSize += a_amount * _zoomSpeed;
                _camera.orthographicSize = Mathf.Max(_camera.orthographicSize, 0.1f);
            }
            else
            {
                _camera.fieldOfView += a_amount * _zoomSpeed;
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, 0.1f, 179.9f);
            }
        }
    }
}
