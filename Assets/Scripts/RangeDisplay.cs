﻿using UnityEngine;
using System.Collections;

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

        float scale
        {
            get
            {
                if (_currentLifeTime == 0.0f)
                {
                    return float.Epsilon;
                }

                return _range * (_currentLifeTime / _lifeTime);
            }
        }

        void Start()
        {
            
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

            transform.localScale = new Vector3(scale, 0.0f, scale);
        }
    }
}
