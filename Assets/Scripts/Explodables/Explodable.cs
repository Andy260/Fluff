using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    public class Explodable : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        GameObject _explosionPrefab;

        [Header("Configuration")]
        [Tooltip("Whether or not the player is able to directly trigger this object to explode")]
        [SerializeField]
        bool _explodableByPlayer = true;

        [Tooltip("The range at which this object will trigger other objects to explode")]
        [SerializeField]
        float _chainReactionRange = 10.0f;

        // Explosion Object
        GameObject _explosionObject;

        // Cached GameObject properties
        Transform _transform;
        GameObject _gameObject;

        public bool exploded
        {
            get
            {
                return _gameObject.activeInHierarchy;
            }
        }

        public void Awake()
        {
            _transform  = transform;
            _gameObject = gameObject;

            if (_explosionPrefab == null)
            {
                Debug.LogError("Explosion prefab not set for: " + this.name);
            }
            else
            {
                // Create explosion object
                GameObject explosionObject = Instantiate(_explosionPrefab, _transform.position, 
                    _transform.rotation) as GameObject;

                // Set this object as explosion object's parent
                explosionObject.transform.SetParent(_transform);
                // Hide object
                explosionObject.SetActive(false);
            }
        }

        public bool Explode(string a_callerTag)
        {
            if (!_explodableByPlayer && 
                a_callerTag == "Player")
            {
                // Ignore messages to explode from player
                // if set to do so
                return false;
            }

            _explosionObject.SetActive(true);
            _gameObject.SetActive(false);

            return true;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _chainReactionRange);
        }
    }
}
