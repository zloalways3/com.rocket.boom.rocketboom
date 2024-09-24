using System;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class MazeGameWindow : Window
    {
        [SerializeField]
        private Rigidbody2D _dotInMaze;

        [SerializeField]
        private Transform MapPoint;

        [SerializeField]
        private MapPrefab[] _maps;

        [SerializeField]
        private float _dotSpeed;
        
        [SerializeField]
        private Timer _timer;


        private MapPrefab _currentMap;

        private bool _started;

        private int _currentLevelIndex;
        
        public void StartGame(int levelIndex)
        {
            if (levelIndex>= _maps.Length)
            {
                levelIndex = 0;
            }

            _currentLevelIndex = levelIndex;
            
            _currentMap = Instantiate(_maps[levelIndex], MapPoint);

            _dotInMaze.transform.position = _currentMap.StartPosition;
            _dotInMaze.gameObject.SetActive(true);
            _timer.StartTime(Lose);
            ButtonReleased();

            _started = true;
        }

        public override void Hide()
        {
            base.Hide();

            DestroyLevel();

        }

        public void ExitButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(MenuWindow));
        }

        public void SettingButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(SettingsWindow));
        }
        
        private void DestroyLevel()
        {
            if (_currentMap != null)
            {
                Destroy(_currentMap.gameObject);
            }
            
            _dotInMaze.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (!_started)
                return;
            
            if (Vector2.Distance(_dotInMaze.position, _currentMap.ExitPosition) <= 0.2f)
            {
                Win();
                _started = false;
            }
        }

        public void Win()
        {
            _timer.StopTimer();
            _dotInMaze.gameObject.SetActive(false);
            ButtonReleased();

            var timesLeft = _timer.GetSecondsLeft();
            
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;

            int stars = 1;

            if (timesLeft.TotalSeconds >= 60)
            {
                stars = 3;
            }
            else if( timesLeft.TotalSeconds >=30)
            {
                stars = 2;
            }
            
            if (!PlayerPrefs.HasKey($"LevelDone{_currentLevelIndex}"))
            {
                PlayerPrefs.SetInt($"LevelDone{_currentLevelIndex}", stars);
            }

            completeWindow?.SetWindow(true, stars, timesLeft);
        }

        public void Lose()
        {
            _timer.StopTimer();
            _dotInMaze.gameObject.SetActive(false);
            ButtonReleased();
            
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;
            
            completeWindow?.SetWindow(false, 0, _timer.GetSecondsLeft());
        }

        public void ButtonDown()
        {
            _dotInMaze.constraints = RigidbodyConstraints2D. None;
        }

        public void PressedButton(Vector3 direction)
        {
            _dotInMaze.velocity = direction*Time.fixedDeltaTime * _dotSpeed;
        }

        public void ButtonReleased()
        {
            _dotInMaze.velocity = Vector2.zero;
            _dotInMaze.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}