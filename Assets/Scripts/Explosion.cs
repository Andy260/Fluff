using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Explosion : MonoBehaviour
    {
        public enum Type
        {
            NORMAL,
            GENERATOR,
            NUKE,
        }
        [Tooltip("Which particle system to use when exploding")]
        public Type _type;

        List<GameObject> _effectedObjects = new List<GameObject>();
        List<ParticleSystem> _particleSystems;

        public GameObject _nukeParticleEffect;
        public GameObject _normalParticleEffect;
        public GameObject _generatorParticleEffect;

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
        Sheep _sheepToShow;

        public void Awake()
        {
            // TODO: Create particle systems and disable this script
            CreateParticleSystems();
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        void CreateParticleSystems()
        {
            GameObject prefab;

            switch (_type)
            {
                case Type.GENERATOR:
                    prefab = _generatorParticleEffect;
                    break;

                case Type.NUKE:
                    prefab = _nukeParticleEffect;
                    break;

                default:
                    prefab = _normalParticleEffect;
                    break;
            }

            // Create particle system prefab
            GameObject particleSystem =
                Instantiate(prefab, transform.position, transform.rotation) as GameObject;

            // Cache particle system references
            particleSystem.transform.SetParent(this.transform);
            ParticleSystem[] particleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>();
            _particleSystems = new List<ParticleSystem>(particleSystems.Length);

            for (int i = 0; i < particleSystems.Length; ++i)
            {
                _particleSystems.Add(particleSystems[i]);
            }
        }
    }
}
