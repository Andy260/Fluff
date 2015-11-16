using UnityEngine;

namespace Sheeplosion.GUI.Menus
{
    public class MenuItemSelector : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        LayerMask _selectorLayerMask;

        Camera _camera;

        // Cached GameObject properties
        Transform _transform;

        public void Awake()
        {
            _transform = transform;

            _camera = GetComponentInChildren<Camera>();
        }
           
        void Update()
        {
            if (Input.touchCount != 1)
            {
                return;
            }

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                // Raycast to any menu items
                Ray ray = _camera.ScreenPointToRay(touch.position);

                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    GameObject hitGameObject = raycastHit.collider.gameObject;

                    if (hitGameObject.CompareTag("Menu Item"))
                    {
                        hitGameObject.SendMessageUpwards("Select");
                    }
                }

                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f);
            }
        }
    }
}
