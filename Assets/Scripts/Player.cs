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

        /// <summary>
        /// The current sheep count
        /// </summary>
        public int sheepCount
        {
            get
            {
                return _sheepCount;
            }
            set
            {
                _sheepCount = value;
            }
        }

        void Start()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * 20.0f), new Color(1.0f, 1.0f, 1.0f));

            if (Input.GetMouseButtonDown(0)             && 
                _hasFocus                               && 
                _explosivesCount < _explosivesTotal)
            {
                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity,
                        _explodableLayerMask))
                {
                    GameObject hitObject = raycastHit.transform.gameObject;

                    if (hitObject.tag == "Sheep")
                    {
                        // Sheep
                        Sheep sheep = hitObject.GetComponent<Sheep>();

                        sheep.Explode();
                    }
                    else
                    {
                        // Crate
                        Crate crate = hitObject.GetComponent<Crate>();

                        crate.Explode();
                    }
                }

                _explosivesCount++;
            }
        }

        public void OnApplicationFocus(bool a_focus)
        {
            _hasFocus = a_focus;
        }
    }
}
