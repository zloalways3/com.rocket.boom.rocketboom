using System.Collections.Generic;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DifferenceGameWindow : Window
    {
        [SerializeField]
        private Image _slotPrefab;

        [SerializeField]
        private TMP_Text _levelText;

       
        private Image[,] slots;

        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;

        [SerializeField] private Vector2Int XYCOunt;

        [SerializeField] private TapItem itemPrefab;

        [SerializeField] private Sprite _rightSprite;
        [SerializeField] private Sprite _wrongSprite;

        private TapItem[,] items;

        [SerializeField]
        private ScoreHandler _scoreHandler;
        
        [SerializeField]
        private Timer _timer;
        
        private bool _started;

        private int _currentLevelIndex;

        private int currentCount = 0;
        private int maxCount = 3;

        public override void Initialize()
        {
            base.Initialize();

            slots = new Image[XYCOunt.x, XYCOunt.y]; 
            items = new TapItem[XYCOunt.x, XYCOunt.y];
            
            for (int x = 0; x < XYCOunt.x; x++)
            {
                for (int y = 0; y < XYCOunt.y; y++)
                {
                    slots[x, y] = Instantiate(_slotPrefab, _gridLayoutGroup.transform);
                    items[x, y] = Instantiate(itemPrefab, slots[x, y].transform);
                    items[x,y].transform.localPosition = Vector3.zero;
                    items[x, y].OnTap += OnTap;
                }
            }

            _scoreHandler.OnWin += Win;
        }

        private void OnTap(TapItem obj)
        {
            if (obj.IsRightItem)
            {
                obj.Image.sprite = _rightSprite;
                obj.Image.color = Color.clear;
                obj.Image.raycastTarget = false;
                obj.IsRightItem = false;
                _scoreHandler.PlusScore(10);

                currentCount++;

                if (currentCount >= maxCount)
                {
                    ChangeSprite();
                }
            }
            else
            {
                _timer.MinusTimer(2);
            }
        }

        public void StartGame(int levelIndex)
        {
            _levelText.text = "Level " + (levelIndex+1).ToString();
            _timer.StartTime(Lose ,60 + levelIndex*5);
            _scoreHandler.Score = 0;
            _scoreHandler.MaxScore = 100 + levelIndex * 10;
            _scoreHandler.RenderScore();
            
            ChangeSprite();
        }

        private void ChangeSprite()
        {
            currentCount = 0;
            
            List<Vector2Int> rightPosition = new  List<Vector2Int>();

            while (rightPosition.Count!= maxCount)
            {
                var pos = new Vector2Int(Random.Range(0, XYCOunt.x), Random.Range(0, XYCOunt.y));
                
                if(!rightPosition.Contains(pos))
                    rightPosition.Add(pos);
                
            }
            
            for (int x = 0; x < XYCOunt.x; x++)
            {
                for (int y = 0; y < XYCOunt.y; y++)
                {
                    if (rightPosition.Contains(new Vector2Int(x, y)))
                    {
                        items[x, y].IsRightItem = true;
                        items[x, y].Image.sprite = _wrongSprite;
                    }
                    else
                    {
                        items[x, y].IsRightItem = false;
                        items[x, y].Image.sprite = _rightSprite;
                    }
                    
                    items[x, y].Image.color = Color.white;
                    items[x, y].Image.raycastTarget = true;
                }
            }
        }
        
        public void Win()
        {
            _timer.StopTimer();

            var timesLeft = _timer.GetSecondsLeft();
            
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;

            // int stars = 1;
            //
            // if (timesLeft.TotalSeconds >= 60)
            // {
            //     stars = 3;
            // }
            // else if( timesLeft.TotalSeconds >=30)
            // {
            //     stars = 2;
            // }
            
            // if (!PlayerPrefs.HasKey($"LevelDone{_currentLevelIndex}"))
            // {
            //     PlayerPrefs.SetInt($"LevelDone{_currentLevelIndex}", stars);
            // }

            completeWindow?.SetWindow(true, _scoreHandler.Score, timesLeft);
        }

        public void Lose()
        {
            _timer.StopTimer();
         
            var completeWindow =_manager.SwitchWindow(typeof(CompleteWindow)) as CompleteWindow;
            
            completeWindow?.SetWindow(false, 0, _timer.GetSecondsLeft());
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
            // if (_currentMap != null)
            // {
            //     Destroy(_currentMap.gameObject);
            // }
            //
            // _dotInMaze.gameObject.SetActive(false);
        }

    }
}