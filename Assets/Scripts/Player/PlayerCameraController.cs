using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        [Tooltip("Speed at which the player is able to move the camere")]
        float _speed;

        // Reference to main camera
        Camera _camera;

        // Cached GameObject properties
        Transform _transform;

        public void Awake()
        {
            _transform = transform;

            // Get reference to the camera
            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
            {
                Debug.LogError("Can't find camera in children of touch camera controller");
            }
        }

        public void TranslateCamera(Vector3 a_translation)
        {
            // Ensure no translation on the Y axis happens
            // for lateral movement
            if (a_translation.y > 0)
            {
                a_translation.y = 0.0f;
            }

            _transform.Translate(a_translation * (_speed * Time.deltaTime));
        }
    }
}
