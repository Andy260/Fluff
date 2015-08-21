using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Explosion : MonoBehaviour
    {
        ParticleSystem _particleSystem;

        List<GameObject> _effectedObjects = new List<GameObject>();

        float _range;
        public float range
        {
            set
            {
                _range = value;
            }
        }

        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (_particleSystem.particleCount == 0)
            {
                DestroySelf();
            }
        }

        void DestroySelf()
        {
            CalculateAffectedSheep();

            // Explode all sheep within range
            for (int i = 0; i < _effectedObjects.Count; ++i)
            {
                Sheep sheep = _effectedObjects[i].GetComponent<Sheep>();

                sheep.Explode();
            }

            // Destroy this explosion
            Destroy(this.gameObject);
        }

        void CalculateAffectedSheep()
        {
            // Find all sheep within the range of this sheep
            GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
            for (int i = 0; i < sheep.Length; ++i)
            {
                Vector3 distanceToSheep = sheep[i].transform.position - transform.position;

                if (distanceToSheep.magnitude <= _range)
                {
                    _effectedObjects.Add(sheep[i]);
                }
            }
        }
    }
}
