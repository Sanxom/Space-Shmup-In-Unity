using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserWeapon : MonoSingleton<PhaserWeapon>
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }

    [SerializeField] private GameObject prefab;

    public void Shoot()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}