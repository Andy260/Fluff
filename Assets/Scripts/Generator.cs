using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class Generator : MonoBehaviour
    {
        public GameObject _explosion;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Explode()
        {
            // Create explosion object
            GameObject explosionObject = Instantiate(_explosion, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f))) as GameObject;

            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.triggerByGenerator = true;

            // Destroy this object
            Destroy(this.gameObject);
        }
    }
}
