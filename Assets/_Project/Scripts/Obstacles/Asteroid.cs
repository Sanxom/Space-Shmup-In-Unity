using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, ICollideable
{
    [field: SerializeField] public int DamageAmount { get; set; }

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;

    public void Collide()
    {
        StartCoroutine(DamageFlashCoroutine());
    }

    private void Awake()
    {
        int randomNumber = UnityEngine.Random.Range(0, sprites.Length);
        sr.sprite = sprites[randomNumber];
        defaultMaterial = sr.material;

        float pushX = UnityEngine.Random.Range(-1f, 0f);
        float pushY = UnityEngine.Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(pushX, pushY);
    }

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

    private IEnumerator DamageFlashCoroutine()
    {
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(0.2f);
        sr.material = defaultMaterial;
    }
}