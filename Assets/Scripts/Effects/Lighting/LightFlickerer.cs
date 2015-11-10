using UnityEngine;
using System.Collections;

namespace Sheeplosion.Effects
{
    public class LightFlickerer : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        float _flickerRate;
        [SerializeField]
        bool _randomiseTime = false;

        Light _light;

        float _currentflickerTime;

        public void Awake()
        {
            // Get light component
            _light = GetComponent<Light>();
            if (_light == null)
            {
                Debug.LogError(string.Format("({0}) Unable to find Light component on object", this.name));
            }
        }

        void Start()
        {
            _currentflickerTime = 0.0f;

        }

        void Update()
        {
            if (_currentflickerTime >= _flickerRate)
            {
                // Toggle light, and reset timer
                _light.enabled = !_light.enabled;

                if (_randomiseTime)
                {
                    _currentflickerTime = Random.Range(0.0f, _flickerRate);
                }
                else
                {
                    _currentflickerTime = 0.0f;
                }
            }

            // Tick timer
            _currentflickerTime += Time.deltaTime;
        }
    }
}
