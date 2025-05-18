using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Transform minPos;
        [SerializeField] private Transform maxPos;
        [SerializeField] private int waveNumber;
        [SerializeField] private List<Wave> waves;

        [Serializable]
        public class Wave
        {
            public GameObject prefab;
            //public Transform parentObject;
            public float spawnTimer;
            public float spawnInterval;
            public int objectsPerWave;
            public int spawnedObjectCount;
        }

        private void Update()
        {
            WaveController();
        }

        private Vector2 RandomSpawnPoint()
        {
            Vector2 spawnPoint;

            spawnPoint.x = minPos.position.x;
            spawnPoint.y = UnityEngine.Random.Range(minPos.position.y, maxPos.position.y);

            return spawnPoint;
        }

        private void SpawnObject(Wave wave)
        {
            ObjectPoolManager.SpawnObject(wave.prefab, RandomSpawnPoint(), transform.rotation);
            wave.spawnedObjectCount++;
        }

        private void WaveController()
        {
            Wave wave = waves[waveNumber];
            float boostMultiplier = PlayerController.Instance.BoostChecking();
            wave.spawnTimer += Time.deltaTime * boostMultiplier;
            if (wave.spawnTimer >= wave.spawnInterval)
            {
                wave.spawnTimer = 0;
                SpawnObject(wave);
            }

            if (wave.spawnedObjectCount >= wave.objectsPerWave)
            {
                wave.spawnedObjectCount = 0;
                waveNumber++;
                if (waveNumber >= waves.Count)
                {
                    waveNumber = 0;
                }
            }
        }
    }
}