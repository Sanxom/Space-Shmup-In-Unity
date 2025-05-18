using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class PhaserWeapon : MonoSingleton<PhaserWeapon>
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }

        [SerializeField] private PhaserBullet prefab;

        public void Shoot()
        {
            ObjectPoolManager.SpawnObject(prefab, transform.position, transform.rotation);
        }
    }
}