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
        [field: SerializeField] public float FireRate { get; private set; } = 5f;

        [SerializeField] private PhaserBullet prefab;
        [SerializeField] private bool canRapidFire = false;

        private WaitForSeconds fireRateWaitTime;

        private void Awake()
        {
            fireRateWaitTime = new WaitForSeconds(1 / FireRate);
        }

        public IEnumerator RapidFire()
        {
            if (canRapidFire)
            {
                while (true)
                {
                    Shoot();
                    yield return fireRateWaitTime;
                }
            }
            else
            {
                Shoot();
                yield return null;
            }
        }

        private void Shoot()
        {
            ObjectPoolManager.SpawnObject(prefab, transform.position, transform.rotation);
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.ShootSound);
        }
    }
}