using UnityEngine;

namespace Sheeplosion.Utilities
{
    public class CinematicCamera : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Camera will move to this object at scene start")]
        [SerializeField]
        Transform _moveToTransform;

        Transform _transform;

        void Awake()
        {
            _transform = this.transform.parent;
        }

        void Start()
        {
            // HACK: Used to reset time scale, fixing frozen main menu from pause bug
            Time.timeScale = 1.0f;
        }

        void Update()
        {
            if (_moveToTransform != null)
            {
                _transform.position = Vector3.Lerp(_transform.position, 
                    _moveToTransform.position, Time.deltaTime);

                _transform.rotation = Quaternion.Slerp(_transform.rotation, 
                    _moveToTransform.rotation, Time.deltaTime);
            }
        }

        public void MoveTo(MoveToNode a_node)
        {
            // Ensure parameters are valid,
            // and only execute if this object is active
            if (a_node == null &&
                this.isActiveAndEnabled)
            {
                Debug.LogWarning(string.Format("({0}) MoveTo() called with a null Transform", this.name));
            }

            _moveToTransform = a_node.transform;
        }
    }
}
