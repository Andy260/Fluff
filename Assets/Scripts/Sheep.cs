using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Sheep : MonoBehaviour, IExplodable
    {
        [Tooltip("Explosion prefab")]
        public GameObject _explosion;

        [Tooltip("Should this sheep be treated as a game ending sheep?")]
        public bool _isNuclear = false;

        public float range
        {
            get
            {
                return _range;
            }
        }
        float _range;

        Player _player;

        void Start()
        {
            
        }

        void Update()
        {

        }

        public void Explode()
        {
            
        }
    }
}
