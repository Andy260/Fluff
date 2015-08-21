using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class GUISystem : MonoBehaviour
    {
        GameObject _failureWindow;
        GameObject _successWindow;

        Player _player;

        bool _levelEnded = false;
        public bool levelEnded
        {
            get
            {
                return _levelEnded;
            }
        }

        int _explosionCount = 0;
        public int explosionCount
        {
            get
            {
                return _explosionCount;
            }

            set
            {
                _explosionCount = value;
            }
        }

        void Start()
        {
            _failureWindow = transform.FindChild("Failure Window").gameObject;
            _successWindow = transform.FindChild("Success Window").gameObject;

            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        void Update()
        {
            if (_explosionCount > 0)
            {
                return;
            }

            if (_player.sheepCount <= 0)
            {
                _successWindow.SetActive(true);
            }
            else if (_player._explosivesCount >= _player._explosivesTotal)
            {
                _failureWindow.SetActive(true);
            }
        }

        public void RestartLevel()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        public void ChangeToLevelSelection()
        {
            Application.LoadLevel("Main Menu");
        }
    }
}
