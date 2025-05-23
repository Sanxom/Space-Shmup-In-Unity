using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class Boom : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Start()
        {
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}