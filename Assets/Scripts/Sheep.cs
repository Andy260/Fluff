using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Sheep : MonoBehaviour, IExplodable
    {
        [Tooltip("Explosion prefab")]
        public GameObject _explosion;

        public float _rangeDistance = 0.0f;

        List<GameObject> _effectedObjects;

        void Start()
        {
            // Increment count of sheep within level
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.sheepCount += 1;
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
            explosion.range = _rangeDistance;

            // Destroy this object
            Destroy(this.gameObject);
        }
    }
}
