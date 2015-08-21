using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class Crate : MonoBehaviour, IExplodable
    {
        Sheep _sheep;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Explode()
        {
            Destroy(this.gameObject);
        }
    }
}
