using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserBullet : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(PhaserWeapon.Instance.Speed * Time.deltaTime, 0f);

        if (transform.position.x > 9)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollideable collideable))
        {
            collideable.Collide();
            Destroy(gameObject);
        }
    }
}