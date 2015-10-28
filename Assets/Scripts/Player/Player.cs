using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    public class Player : MonoBehaviour
    {
        [Header("Explosion Triggering")]
        [SerializeField]
        uint _explosionsAmount = 2;
        [SerializeField]
        LayerMask _explodableLayers;

        // Explosions
        uint _currentExplosionsCount = 0;

        // Cached scene objects
        Camera _playerCamera;

        // Cached GameObject properties
        Transform _transform;

        public void Awake()
        {
            _transform = transform;

            // Find player camera controller
            PlayerCameraController cameraController = FindObjectOfType<PlayerCameraController>();
            if (cameraController == null)
            {
                Debug.LogWarning("Either no player camera controller present within the scene, or Player script is unable to find it");
            }
            else
            {
                // Get player camera
                _playerCamera = cameraController.GetComponentInChildren<Camera>();
                if (_playerCamera == null)
                {
                    Debug.LogWarning("Player script is unable to find the player camera");
                }
            }
        }

        /// <summary>
        /// Will first raycast for any objects which the player is able to explode
        /// then attempt to trigger them to explode
        /// </summary>
        public void TriggerExplosion(Vector3 a_inputPosition)
        {
            // Ensure player has enough explosions left
            // to trigger another
            if (_currentExplosionsCount > _explosionsAmount)
            {
                Debug.Log("Player is out of explosions");
                return;
            }

            Debug.Log("Player is attemtping to explode an object");

            RaycastHit raycastHit;

            if (Physics.Raycast(_playerCamera.ScreenPointToRay(a_inputPosition), 
                    out raycastHit, _explodableLayers))
            {
                Explodable explodableObject = raycastHit.transform.GetComponent<Explodable>();

                // Ensure object has an explodable script attached
                if (explodableObject == null)
                {
                    Debug.LogWarning(string.Format("Attempted to explode object ({0}) but found no Explodable script attached", 
                        raycastHit.collider.name));

                    return;
                }

                // Attmept to explode object, and increment
                // explosives count, if exploded
                bool explodedObject = explodableObject.Explode(this.tag);
                if (explodedObject)
                {
                    _currentExplosionsCount++;
                }
            }
        }
    }
}
