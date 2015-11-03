using UnityEngine;

namespace Sheeplosion
{
    public enum ExplodableType
    {
        Sheep,
        Crate,
        NuclearSheep,
        Generator
    }

    [DisallowMultipleComponent]
    public class Explodable : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        GameObject _explosionPrefab;

        [Header("References")]
        [SerializeField]
        GameObject _modelReference;
        [SerializeField]
        GameObject _craterReference;
        [SerializeField]
        GameObject _hiddenSheep;

        [Header("Configuration")]
        [Tooltip("Sheep: Chains explosions (generic type) \nCrate: Disables parented sheep upon scene start, then enables it once exploded \nNuclear Sheep: Fails the player with nuclear sheep endgame effect \nGenerator: Player succeeds")]
        [SerializeField]
        ExplodableType _type;

        [Tooltip("Whether or not the player is able to directly trigger this object to explode")]
        [SerializeField]
        bool _explodableByPlayer = true;

        [Tooltip("The range at which this object will trigger other objects to explode")]
        [SerializeField]
        float _chainReactionRange = 10.0f;

        [Tooltip("Time at which chain reactions will trigger. (A value of 0 will wait until particle effect has finished before triggering chains, and sending events)")]
        [SerializeField]
        float _explosionTime = 0.0f;

        // Precalulated particle System lifetime
        float _particleSystemLifeTime = 0.0f;
        
        // Explosion Object
        GameObject _explosionObject;

        // Trigger used to determine which objects
        // can be exploded within a chain reaction
        SphereCollider _sphereTrigger;

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
            // Cache game object attributes
            _transform  = transform;
            _gameObject = gameObject;

            // Ensure explosion particle effect is not null
            if (_explosionPrefab == null)
            {
                Debug.LogError("Explosion prefab not set for: " + this.name);
            }
            else
            {
                // Create explosion particle effect object
                GameObject explosionObject = Instantiate(_explosionPrefab, _transform.position,
                    _transform.rotation) as GameObject;

                // Set this object as explosion object's parent
                explosionObject.transform.SetParent(_transform);
                // Hide object
                explosionObject.SetActive(false);
                // Correctly name object
                explosionObject.name = "Explosion (Particle Effect)";

                // Save reference to object
                _explosionObject = explosionObject;
            }

            // Ensure crater prefab is not null
            if (_craterReference == null)
            {
                Debug.LogWarning("Crater reference not set for: " + this.name);
            }
            else
            {
                _craterReference.SetActive(false);
            }

            // Ensure model reference is not null
            if (_modelReference == null)
            {
                Debug.LogWarning("Model reference not set for: " + this.name);
            }

            // Ensure hidden sheep reference is not null
            // if this explodable object is a crate
            if (_hiddenSheep == null &&
                _type == ExplodableType.Crate)
            {
                Debug.LogWarning("Hidden Sheep reference not set for: " + this.name);
            }
            else if (_type == ExplodableType.Crate)
            {
                _hiddenSheep.SetActive(false);
            }

            // Calculate particle system lifetime
            _particleSystemLifeTime = CalculateParticleSystemLifeTime();
        }

        float CalculateParticleSystemLifeTime()
        {
            float totalLifeTime = 0.0f;

            // Check lifetime of any attached particle system components
            ParticleSystem[] particleSystems = _explosionObject.GetComponents<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; ++i)
            {
                // Replace current count with higher count, if found
                if (totalLifeTime < particleSystems[i].duration)
                {
                    totalLifeTime = particleSystems[i].duration;
                }
            }

            // Check lifetime of any child particle system components
            ParticleSystem[] childParticleSystems = _explosionObject.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < childParticleSystems.Length; ++i)
            {
                // Replace current count with higher count, if found
                if (totalLifeTime < childParticleSystems[i].duration)
                {
                    totalLifeTime = childParticleSystems[i].duration;
                }
            }

            return totalLifeTime;
        }

        void Update()
        {

        }

        public void OnDrawGizmosSelected()
        {
            // Render chain reaction range gizmo
            Gizmos.DrawWireSphere(transform.position, _chainReactionRange);
        }

        /// <summary>
        /// Begins the explosion routine. 
        /// Returns false if explosion didn't occur.
        /// </summary>
        /// <param name="a_callerTag">Used to identify which object attempted to trigger the explosion routine</param>
        /// <returns>Success of explosion</returns>
        public bool Explode(string a_callerTag)
        {
            if (!_explodableByPlayer && 
                a_callerTag == "Player")
            {
                // Ignore messages to explode from player
                // if set to do so
                return false;
            }

            // Display particle effect
            _explosionObject.SetActive(true);

            // Hide this object's model
            _modelReference.SetActive(false);

            // Disable particle effect at end of particle effect
            float particleDisableTime = _explosionTime;
            if (_explosionTime <= 0.0f)
            {
                particleDisableTime = _particleSystemLifeTime;
            }

            Invoke("ChainExplosions", particleDisableTime);

            // Handle type specific functionality
            switch (_type)
            {
                case ExplodableType.Sheep:
                    SheepExplosion();
                    break;

                case ExplodableType.Crate:
                    CrateExplosion();
                    break;

                case ExplodableType.NuclearSheep:
                    NuclearSheepExplosion();
                    break;

                case ExplodableType.Generator:
                    GeneratorExplosion();
                    break;
            }
            
            return true;
        }

        /// <summary>
        /// Disables the explosion particle effect, then attempts to chain explosions
        /// </summary>
        void ChainExplosions()
        {
            _explosionObject.SetActive(false);

            // TODO: Chain explosions
        }

        void SheepExplosion()
        {
            // TODO: Alert Scene manager of sheep being destroyed
        }

        void CrateExplosion()
        {
            // TODO: Show hidden sheep once particle effect has ended
        }

        void NuclearSheepExplosion()
        {
            // TODO: Play nuclear sheep loss effect
            // and send PlayerLoss message
        }

        void GeneratorExplosion()
        {
            // TODO: Send PlayerWin message
        }
    }
}
