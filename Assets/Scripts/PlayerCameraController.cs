using UnityEngine;
using System.Collections;

namespace Sheeplosion
{
    public class PlayerCameraController : MonoBehaviour
    {
#if UNITY_STANDALONE
        [Header("Controls")]
        [SerializeField]
        KeyCode _moveForwardKey     = KeyCode.W;
        [SerializeField]
        KeyCode _moveBackwardKey    = KeyCode.S;
        [SerializeField]
        KeyCode _moveLeftKey        = KeyCode.A;
        [SerializeField]
        KeyCode _moveRightKey       = KeyCode.D;
#endif
        [Header("Movement Limits")]
        [SerializeField]
        float _moveSpeed            = 20.0f;
        [SerializeField]
        float _zoomSpeed            = 50.0f;

        [SerializeField]
        float _maxZoomDistance      = 100.0f;
        [SerializeField]
        float _minZoomDistance      = 1.0f;

#if UNITY_EDITOR
        [Header("Editor Only")]
        [SerializeField]
        bool _simulateTouch = false;
#endif

        Transform _moveTransform;
        Transform _zoomTransform;
        Vector3 _lastMousePos;

        public void Awake()
        {
            // Get relevant transforms for movement and zooming
            _zoomTransform = transform;
            _moveTransform = transform.parent.parent;
        }

        void Start()
        {

        }

        void Update()
        {
            Vector3 transformDirection = Vector3.zero;
            float zoomAmount = 0.0f;

#if UNITY_STANDALONE
            // Camera translation
            if (Input.GetKey(_moveForwardKey))
            {
                transformDirection.z += 1.0f;
            }
            if (Input.GetKey(_moveBackwardKey))
            {
                transformDirection.z -= 1.0f;
            }
            if (Input.GetKey(_moveLeftKey))
            {
                transformDirection.x -= 1.0f;
            }
            if (Input.GetKey(_moveRightKey))
            {
                transformDirection.x += 1.0f;
            }
            
            // Prevent diagonal movement from moving camera faster than
            // non-diagonal movement
            if (transformDirection.x != 0.0f && transformDirection.z != 0.0f)
            {
                transformDirection.x *= 0.5f;
                transformDirection.z *= 0.5f;
            }

    #if UNITY_EDITOR
            if (_simulateTouch)
            {
                // Simulate touch with mouse input
                if (Input.GetMouseButton(0))
                {
                    Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0.0f, Input.mousePosition.y);

                    transformDirection = mousePosition - _lastMousePos;
                    transformDirection *= -1.0f;
                }

                _lastMousePos = new Vector3(Input.mousePosition.x, 0.0f, Input.mousePosition.y);

                // Camera zoom
                zoomAmount = Input.mouseScrollDelta.y;
            }
    #endif

#else
            // Camera translation
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                transformDirection = touch.deltaPosition;
            }
            // Camera zoom
            else if (Input.touchCount == 2)
            {
                Touch firstTouch    = Input.GetTouch(0);
                Touch secondTouch   = Input.GetTouch(1);

                // TODO: Camera zoom - touch input
            }
#endif
            TransformCamera(transformDirection);
            ZoomCamera(zoomAmount);
        }

        void TransformCamera(Vector3 a_direction)
        {
            // Calculate translation vector
            Vector3 translation = a_direction * (_moveSpeed * Time.deltaTime);

            // TODO: Keep translation within limits

            _moveTransform.Translate(translation);
        }

        void ZoomCamera(float a_amount)
        {
            float zoomAmount = a_amount;

            // Calculate local forward vector
            Vector3 localForward = _zoomTransform.worldToLocalMatrix.MultiplyVector(_zoomTransform.forward);

            Vector3 zoomVector = localForward * (Time.deltaTime * _zoomSpeed) * zoomAmount;

            // TODO: Keep zoom vector within limits

            // Transform camera along local forward vector,
            // using zoom amount as a scalar
            _zoomTransform.Translate(zoomVector);
        }
    }
}
