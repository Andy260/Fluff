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
            _sheep = transform.GetComponentInChildren<Sheep>();

            // Ensure sheep is hide on scene start
            _sheep.gameObject.SetActive(false);
        }

        void Update()
        {

        }

        public void Explode()
        {
            // Create explosion object
            GameObject explosionObject = Instantiate(_explosion, transform.position,
                Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f))) as GameObject;

            // Create explosion
            Explosion explosion     = explosionObject.GetComponent<Explosion>();
            explosion.sheepToShow   = _sheep;

            // Unparent hidden sheep
            _sheep.transform.SetParent(null);

            // Destroy this object
            Destroy(this.gameObject);
        }
    }
}
