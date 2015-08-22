using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fluffy
{
    public class Sheep : MonoBehaviour, IExplodable
    {
        [Tooltip("Explosion prefab")]
        public GameObject _explosion;

        public Color _highlightColour = new Color(1.0f, 0.0f, 0.0f);

        float _highlightTime = 0.0f;
        public float highlightTime
        {
            set
            {
                _highlightTime = value;
            }
        }

        float _highlightCount = 0.0f;

        bool _shouldHighlight = false;

        public Color _colour = new Color(0.0f, 0.0f, 1.0f);

        List<Renderer> _modelRenderers;

        float _range;
        public float range
        {
            get
            {
                return _range;
            }
        }

        Player _player;

        void Start()
        {
            // Increment count of sheep within level
            _player = GameObject.Find("Player").GetComponent<Player>();
            _player.sheepCount += 1;

            // Get range
            SphereCollider collider = GetComponent<SphereCollider>();
            _range = collider.radius;
            Destroy(collider);

            // Get all model renderers
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            _modelRenderers = new List<Renderer>(renderers.Length);

            for (int i = 0; i < renderers.Length; ++i)
            {
                _modelRenderers.Add(renderers[i]);
            }
        }

        public void OnDestroy()
        {
            _player.sheepCount -= 1;
        }

        void Update()
        {
            if (!_shouldHighlight)
            {
                return;
            }

            if (_highlightCount > _highlightTime)
            {
                for (int i = 0; i < _modelRenderers.Count; ++i)
                {
                    // Reset highlighting
                    _modelRenderers[i].material.color = _colour;
                    _shouldHighlight = false;
                    _highlightCount = 0.0f;
                }
            }
            else
            {
                // Highlight counter
                _highlightCount += Time.deltaTime;
            }
        }

        public void Explode()
        {
            // Create explosion object
            GameObject explosionObject = Instantiate(_explosion, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f))) as GameObject;

            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.range = _range;

            // Destroy this object
            Destroy(this.gameObject);
        }

        public void Highlight(float a_time)
        {
            _shouldHighlight = true;

            if (_highlightCount > 0.0f)
            {
                return;
            }

            for (int i = 0; i < _modelRenderers.Count; ++i)
            {
                _modelRenderers[i].material.color = _highlightColour;
            }

            _highlightTime = a_time;
        }
    }
}
