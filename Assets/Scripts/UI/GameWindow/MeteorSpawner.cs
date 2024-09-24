using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

namespace UI
{
    public class MeteorSpawner : MonoBehaviour
    {
        [SerializeField] private Meteor Meteor;
        [SerializeField] private float Speed;
        [SerializeField] private float SpeedRandom;

        [SerializeField] private float Borders;

        private bool isEnabled = false;

        
        private float timer;

        private void Update()
        {
            if (isEnabled)
            {
                if (timer <= 0)
                {
                    var meteor = Instantiate(Meteor, transform);
                    meteor.transform.position += new Vector3(Random.Range(-Borders, Borders), 0, 0);
                    meteor.Speed = Speed + Random.Range(-SpeedRandom, SpeedRandom);
                    timer = Random.Range(1f, 2f);
                }

                timer -= Time.deltaTime;
            }
        }

        public void StartLevel()
        {
            isEnabled = true;
            gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            isEnabled = false;
        }
        
    }
}