using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class Boom : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private WaitForSeconds destroyWaitTime;

        private void Awake()
        {
            destroyWaitTime = new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        private void OnEnable()
        {
            StartCoroutine(DestroyCoroutine());
        }

        private void Update()
        {
            float boostMultiplier = PlayerController.Instance.BoostChecking();
            float moveX = GameManager.Instance.WorldSpeed * boostMultiplier * Time.deltaTime;
            transform.position += new Vector3(-moveX, 0);
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return destroyWaitTime;
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}