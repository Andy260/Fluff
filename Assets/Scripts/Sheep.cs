using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class Sheep : MonoBehaviour, IExplodable
    {

        void Start()
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            player.sheepCount += 1;
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
