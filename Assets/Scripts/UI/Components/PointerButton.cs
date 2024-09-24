using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Components
{
    public class PointerButton : MonoBehaviour, IPointerDownHandler,IUpdateSelectedHandler, IPointerUpHandler
    {
        [SerializeField]
        private Vector2 direction;

        [SerializeField]
        private MazeGameWindow _mazeGameWindow;

        private bool _clicked;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _clicked = true;
            _mazeGameWindow.ButtonDown();
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (!_clicked)
                return;
            Debug.Log("Pressed");
            _mazeGameWindow.PressedButton(direction);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _clicked = false;
            _mazeGameWindow.ButtonReleased();
        }
    }
}