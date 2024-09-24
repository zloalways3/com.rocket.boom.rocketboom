using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;

        private Vector2 _prevVector;

        private Action OnTrigger;

        public int Health = 3;

        public void Subscribe(Action onTrigger) => OnTrigger = onTrigger;

        private void OnEnable()
        {
            transform.position = _startPoint.position;
            Health = 3;
        }
        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newPosition.y = transform.position.y;
                newPosition.z = transform.position.z;

                transform.position = newPosition;
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var meteor = other.GetComponent<Meteor>();

            if (meteor != null)
            {
                meteor.Complete();
                OnTrigger?.Invoke();
            }
                
        }
    }
}