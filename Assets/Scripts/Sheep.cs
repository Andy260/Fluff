using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Sheep : MonoBehaviour, IExplodable
    {
        [Tooltip("Explosion prefab")]
        public GameObject _explosion;

        float _range;
        public float range
        {
            get
            {
                return _range;
            }
        }

        Player _player;

        void Start()
        {
            // Increment count of sheep within level
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.sheepCount += 1;

            // Get range
            SphereCollider collider = GetComponent<SphereCollider>();
            _range = collider.radius;
            Destroy(collider);
        }

        public void OnDestroy()
        {
            _player.sheepCount -= 1;
        }

        void Update()
        {

        }

        public void Explode()
        {
            // Create explosion object
            GameObject explosionObject = Instantiate(_explosion, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f))) as GameObject;

            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.range = _range;

            // Destroy this object
            Destroy(this.gameObject);
        }
    }
}
