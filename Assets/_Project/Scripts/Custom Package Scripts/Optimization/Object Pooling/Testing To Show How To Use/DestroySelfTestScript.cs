using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class DestroySelfTestScript : MonoBehaviour
    {
        [SerializeField] private float timeDelay = 2.5f;

        private WaitForSeconds timeToWait;

        private void Awake()
        {
            timeToWait = new WaitForSeconds(timeDelay);
        }

        private void OnEnable()
        {
            StartCoroutine(Destroy());
        }

        private IEnumerator Destroy()
        {
            yield return timeToWait;

            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}