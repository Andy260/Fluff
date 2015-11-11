using UnityEngine;
using System.Collections.Generic;

namespace Sheeplosion.Utilities
{
    public class MovingObject : MonoBehaviour
    {
        public enum MovementMethod
        {
            Lerp,
            SteeringBehaviour
        }

        [Header("Configuration")]
        [Tooltip("List of nodes which this object will wonder towards")]
        [SerializeField]
        List<Transform> _moveToTransforms;

        [SerializeField]
        MovementMethod _movementMethod = MovementMethod.SteeringBehaviour;

        [Tooltip("Should the wanderer loop back to the first node and continue infinatly?")]
        [SerializeField]
        bool _loop = false;

        [Tooltip("How fast this object will wonder, in units per second")]
        [SerializeField]
        float _movementSpeed = 10.0f;

        [Space(10)]
        [SerializeField]
        float _steeringBehaviourTolerance;

        // Movement
        int _transformID;

        Vector3 _velocity;

        // Lerping values
        float _rawLerp;
        float _lerpTime;

        Vector3 _startPosition;

        // Cached GameObject properties
        Transform _transform;

        public void Awake()
        {
            // Cache GameObject properties
            _transform = this.transform;

            // Warn user if no waypoints are set
            if (_moveToTransforms.Count == 0)
            {
                Debug.LogWarning(string.Format("({0}) No nodes for MovingObject, object will not move", this.name));
            }
        }

        void Start()
        {
            _transformID = 0;
            _startPosition = _transform.position;
        }

        void Update()
        {
            if (_moveToTransforms == null ||
                _moveToTransforms.Count < 1)
            {
                return;
            }

            Transform currentTransform = _moveToTransforms[_transformID];
            
            // Choose a movement method, and move object
            switch (_movementMethod)
            {
                case MovementMethod.SteeringBehaviour:
                    SteeringBehaviourMovement(currentTransform);
                    break;

                case MovementMethod.Lerp:
                    LerpMovement(currentTransform);
                    break;
            }

            // Rotate towards next node
            //_transform.LookAt(currentTransform);
        }

        void SteeringBehaviourMovement(Transform a_currentTransform)
        {
            Vector3 desiredVelo = a_currentTransform.position - _transform.position;
            desiredVelo.Normalize();
            desiredVelo *= _movementSpeed;

            Vector3 force = desiredVelo - _velocity;

            _velocity = force * Time.deltaTime;
            _transform.position += _velocity * Time.deltaTime;

            Debug.DrawLine(_transform.position, _transform.position + _velocity);

            Quaternion rotation = _transform.rotation;
            rotation.eulerAngles = _velocity.normalized;
            _transform.rotation = rotation;

            if ((a_currentTransform.position - _transform.position).sqrMagnitude < _steeringBehaviourTolerance)
            {
                IncrementTransformID();
            }
        }

        void LerpMovement(Transform a_currentTransform)
        {
            // Update lerp values
            _rawLerp += Time.deltaTime * _movementSpeed;
            _lerpTime = Mathf.Min(_rawLerp, 1.0f);

            if (_lerpTime >= 1.0f)
            {
                // Reset lerp and move to next node
                _lerpTime = 0.0f;
                _rawLerp = 0.0f;

                IncrementTransformID();
            }

            // Lerp to next target
            _transform.position = Vector3.Lerp(_startPosition, a_currentTransform.position, _lerpTime);
        }

        void IncrementTransformID()
        {
            // Set transform ID
            if (_transformID + 1 < _moveToTransforms.Count)
            {
                _transformID++;
            }
            else if (_loop)
            {
                _transformID = 0;
            }

            _startPosition = _transform.position;
        }
    }
}
