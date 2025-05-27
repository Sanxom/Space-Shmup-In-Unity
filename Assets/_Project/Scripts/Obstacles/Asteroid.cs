using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLabTutorial
{
    public class Asteroid : MonoBehaviour, ICollideable
    {
        [field: SerializeField] public int DamageAmount { get; set; }

        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private GameObject destroyEffect;
        [SerializeField] private int returnToPoolXValue;
        [SerializeField] private int lives;

        private WaitForSeconds damageFlashWaitTime;
        private float damageFlashDuration = 0.2f;

        private void Awake()
        {
            damageFlashWaitTime = new WaitForSeconds(damageFlashDuration);
            SetRandomSprite();
            SetRandomMoveVector();
            SetRandomScale();
        }

        private void Update()
        {
            float boostMultiplier = PlayerController.Instance.BoostChecking();
            float moveX = GameManager.Instance.WorldSpeed * boostMultiplier * Time.deltaTime;
            transform.position += new Vector3(-moveX, 0);

            if (transform.position.x < returnToPoolXValue)
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }

        public void Collide()
        {
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.AsteroidHitSound);
            StartCoroutine(DamageCoroutine());
        }

        private IEnumerator DamageCoroutine()
        {
            lives--;
            sr.material = whiteMaterial;
            yield return damageFlashWaitTime;
            sr.material = defaultMaterial;
            if (lives <= 0)
            {
                ObjectPoolManager.SpawnObject(destroyEffect, transform.position, transform.rotation);
                AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.AsteroidDeathSound);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }

        private void SetRandomSprite()
        {
            int randomNumber = UnityEngine.Random.Range(0, sprites.Length);
            sr.sprite = sprites[randomNumber];
            defaultMaterial = sr.material;
        }

        private void SetRandomMoveVector()
        {
            float pushX = UnityEngine.Random.Range(-1f, 0f);
            float pushY = UnityEngine.Random.Range(-1f, 1f);
            rb.linearVelocity = new Vector2(pushX, pushY);
        }

        private void SetRandomScale()
        {
            float randomScale = UnityEngine.Random.Range(0.6f, 1f);
            transform.localScale = new Vector3(randomScale, randomScale, transform.localScale.z);
        }
    }
}