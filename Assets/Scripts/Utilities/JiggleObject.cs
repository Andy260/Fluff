using UnityEngine;

namespace Sheeplosion.Utilities
{
    public class JiggleObject : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        float _jiggleTime;
        [SerializeField]
        float _jiggleAmount;

        float _lastJiggleTime;

        Transform _transform;
        Vector3 _startPos;

        public void Awake()
        {
            _transform = transform;
        }

        void Start()
        {
            _startPos = _transform.localPosition;
            InvokeRepeating("Jiggle", 0.0f, _jiggleTime);

            _lastJiggleTime = _jiggleTime;
        }
        
        void Update()
        {

        }

        void Jiggle()
        {
            if (_lastJiggleTime != _jiggleTime)
            {
                CancelInvoke();
                InvokeRepeating("Jiggle", 0.0f, _jiggleTime);

                _lastJiggleTime = _jiggleTime;
            }

            Vector3 randomUnitCircle = Random.insideUnitCircle;
            randomUnitCircle *= _jiggleAmount;

            _transform.localPosition = _startPos + randomUnitCircle;
        }
    }
}
