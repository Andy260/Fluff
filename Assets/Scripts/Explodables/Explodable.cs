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
        [SerializeField]
        GameObject _craterPrefab;
        [SerializeField]
        GameObject _hiddenSheepPrefab;

        [Header("References")]
        [SerializeField]
        GameObject _modelReference;

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
        
        // Instantiated prefabs
        GameObject _explosionObject;
        GameObject _craterObject;
        GameObject _hiddenSheepObject;
        
        // Scene manager reference within scene
        SceneManager _sceneManager;

        // Cached GameObject properties
        Transform _transform;
        GameObject _gameObject;

        public ExplodableType type
        {
            get
            {
                return _type;
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
                _explosionObject = InstantiatePrefab(_explosionPrefab);
            }

            // Ensure crater prefab is not null
            if (_craterPrefab == null &&
                _type != ExplodableType.Crate)
            {
                Debug.LogWarning("Crater reference not set for: " + this.name);
            }
            else if (_craterPrefab != null)
            {
                _craterObject = InstantiatePrefab(_craterPrefab);
            }

            // Ensure model reference is not null
            if (_modelReference == null)
            {
                Debug.LogWarning("Model reference not set for: " + this.name);
            }

            // Ensure hidden sheep prefab is not null
            // if this explodable object is a crate
            if (_hiddenSheepPrefab == null &&
                _type == ExplodableType.Crate)
            {
                Debug.LogWarning("Hidden Sheep reference not set for: " + this.name);
            }
            else if (_type == ExplodableType.Crate)
            {
                _hiddenSheepObject = InstantiatePrefab(_hiddenSheepPrefab);
            }

            // Calculate particle system lifetime
            _particleSystemLifeTime = CalculateParticleSystemLifeTime();

            // Save reference to scene manager
            _sceneManager = FindObjectOfType<SceneManager>();
            if (_sceneManager == null)
            {
                Debug.LogWarning("Unable to find scene manager within scene");
            }
        }

        GameObject InstantiatePrefab(GameObject a_prefab)
        {
            // Create prefab
            GameObject instantiatedObject = Instantiate(_explosionPrefab, _transform.position,
                _transform.rotation) as GameObject;

            // Hide object
            instantiatedObject.SetActive(false);

            // Set object's parent
            if (a_prefab == _explosionPrefab)
            {
                // Find Particle Effects folder object
                GameObject particleEffectsFolder = GameObject.Find("Particle Effects");

                if (particleEffectsFolder == null)
                {
                    // Create object if not found
                    particleEffectsFolder       = new GameObject();
                    particleEffectsFolder.name  = "Particle Effects";

                    Debug.LogWarning("Unable to find 'Particle Effects' folder object, creating...");
                }

                // Find explosions folder object
                Transform explosionsFolder = particleEffectsFolder.transform.FindChild("Explosions");

                if (explosionsFolder == null)
                {
                    // Create object if not found
                    explosionsFolder        = new GameObject().transform;
                    explosionsFolder.name   = "Explosions";
                    explosionsFolder.transform.SetParent(particleEffectsFolder.transform);

                    Debug.LogWarning("Unable to find 'Explosions' folder object, creating...");
                }

                // Parent and name instantiated object
                instantiatedObject.transform.SetParent(explosionsFolder.transform);
                instantiatedObject.name = string.Format("Explosion ({0})", this.name);
            }
            else if (a_prefab == _craterPrefab)
            {
                // Find Environment folder object
                GameObject environmentFolder = GameObject.Find("Environment");

                if (environmentFolder == null)
                {
                    // Create object if not found
                    environmentFolder       = new GameObject();
                    environmentFolder.name  = "Environment";

                    Debug.LogWarning("Unable to find 'Environment' folder object, creating...");
                }

                // Find craters folder object
                Transform craterFolder = environmentFolder.transform.FindChild("Craters");

                if (craterFolder == null)
                {
                    // Create object if not found
                    craterFolder        = new GameObject().transform;
                    craterFolder.name   = "Craters";
                    craterFolder.transform.SetParent(environmentFolder.transform);

                    Debug.LogWarning("Unable to find 'Craters' folder object, creating...");
                }

                // Parent and name instantiated object
                instantiatedObject.transform.SetParent(craterFolder.transform);
                instantiatedObject.name = string.Format("Crater ({0})", this.name);
            }
            else if (a_prefab == _hiddenSheepObject)
            {
                // Find Explodables folder object
                GameObject explodablesFolder = GameObject.Find("Explodables");

                if (explodablesFolder == null)
                {
                    // Create object if not found
                    explodablesFolder       = new GameObject();
                    explodablesFolder.name  = "Explodables";

                    Debug.LogWarning("Unable to find 'Explodables' folder object, creating...");
                }

                // Find sheep folder object
                Transform sheepFolder = explodablesFolder.transform.FindChild("Sheep");

                if (sheepFolder == null)
                {
                    // Create object if not found
                    sheepFolder         = new GameObject().transform;
                    sheepFolder.name    = "Sheep";
                    sheepFolder.transform.SetParent(explodablesFolder.transform);

                    Debug.LogWarning("Unable to find 'Sheep' folder object, creating...");
                }

                // Parent and name instantiated object
                instantiatedObject.transform.SetParent(sheepFolder.transform);
                instantiatedObject.name = string.Format(a_prefab.name);
            }

            return instantiatedObject;
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
            if (_type == ExplodableType.Sheep)
            {
                // Render chain reaction range gizmo
                Gizmos.DrawWireSphere(transform.position, _chainReactionRange);
            }
            else if (_type == ExplodableType.Crate)
            {
                // Render chain reaction range gizmo for hidden sheep
                if (_hiddenSheepPrefab != null)
                {
                    float range = _hiddenSheepPrefab.GetComponent<Explodable>()._chainReactionRange;
                    Gizmos.DrawWireSphere(transform.position, range);
                }
            }
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

            // Ensure this gameobject is active
            if (!_gameObject.activeInHierarchy)
            {
                return false;
            }

            // Display particle effect
            _explosionObject.SetActive(true);

            // Hide this object's model
            _modelReference.SetActive(false);

            // Display crater if available
            if (_craterPrefab != null)
            {
                _craterPrefab.SetActive(true);
            }

            // Disable particle effect at end of particle effect
            Invoke("DisableExplosionEffect", _particleSystemLifeTime);

            float chainReactionTime = _particleSystemLifeTime;
            if (_explosionTime > 0)
            {
                chainReactionTime = _explosionTime;
            }

            // Handle type specific functionality
            switch (_type)
            {
                case ExplodableType.Sheep:
                    Invoke("SheepExplosion", chainReactionTime);
                    break;

                case ExplodableType.Crate:
                    Invoke("CrateExplosion", chainReactionTime);
                    break;

                case ExplodableType.NuclearSheep:
                    Invoke("NuclearSheepExplosion", chainReactionTime);
                    break;

                case ExplodableType.Generator:
                    Invoke("GeneratorExplosion", chainReactionTime);
                    break;
            }
            
            return true;
        }

        /// <summary>
        /// Disables the explosion particle effect through an Invoke method call
        /// </summary>
        void DisableExplosionEffect()
        {
            _explosionObject.SetActive(false);

            Debug.Log("Disabling explosion particle effect for: " + this.name);
        }

        void SheepExplosion()
        {
            // TODO: Alert Scene manager of sheep being destroyed

            TriggerChainReactions();

            _gameObject.SetActive(false);
        }

        void CrateExplosion()
        {
            if (_hiddenSheepObject != null)
            {
                // Show hidden sheep
                _hiddenSheepObject.SetActive(true);

                // Unparent hidden sheep, so it remains active
                // when this object gets deactivated
                _hiddenSheepObject.transform.SetParent(null);
            }

            _gameObject.SetActive(false);
        }

        void NuclearSheepExplosion()
        {
            // TODO: Play nuclear sheep loss effect
            // explode all explodables, and send failure message

            Debug.Log("Player triggered Nuclear explosion");

            _gameObject.SetActive(false);
        }

        void GeneratorExplosion()
        {
            // TODO: Send PlayerWin message

            Debug.Log("Player exploded generator");

            _gameObject.SetActive(false);
        }

        void TriggerChainReactions()
        {
            Explodable[] explodables = _sceneManager.allExplodables;

            // Get all objects within range of chain reaction of this object
            foreach (Explodable explodable in explodables)
            {
                // Ignore this object
                if (explodable == this)
                {
                    continue;
                }

                // Get distance from this object to object in question
                Transform transform = explodable.transform;
                float distanceSqr   = (_transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < Mathf.Pow(_chainReactionRange, 2.0f))
                {
                    // Attempt to explode object within range
                    bool chainReactionStatus = explodable.Explode("Explosion");
                    if (!chainReactionStatus)
                    {
                        // Unable to explode object through chain reaction, log it
                        Debug.LogWarning(string.Format("{0} attempted to trigger chain reaction on {1}, but failed",
                            this.name, explodable.name));
                    }
                    else
                    {
                        Debug.Log(string.Format("{0} triggered chain reaction on {1}", this.name, explodable.name));
                    }
                }
            }
        }
    }
}
