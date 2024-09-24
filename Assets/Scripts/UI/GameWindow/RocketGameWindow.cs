using System;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class RocketGameWindow : Window
    {
        [Header("Top UI")] 
        [SerializeField]
        private Button _exitButton;
        [SerializeField]
        private Button _settingsButton;
        
        [SerializeField]
        private Timer _timer;
        
        [SerializeField]
        private Rocket _rocket;

        [SerializeField]
        private MeteorSpawner _meteorSpawner;

        private int _currentLevelIndex;
        
        
        public void StartGame(int levelIndex)
        {
             _timer.StartTime(Win ,120 + levelIndex*5);
             _meteorSpawner.StartLevel();
             _rocket.gameObject.SetActive(true);
        }
        
        public override void Initialize()
        {
            base.Initialize();

            _exitButton.onClick.AddListener(ExitButtonPressed);
            _settingsButton.onClick.AddListener(SettingButtonPressed);
            
            _rocket.Subscribe(MeteorTouched);
        }
        
        private void ExitButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(MenuWindow));
            _meteorSpawner.gameObject.SetActive(false);
            _rocket.gameObject.SetActive(false);
        }

        private void SettingButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(SettingsWindow));
            _meteorSpawner.gameObject.SetActive(false);
            _rocket.gameObject.SetActive(false);
        }

        private void MeteorTouched()
        {
            _timer.PlusTimer(10);
            _rocket.Health--;
            
            if(_rocket.Health == 0)
                Lose();
        }

        private void Lose()
        {
            _timer.StopTimer();
            _meteorSpawner.gameObject.SetActive(false);
            _rocket.gameObject.SetActive(false);
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;
            
            
            completeWindow?.SetWindow(false, 0, TimeSpan.Zero);
        }

        private void Win()
        {
            _timer.StopTimer();
            _meteorSpawner.gameObject.SetActive(false);
            _rocket.gameObject.SetActive(false);
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;
            
            completeWindow?.SetWindow(true, 0, TimeSpan.Zero);
            
            int stars = (_rocket.Health);
            
            PlayerPrefs.SetInt($"LevelDone{_currentLevelIndex}", stars);
        }
    }
}