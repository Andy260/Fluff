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

        private GUISystem _guiSystem;

        public GameObject _rangeDisplay;

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

            _guiSystem = GameObject.Find("GUI System").GetComponent<GUISystem>();
        }

        void Update()
        {
            if (!_hasFocus || _guiSystem.levelEnded)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * 20.0f), new Color(1.0f, 1.0f, 1.0f));

            if (Input.GetMouseButtonDown(0) && 
                    _explosivesCount < _explosivesTotal)
            {
                ExplodeObject(ray);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ShowSheepRange(ray);
            }
        }

        void ExplodeObject(Ray a_ray)
        {
            RaycastHit raycastHit;

            if (Physics.Raycast(a_ray, out raycastHit, Mathf.Infinity,
                    _explodableLayerMask))
            {
                GameObject hitObject = raycastHit.transform.gameObject;

                if (hitObject.tag == "Sheep")
                {
                    // Sheep
                    Sheep sheep = hitObject.GetComponent<Sheep>();

                    sheep.Explode();

                    _guiSystem.explosionCount += 1;

                    _explosivesCount++;
                }
            }
        }

        void ShowSheepRange(Ray a_ray)
        {
            RaycastHit raycastHit;

            if (Physics.Raycast(a_ray, out raycastHit, Mathf.Infinity,
                    _explodableLayerMask))
            {
                GameObject hitObject = raycastHit.transform.gameObject;

                if (hitObject.tag != "Sheep")
                {
                    return;
                }

                Sheep sheep = hitObject.GetComponent<Sheep>();

                // Create range display game object
                GameObject rangeDisplayObject = Instantiate(_rangeDisplay, 
                    sheep.transform.position, new Quaternion()) as GameObject;

                // Set range display range
                RangeDisplay rangeDisplay = rangeDisplayObject.GetComponent<RangeDisplay>();
                rangeDisplay._range = sheep.range;
            }
        }

        public void OnApplicationFocus(bool a_focus)
        {
            _hasFocus = a_focus;
        }
    }
}
