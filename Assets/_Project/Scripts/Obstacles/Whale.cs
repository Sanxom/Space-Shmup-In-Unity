using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : MonoBehaviour
{
    private void Update()
    {
        float boostMultiplier = PlayerController.Instance.BoostChecking();
        float moveX = GameManager.Instance.WorldSpeed * boostMultiplier * Time.deltaTime;

        transform.position += new Vector3(-moveX, 0);

        if (transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }
}