using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class Whale : MonoBehaviour
    {
        [SerializeField] private int returnToPoolValue;

        private void Update()
        {
            float boostMultiplier = PlayerController.Instance.BoostChecking();
            float moveX = GameManager.Instance.WorldSpeed * boostMultiplier * Time.deltaTime;

            transform.position += new Vector3(-moveX, 0);

            if (transform.position.x < returnToPoolValue)
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}