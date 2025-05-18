using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class ParallaxBackground : MonoBehaviour
    {
        [field: SerializeField] public bool CanScroll { get; private set; } = true;
        [field: SerializeField] public bool CanHorizontalScroll { get; private set; } = true;
        [field: SerializeField] public bool CanVerticalScroll { get; private set; } = false;
        [field: SerializeField] public bool IsScrolling { get; private set; } = false;

        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float moveSpeed;

        private float backgroundImageWidth;
        private float backgroundImageHeight;
        private float moveX;
        private float moveY;

        private void Awake()
        {
            backgroundImageWidth = sr.sprite.texture.width / sr.sprite.pixelsPerUnit;
            backgroundImageHeight = sr.sprite.texture.height / sr.sprite.pixelsPerUnit;
        }

        private void Update()
        {
            if (!CanScroll) return;

            HorizontalParallaxScroll();
            VerticalParallaxScroll();
        }

        private void HorizontalParallaxScroll()
        {
            if (!CanHorizontalScroll) return;

            IsScrolling = true;
            float boostMultiplier = PlayerController.Instance.BoostChecking();

            moveX = boostMultiplier * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(moveX, 0);

            if (Mathf.Abs(transform.position.x) - backgroundImageWidth > 0)
            {
                transform.position = new Vector3(0f, transform.position.y);
            }

            IsScrolling = false;
        }

        private void VerticalParallaxScroll()
        {
            if (!CanVerticalScroll) return;

            IsScrolling = true;
            float boostMultiplier = PlayerController.Instance.BoostChecking();

            moveY = boostMultiplier * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, moveY);
            if (Mathf.Abs(transform.position.y) - backgroundImageHeight > 0)
            {
                transform.position = new Vector3(transform.position.x, 0f);
            }

            IsScrolling = false;
        }
    }
}