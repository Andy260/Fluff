using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Explosion : MonoBehaviour
    {
        ParticleSystem _particleSystem;                                 // Particle system of explosion

        List<GameObject> _effectedObjects = new List<GameObject>();     // Which objects to explode once this explosion finishes

        bool _triggeredByNuke = false;
        public bool triggerByNuke
        {
            set
            {
                _triggeredByNuke = value;
            }
        }

        Sheep _sheepToShow;                                             // Which sheep to show once explosion has finished
        /// <summary>
        /// Which sheep show the explosion set as
        /// active once finished
        /// </summary>
        public Sheep sheepToShow
        {
            set
            {
                _sheepToShow = value;
            }
        }

        float _range;                                                   // Range of which objects to explode once explosion finishes

        public int _totalExplosivesInScene = 0;

        public LayerMask _effectLayerMask;

        GUISystem _guiSystem;

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
            
            _guiSystem = GameObject.Find("GUI System").GetComponent<GUISystem>();
        }

        void Update()
        {
            if (_particleSystem.particleCount == 0)
            {
                DestroySelf();
            }
        }

        public void OnDestroy()
        {
            _guiSystem.explosionCount -= 1;
        }

        void DestroySelf()
        {
            CalculateAffectedObjects();

            // Explode all sheep within range
            for (int i = 0; i < _effectedObjects.Count; ++i)
            {
                GameObject gameObject = _effectedObjects[i];

                if (gameObject.tag == "Sheep" && gameObject.activeInHierarchy)
                {
                    Sheep sheep = gameObject.GetComponent<Sheep>();
                    sheep.Explode();
                }
                else
                {
                    Crate crate = gameObject.GetComponent<Crate>();
                    crate.Explode();
                }
            }

            // Show hidden sheep if any
            if (_sheepToShow != null)
            {
                _sheepToShow.gameObject.SetActive(true);
            }

            // Destroy this explosion
            Destroy(this.gameObject);
        }

        void CalculateAffectedObjects()
        {
            // Find all sheep within the range of this sheep
            GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
            for (int i = 0; i < sheep.Length; ++i)
            {
                Vector3 distanceToSheep = sheep[i].transform.position - transform.position;

                // Check if object is within range
                if (distanceToSheep.magnitude <= _range)
                {
                    _effectedObjects.Add(sheep[i]);

                    //RaycastHit raycastHit;
                    //
                    //// Check for object in the way of this object
                    //if (Physics.Raycast(transform.position, distanceToSheep.normalized, out raycastHit, 
                    //        Mathf.Infinity, _effectLayerMask))
                    //{
                    //    _effectedObjects.Add(sheep[i]);
                    //}
                }
            }

            // Find all crates within the range of this sheep
            GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
            for (int i = 0; i < crates.Length; ++i)
            {
                Vector3 distanceToCrate = crates[i].transform.position - transform.position;

                // Check if object is within range
                if (distanceToCrate.magnitude <= _range)
                {
                    _effectedObjects.Add(crates[i]);

                    //RaycastHit raycastHit;
                    //
                    //// Check for object in the way of this object
                    //if (Physics.Raycast(transform.position, distanceToCrate.normalized, out raycastHit,
                    //        Mathf.Infinity, _effectLayerMask))
                    //{
                    //    _effectedObjects.Add(crates[i]);
                    //}
                }
            }
        }
    }
}
