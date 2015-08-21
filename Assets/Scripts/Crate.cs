using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class Crate : MonoBehaviour, IExplodable
    {
        public GameObject _explosion;
        Sheep _sheep;

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

            // Destroy this object
            Destroy(this.gameObject);
        }
    }
}
