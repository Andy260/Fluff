using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class Player : MonoBehaviour
    {
        [Tooltip("The amount of explosives which the player will start with")]
        public int _explosivesTotal             = 0;        // Total number of explosives the player currently has

        public LayerMask _explodableLayerMask;              // Represents which Unity layered gameObjects may be exploded

        public int _explosivesCount            = 0;         // Represents the current number of explosives

        private Camera _camera;                             // Should be the main camera

        private int _sheepCount                 = 0;        // Current count of sheep

        private bool _hasFocus                  = true;     // Whether or not the game window is currently in focus

        public GameObject _rangeDisplay;

        void Start()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            
        }

        public void OnApplicationFocus(bool a_focus)
        {
            _hasFocus = a_focus;
        }
    }
}
