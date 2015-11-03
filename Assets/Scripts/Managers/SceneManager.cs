using UnityEngine;
using System.Collections.Generic;

namespace Sheeplosion
{
    [DisallowMultipleComponent]
    public class SceneManager : MonoBehaviour
    {
        List<GameObject> _sheep;
        List<GameObject> _crates;
        List<GameObject> _nukeSheep;
        List<GameObject> _generators;

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

        public void Awake()
        {

        }

        void Start()
        {

        }
        
        void Update()
        {

        }

        public void DecreaseCount(ExplodableType a_type, GameObject a_gameObject)
        {

        }
    }
}
