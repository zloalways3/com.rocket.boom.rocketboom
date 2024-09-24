using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CompleteWindow : Window
    {
        [Header("Common")] 
        [SerializeField]
        private TextMeshProUGUI _tittle;
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private TextMeshProUGUI _timerText;
        [SerializeField]
        private Button _exitButton;
        
        [Header("Win UI")]
        [SerializeField]
        private GameObject _winImage;
        [SerializeField]
        private Button _winButton;

        
        [Header("Lose UI")]
        [SerializeField]
        private GameObject _loseImage;
        [SerializeField]
        private Button _tryAgainButton;

        public override void Initialize()
        {
            base.Initialize();
            
            _tryAgainButton.onClick.AddListener(OnTryAgainPressed);
            _winButton.onClick.AddListener(OnTryAgainPressed);
            _exitButton.onClick.AddListener(OnExitPressed);
        }

        private void OnExitPressed()
        {
            _manager.SwitchWindow(typeof(MenuWindow));
        }

        private void OnTryAgainPressed()
        {
            _manager.SwitchWindow(typeof(LevelSelectionWindow));
        }

        public void SetWindow(bool isWin, int score, TimeSpan timesLeft)
        {
            if (isWin)
            {
                _winImage.gameObject.SetActive(true);
                _loseImage.gameObject.SetActive(false);
                _scoreText.text = score.ToString();
                _timerText.text = timesLeft.ToString("mm\\:ss");
                _timerText.gameObject.SetActive(true);
                _tittle.text = "You Win!";
                _tittle.color = Color.white;
            }
            else
            {
                _winImage.gameObject.SetActive(false);
                _loseImage.gameObject.SetActive(true);
                _scoreText.text = score.ToString();
                _timerText.text = timesLeft.ToString("mm\\:ss");
                _timerText.gameObject.SetActive(true);
                _tittle.text = "You Lose!";
                _tittle.color = Color.white;
            }

            //_scoreText.text = .ToString();
            
        }
    }
}