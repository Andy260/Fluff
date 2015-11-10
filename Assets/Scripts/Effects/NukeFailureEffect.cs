using UnityEngine;
using System.Collections;
using Sheeplosion.Events;
using UnityStandardAssets.ImageEffects;

namespace Sheeplosion
{
    public class NukeFailureEffect : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        Light _directionalLight;
        [SerializeField]
        Camera _playerCamera;

        [Header("Configuration")]
        [SerializeField]
        float _effectShiftRate = 0.5f;

        [Range(0.0f, 8.0f)]
        [SerializeField]
        float _lightMaxIntensity = 8.0f;

        [Space(10)]
        [Range(0.0f, 1.5f)]
        [SerializeField]
        float _bloomMaxThreshold;
        [Range(0.0f, 2.5f)]
        [SerializeField]
        float _bloomMaxIntensity;

        // Reference to Bloom script attached to player camera
        BloomOptimized _playerBloom;
        
        // Lerp values
        float _rawLerp;
        float _lerpTime;

        public float effectShiftTime
        {
            get
            {
                return _effectShiftRate;
            }

            set
            {
                _effectShiftRate = value;
            }
        }

        public void Awake()
        {
            // Ensure directional light reference is not null
            if (_directionalLight == null)
            {
                Debug.LogError(string.Format("({0}) Directional Light reference not set, or null"));
            }
            else if (_directionalLight.type != LightType.Directional)
            {
                // Notify user if given directional light reference, is not a directional light
                Debug.LogWarning(string.Format("({0}) Directional Light reference set to light which is not a directional light"));
            }

            // Ensure player camera reference is not null
            if (_playerCamera == null)
            {
                Debug.LogError(string.Format("({0}) Player Camera reference not set, or null"));
            }
            else
            {
                // Get Bloom Optimized component on player camera
                _playerBloom = _playerCamera.GetComponent<BloomOptimized>();

                if (_playerBloom == null)
                {
                    // Unable to find component on player camera, 
                    // attach new Bloom Optimized component to player camera, and log it
                    _playerBloom = _playerCamera.gameObject.AddComponent<BloomOptimized>();

                    Debug.LogWarning(string.Format("({0}) Unable to find Bloom Optimized Component on player camera, creating...", 
                        this.name));
                }
            }
        }

        void Update()
        {
            // Lerp bloom values
            _playerBloom.intensity = Mathf.Lerp(_playerBloom.intensity, _bloomMaxIntensity, _lerpTime);
            _playerBloom.threshold = Mathf.Lerp(_playerBloom.threshold, _bloomMaxThreshold, _lerpTime);

            // Lerp directional light values
            _directionalLight.intensity = Mathf.Lerp(_directionalLight.intensity, _lightMaxIntensity, _lerpTime);

            // Update lerp time
            _rawLerp += Time.deltaTime * _effectShiftRate;
            _lerpTime = Mathf.Min(_rawLerp, 1.0f);
        }
    }
}
