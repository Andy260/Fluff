using UnityEngine;
using UnityEngine.Events;

namespace Sheeplosion.GUI.Menus
{
    public class MenuItem : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        UnityEvent _onTap;

        void Update()
        {

        }

        void Select()
        {
            if (this.isActiveAndEnabled)
            {
                _onTap.Invoke();
            }
        }
    }
}
