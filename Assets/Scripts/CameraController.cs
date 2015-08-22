using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class CameraController : MonoBehaviour
    {
        public KeyCode _moveUp          = KeyCode.W;
        public KeyCode _moveDown        = KeyCode.S;
        public KeyCode _moveLeft        = KeyCode.A;
        public KeyCode _moveRight       = KeyCode.D;

        public float _speed;

        [Tooltip("Distance away from world origin")]
        public float _maxZoomDistance;

        GameObject _cameraObject;
        Vector3 _startCameraLocalPos;

        void Start()
        {
            _cameraObject = transform.GetComponentInChildren<Camera>().gameObject;
            _startCameraLocalPos = _cameraObject.transform.localPosition;
        }

        void Update()
        {
            Vector3 direction = new Vector3();

            if (Input.GetKey(_moveUp))
            {
                direction.z += 1.0f;
            }
            else if (Input.GetKey(_moveDown))
            {
                direction.z -= 1.0f;
            }

            if (Input.GetKey(_moveLeft))
            {
                direction.x -= 1.0f;
            }
            else if (Input.GetKey(_moveRight))
            {
                direction.x += 1.0f;
            }

            transform.Translate(direction * (Time.deltaTime * _speed));

            HandleZoom();
        }

        void HandleZoom()
        {
            float mouseScrollDelta = Input.mouseScrollDelta.y;

            Vector3 localForward = _cameraObject.transform.worldToLocalMatrix.MultiplyVector(
                _cameraObject.transform.forward);

            _cameraObject.transform.Translate(localForward * 
                (Time.deltaTime * _speed) * mouseScrollDelta);
        }
    }
}
