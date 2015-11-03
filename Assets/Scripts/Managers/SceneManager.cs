using UnityEngine;
using System.Collections.Generic;

namespace Sheeplosion
{
    [DisallowMultipleComponent]
    public class SceneManager : MonoBehaviour
    {
        List<Explodable> _sheep;
        List<Explodable> _crates;
        List<Explodable> _nukeSheep;
        List<Explodable> _generators;

        List<Explodable> _explodables;

        public int sheepCount
        {
            get
            {
                return _sheep.Count;
            }
        }

        public int crateCount
        {
            get
            {
                return _crates.Count;
            }
        }

        public int nukeSheepCount
        {
            get
            {
                return _nukeSheep.Count;
            }
        }

        public int generatorsCount
        {
            get
            {
                return _generators.Count;
            }
        }

        /// <summary>
        /// Returns a snapshot of all explodables 
        /// active within the scene at time of getter being called
        /// </summary>
        public Explodable[] allExplodables
        {
            get
            {
                List<Explodable> explodables = new List<Explodable>(_explodables.Count);
                for (int i = 0; i < _explodables.Count; ++i)
                {
                    if (_explodables[i].gameObject.activeInHierarchy)
                    {
                        explodables.Add(_explodables[i]);
                    }
                }

                return explodables.ToArray();
            }
        }

        public void Awake()
        {
            // Find all explodables
            Explodable[] explodables = FindObjectsOfType<Explodable>();

            // Create lists
            _explodables    = new List<Explodable>(explodables.Length);
            _sheep          = new List<Explodable>();
            _crates         = new List<Explodable>();
            _nukeSheep      = new List<Explodable>();
            _generators     = new List<Explodable>();

            // Store explodables in relevant lists
            for (int i = 0; i < explodables.Length; ++i)
            {
                _explodables.Add(explodables[i]);

                switch (explodables[i].type)
                {
                    case ExplodableType.Sheep:
                        _sheep.Add(explodables[i]);
                        break;

                    case ExplodableType.Crate:
                        _crates.Add(explodables[i]);
                        break;

                    case ExplodableType.NuclearSheep:
                        _nukeSheep.Add(explodables[i]);
                        break;

                    case ExplodableType.Generator:
                        _generators.Add(explodables[i]);
                        break;
                }
            }
        }

        void Start()
        {

        }
        
        void Update()
        {
            
        }
    }
}
