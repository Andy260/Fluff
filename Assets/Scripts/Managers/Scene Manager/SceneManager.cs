using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Sheeplosion.Events;

namespace Sheeplosion
{
    [DisallowMultipleComponent]
    public class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// Used to store the current state of the player's win. 
        /// (Used to ignore events when a state has already been determined)
        /// </summary>
        enum WinState
        {
            Win,
            Loss,
            None
        }

        // Array of all event listeners
        [Header("Event System")]
        [Tooltip("List of objects will be notified of Scene Manager events, if able to receive such events")]
        [SerializeField]
        List<GameObject> _eventListeners = new List<GameObject>();

        // Reference to player within scene
        Player _player;

        // List of all sheep within the scene
        List<Explodable> _sheep;
        // List of all crates within the scene
        List<Explodable> _crates;
        // List of all nuclear sheep within the scene
        List<Explodable> _nukeSheep;
        // List of all generators within the scene
        List<Explodable> _generators;

        // List of all explodables within the scene
        List<Explodable> _explodables;

        // Defines the current player win state
        WinState _winState = WinState.None;

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
            // Initialise lists
            _sheep          = new List<Explodable>();
            _crates         = new List<Explodable>();
            _nukeSheep      = new List<Explodable>();
            _generators     = new List<Explodable>();
            _explodables    = new List<Explodable>();

            // Get reference to player
            _player = FindObjectOfType<Player>();
            if (_player == null)
            {
                Debug.LogError("(Scene Manager) Unable to find player within scene");
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            // Ensure all sheep are referenced
            Explodable[] explodables = FindObjectsOfType<Explodable>();
            foreach (Explodable explodable in explodables)
            {
                List<Explodable> referenceList = GetList(explodable.type);

                if (!referenceList.Contains(explodable) ||
                    !_explodables.Contains(explodable))
                {
                    if (explodable.gameObject.activeInHierarchy)
                    {
                        Debug.LogWarning("(Scene Manager) Unreferenced Explodable object found: " + explodable.name);
                    }
                }
            }
#endif
        }

        List<Explodable> GetList(ExplodableType a_type)
        {
            switch (a_type)
            {
                case ExplodableType.Sheep:
                    return _sheep;

                case ExplodableType.NuclearSheep:
                    return _nukeSheep;

                case ExplodableType.Crate:
                    return _crates;

                case ExplodableType.Generator:
                    return _generators;
            }

            return null;
        }

        public void AddExplodableReference(ExplodableType a_type, Explodable a_explodable)
        {
            List<Explodable> referenceList = GetList(a_type);

#if UNITY_EDITOR
            // Ensure object isn't already referenced
            if (_explodables.Contains(a_explodable) ||
                referenceList.Contains(a_explodable))
            {
                Debug.LogWarning(string.Format("(Scene Manager) Attempted to reference '{0}' when already referenced", a_explodable.name));
                return;
            }
#endif

            referenceList.Add(a_explodable);
            _explodables.Add(a_explodable);
        }

        void RaisePlayerWinEvent(PlayerWinState a_state)
        {
            CancelInvoke();

            // Ignore attempts to raise player events, when a state has been already determined
            if (_winState != WinState.None)
            {
                Debug.LogWarning("Attempted to raise PlayerWin Event, but player win state has already been determined, ignoring...");
                return;
            }

            // Notify listeners of event
            foreach (GameObject eventListener in _eventListeners)
            {
                ExecuteEvents.Execute<ISceneManagerEvents>(eventListener, null, (x, y) => x.OnPlayerWon(a_state));
            }

            // Set win state to current event
            _winState = WinState.Win;
        }

        public void RemoveExplodableReference(ExplodableType a_type, Explodable a_explodable)
        {
            List<Explodable> referenceList = GetList(a_type);

#if UNITY_EDITOR
            // Ensure object is referenced
            if (!_explodables.Contains(a_explodable) ||
                !referenceList.Contains(a_explodable))
            {
                Debug.LogWarning(string.Format("(Scene Manager) Attempted remove to reference '{0}' when not already referenced", a_explodable.name));
                return;
            }
#endif

            referenceList.Remove(a_explodable);
            _explodables.Remove(a_explodable);

            // Raise player win/loss event if objectives are met
            switch (a_type)
            {
                case ExplodableType.Sheep:
                    if (_player.explosionCount <= 0 && sheepCount > 0 &&
                        _generators.Count > 0)
                    {
                        // HACK: Fixes bug where having no explodables with
                        // an explosions left fails player
                        Invoke("NoExplosionsLeftFailure", 1.0f);
                    }
                    else if (sheepCount <= 0)
                    {
                        RaisePlayerWinEvent(PlayerWinState.AllSheepDestroyed);
                    }
                    break;

                case ExplodableType.NuclearSheep:
                    RaisePlayerLossEvent(PlayerLoseState.DetonatedNukeSheep);
                    break;

                case ExplodableType.Crate:
                    break;

                case ExplodableType.Generator:
                    if (generatorsCount <= 0)
                    {
                        RaisePlayerWinEvent(PlayerWinState.AllGeneratersDestroyed);
                    }
                    break;
            }
        }

        void RaisePlayerLossEvent(PlayerLoseState a_state)
        {
            CancelInvoke();

            // Ignore attempts to raise player events, when a state has been already determined
            if (_winState != WinState.None)
            {
                Debug.LogWarning("Attempted to raise PlayerLoss Event, but player win state has already been determined, ignoring...");
                return;
            }

            // Notify listeners of event
            foreach (GameObject eventListener in _eventListeners)
            {
                ExecuteEvents.Execute<ISceneManagerEvents>(eventListener, null, (x, y) => x.OnPlayerFailed(a_state));
            }

            // Set win state to current event
            _winState = WinState.Loss;
        }

        void NoExplosionsLeftFailure()
        {
            RaisePlayerLossEvent(PlayerLoseState.NoExplosionsLeft);
        }
    }
}
