using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        [Tooltip("Speed at which the player is able to move the camere")]
        float _speed;

        // Cached GameObject properties
        Transform _transform;

        public void Awake()
        {
            _transform = transform;
        }

        void Update()
        {

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

            _transform.Translate(a_translation * (_speed * Time.deltaTime));
        }
    }
}
