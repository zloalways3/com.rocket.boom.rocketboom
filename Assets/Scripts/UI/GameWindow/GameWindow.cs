using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace UI
{
    public class GameWindow : Window
    {
        [Header("Top UI")] 
        [SerializeField]
        private Button _exitButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private TextMeshProUGUI _difficultyTittle;
        [SerializeField]
        private TextMeshProUGUI _taskText;


        [Header("Bottom UI")] 
        [SerializeField]
        private Button _mixButton;
        [SerializeField]
        private Button _readyButton;
        [SerializeField]
        private Timer _timer;

        
        private Dictionary<Difficulty, Vector2[]> _positionsPreset = new Dictionary<Difficulty, Vector2[]>()
        {
            {Difficulty.Easy, new []
            {
                new Vector2(-125,62.5f),
                new Vector2(0, 62.5f),
                new Vector2(125, 62.5f),
                new Vector2(-125,-62.5f),
                new Vector2(0, -62.5f),
                new Vector2(125, -62.5f)
            }},
            {Difficulty.Medium, new []
            {
                new Vector2(-125,125),
                new Vector2(0,125),
                new Vector2(125, 125),
                new Vector2(-125,0),
                new Vector2(0,0),
                new Vector2(125, 0),
                new Vector2(-125,-125),
                new Vector2(0,-125),
                new Vector2(125, -125)
            }},
            {Difficulty.Hard, new []
            {
                new Vector2(-150,150),
                new Vector2(-50, 150),
                new Vector2(50, 150),
                new Vector2(150, 150),
                new Vector2(-150,50),
                new Vector2(-50, 50),
                new Vector2(50, 50),
                new Vector2(150, 50),
                new Vector2(-150,-50),
                new Vector2(-50, -50),
                new Vector2(50, -50),
                new Vector2(150, -50),
                new Vector2(-150,-150),
                new Vector2(-50, -150),
                new Vector2(50, -150),
                new Vector2(150, -150),
            }},
        };

        [Header("Gameplay")]
        [SerializeField]
        private Transform _slotsParent;
        
        [SerializeField]
        private Slot _slotPrefab;
        private Slot[] _slots;

        [SerializeField]
        private GameItem _gameItemPrefab;
        private GameItem[] _items;

        [SerializeField]
        private Sprite[] _sprites;

        [Header("Score")] 
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        private int _score = 0;

        public override void Initialize()
        {
            base.Initialize();
            
            _exitButton.onClick.AddListener(ExitButtonPressed);
            _settingsButton.onClick.AddListener(SettingButtonPressed);
            
            _mixButton.onClick.AddListener(Shuffle);
            _readyButton.onClick.AddListener(Ready);
        }

        public override void Hide()
        {
            base.Hide();

            foreach (var slot in _slots)
            {
                Destroy(slot.gameObject);
            }

            foreach (var item in _items)
            {
                Destroy(item);
            }

            _slots = null;
            _items = null;
        }

        private void ExitButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(MenuWindow));
        }

        private void SettingButtonPressed()
        {
            _timer.StopTimer();
            _manager.SwitchWindow(typeof(SettingsWindow));
        }
        

        public void StartGame(Difficulty difficulty)
        {
            _mixButton.gameObject.SetActive(true);
            _readyButton.gameObject.SetActive(false);
            _timer.gameObject.SetActive(false);
            
            _difficultyTittle.text = ConvertToString(difficulty);
            _taskText.text = "REMEMBER";
            
            var positions = _positionsPreset[difficulty];
            _slots = new Slot[positions.Length];
            _items = new GameItem[positions.Length];

            for(int i=0; i < _slots.Length; i++)
            {
                int id = UnityEngine.Random.Range(0, _sprites.Length);
                
                _slots[i] = Instantiate(_slotPrefab, _slotsParent);
                _slots[i].transform.localPosition = positions[i];
                _slots[i].SetId(id);
                
                if (difficulty == Difficulty.Hard)
                    _slots[i].SetSize(75f);
                else 
                    _slots[i].SetSize(100f);

                _items[i] = Instantiate(_gameItemPrefab, _slots[i].transform);
                _items[i].transform.localPosition = Vector3.zero;
                _items[i].Configure(id,_sprites[id],this);
            }

            _score = 0;
            RenderScore();
        }

        private void RenderScore()
        {
            _scoreText.text = _score.ToString();
        }


        private void Shuffle()
        {
            _taskText.text = "REPEAT\nTHE SEQUENCE";
            List<Slot> slotsToShuffle = _slots.ToList();

            Shuffle(slotsToShuffle);

            for (int i = 0; i < _items.Length; i++)
            {
                _items[i].AttachToNewSlot(slotsToShuffle[i]);
            }
            
            
            
            _mixButton.gameObject.SetActive(false);
            _readyButton.gameObject.SetActive(true);
            _timer.gameObject.SetActive(true);
            
            _timer.StartTime(Ready);

            foreach (var item in _items)
            {
                item.Draggable();
            }
        }
        
        private void Ready()
        {
           _timer.StopTimer();

           bool isCorrect = CheckAnswer();
           
           TimeSpan secondsLeft = _timer.GetSecondsLeft();

           int score = GetCorrectCount() * 100;

           var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;

           completeWindow?.SetWindow(isCorrect, score, secondsLeft);
        }

        private bool CheckAnswer()
        {
            foreach (var item in _items)
            {
                if (!item.IsInCorrectPosition)
                    return false;
            }

            return true;
        }


        private String ConvertToString( Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
        
        private void Shuffle<T>(List<T> list)  
        {  
            int n = list.Count;  
            Random rng = new Random();  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public void ItemDropped()
        {
            _score = GetCorrectCount()* 100;
            
            RenderScore();
        }

        private int GetCorrectCount()
        {
            int count = 0;
            foreach (var item in _items)
            {
                if (item.IsInCorrectPosition)
                    count++;
            }

            return count;
        }
    }
}