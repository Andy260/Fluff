using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class RangeDisplay : MonoBehaviour
    {
        public float _lifeTime = 0.5f;
        float _currentLifeTime = 0.0f;

        public float _range;
        public float range
        {
            set
            {
                _range = value;
            }
        }

        GameObject _model;

        float scale
        {
            get
            {
                if (_currentLifeTime == 0.0f)
                {
                    return float.Epsilon;
                }

                return (_range * (_currentLifeTime / _lifeTime)) * 2.0f;
            }
        }

        void Start()
        {
            _model = transform.GetChild(0).gameObject;
            _model.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
        }

        void Update()
        {
            if (_currentLifeTime > _lifeTime)
            {
                Destroy(this.gameObject);
            }
            else
            {
                // Increment timer
                _currentLifeTime += Time.deltaTime;
            }

            _model.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
