using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public float Speed;

        private void Awake()
        {
            Invoke(nameof(Complete), 10f);

            _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        }

        private void Update()
        {
            transform.position -= new Vector3(0, Speed * Time.deltaTime, 0f);
        }
        
        public void Complete()
        {
            CancelInvoke(nameof(Complete));
            
            Destroy(gameObject);
        }
    }
}