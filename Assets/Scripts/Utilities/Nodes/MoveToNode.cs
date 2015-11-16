using UnityEngine;

namespace Sheeplosion.Utilities
{
    public class MoveToNode : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Will instruct the player's Cinematic Camera to move to this node")]
        [SerializeField]
        MoveToNode _nextNode;

        [Tooltip("Seconds to wait before instructing player's Cinematic Camera to move to the next node")]
        [SerializeField]
        float _delay;

        CinematicCamera _playerCam;

        void Awake()
        {
            // Find player within scene
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError(string.Format("({0}) Unable to find player. Have you tagged the player with 'Player'?", 
                    this.name));
            }
            else
            {
                // Get Cinematic Camera component from player
                _playerCam = player.GetComponentInChildren<CinematicCamera>();
                if (_playerCam == null)
                {
                    Debug.LogWarning(string.Format("({0}) Didn't find Cinematic Camera attached to player", 
                        this.name));
                }
            }
        }

        void Update()
        {

        }

        public void OnTriggerEnter(Collider a_other)
        {
            if (!a_other.CompareTag("Player"))
            {
                return;
            }

            if (_nextNode != null)
            {
                // Tell player to move to the next node
                Invoke("MovePlayerToNextNode", _delay);

                Debug.Log(string.Format("({0}) Moving player to next node in {1} seconds...", this.name, _delay));
            }
        }

        void MovePlayerToNextNode()
        {
            _playerCam.MoveTo(_nextNode);
        }
    }
}
