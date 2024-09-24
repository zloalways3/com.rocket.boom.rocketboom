using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameItem : MonoBehaviour, IDragHandler, IDropHandler
    {
        [SerializeField]
        private Image _image;
        
        private Slot _currentSlot;

        public bool IsInCorrectPosition => _currentSlot.Id == _id;

        private bool isDraggable = false;

        private int _id;

        private GameWindow _game;

        public void Undraggable()
        {
            isDraggable = false;
        }

        public void SetCurrentSlot(Slot slot)
        {
            _currentSlot = slot;
        }

        public void Configure(int id, Sprite sprite, GameWindow game)
        {
            _id = id;
            _image.sprite = sprite;
            _image.SetNativeSize();
            _game = game;
           Undraggable(); 
        }

        public void SetParent(Transform newParent)
        {
            transform.SetParent(newParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDraggable)
                return;

            transform.position = eventData.position;
        }


        public void OnDrop(PointerEventData eventData)
        {
            eventData.position = transform.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,results);

            
            GameItem item = null;

            foreach (var result in results)
            {
                var fooItem = result.gameObject.GetComponent<GameItem>();
                if (fooItem != null)
                    item = fooItem;
            }


            if (item != null && item != this)
            {
                var newItemSlot = item._currentSlot;
                var oldItemSlot = _currentSlot;


                AttachToNewSlot(newItemSlot);
                
                item.AttachToNewSlot(oldItemSlot);

                _game.ItemDropped();
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
            
        }

        public void AttachToNewSlot(Slot slot)
        {
            SetCurrentSlot(slot);
            SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
        }

        public void Draggable()
        {
            isDraggable = true;
        }
    }
}