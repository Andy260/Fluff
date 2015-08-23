using UnityEngine;
using System.Collections;

namespace Fluffy
{
    public class GUISystem : MonoBehaviour
    {
        GameObject _failureWindow;
        GameObject _successWindow;

        Player _player;
        CameraController _playerCamController;

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
            _playerCamController = _player.GetComponent<CameraController>();
        }

        void Update()
        {
            if (_playerCamController == null)
            {
                // Player has failed
                return;
            }

            if (_player.sheepCount <= 0)
            {
                ShowSuccess();
            }
            else if (_player._explosivesCount >= _player._explosivesTotal)
            {
                ShowFailure();
            }
        }

        public void ShowFailure()
        {
            _failureWindow.SetActive(true);

            Destroy(_playerCamController);
        }

        public void ShowSuccess()
        {
            _successWindow.SetActive(true);
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
