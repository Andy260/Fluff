using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class MainMenu : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {

        }

        public void LoadLevel(string a_level)
        {
            Application.LoadLevel(a_level);
        }
    }
}
