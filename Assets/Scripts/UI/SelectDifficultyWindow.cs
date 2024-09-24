using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectDifficultyWindow : Window
    {
        [Header("Difficult Select")]
        [SerializeField]
        private Button _easyButton;
        [SerializeField]
        private Button _mediumButton;
        [SerializeField]
        private Button _hardButton;
        [SerializeField]
        private Button _exitToMenu;
        [SerializeField]
        private Transform _difficultHighlight;
        

        private Difficulty _difficulty;


        public override void Initialize()
        {
            base.Initialize();
            
            _easyButton.onClick.AddListener(()=> Play(Difficulty.Easy));
            _mediumButton.onClick.AddListener(()=> Play(Difficulty.Medium));
            _hardButton.onClick.AddListener(()=> Play(Difficulty.Hard));
            _exitToMenu.onClick.AddListener(()=>_manager.SwitchWindow(typeof(MenuWindow)));
        }

        public override void Show()
        {
            base.Show();

            if (_isFirstShow)
            {
                SetHighlightToDifficulty(Difficulty.Easy);
            }
            else
            {
                SetHighlightToDifficulty(_difficulty);
            }
        }

        private void Play(Difficulty difficulty)
        {
            _difficulty = difficulty;
            
            var game = _manager.SwitchWindow(typeof(GameWindow)) as GameWindow;

            // ReSharper disable once Unity.NoNullPropagation
            game?.StartGame(_difficulty);
        }


        private void SetHighlightToDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _difficultHighlight.SetParent(_easyButton.transform);
                    break;
                case Difficulty.Medium:
                    _difficultHighlight.SetParent(_mediumButton.transform);
                    break;
                case Difficulty.Hard:
                    _difficultHighlight.SetParent(_hardButton.transform);
                    break;
            }
            
            _difficultHighlight.transform.localPosition = Vector3.zero;
        }
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}