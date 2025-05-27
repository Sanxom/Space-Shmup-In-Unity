using CodeLabTutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Critter1 : MonoBehaviour, ICollideable
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject zappedEffect;
    [SerializeField] private GameObject burnedEffect;
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float minMoveInterval;
    [SerializeField] private float maxMoveInterval;
    [SerializeField] private float minMoveDistance;
    [SerializeField] private float maxMoveDistance;
    [SerializeField] private float returnToPoolXValue;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float moveSpeed;
    private float rotationSpeed;
    private float moveTimer;
    private float moveInterval;

    private const int ZERO_INT = 0;

    public int DamageAmount { get; set; }

    private void Awake()
    {
        rotationSpeed = 1080f;
    }

    private void OnEnable()
    {
        sr.sprite = sprites[UnityEngine.Random.Range(ZERO_INT, sprites.Length)];
        moveSpeed = UnityEngine.Random.Range(minMoveSpeed, maxMoveSpeed);
        GenerateRandomPosition();
        moveInterval = UnityEngine.Random.Range(minMoveInterval, maxMoveInterval);
        moveTimer = moveInterval;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        MoveLeftWithGameWorld();
        CheckReturnToPoolXValue();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer _))
        {
            HandleBurnCollisionDeath();
        }
    }

    public void Collide()
    {
        if (gameObject.activeSelf)
            StartCoroutine(ZapCollideCoroutine());
    }

    private IEnumerator ZapCollideCoroutine()
    {
        HandleZapCollisionDeath();

        yield return null;
    }

    private void HandleBurnCollisionDeath()
    {
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.CritterBurnDeathSound);
        ObjectPoolManager.SpawnObject(burnedEffect, transform.position, transform.rotation);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void HandleZapCollisionDeath()
    {
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.CritterZapDeathSound);
        ObjectPoolManager.SpawnObject(zappedEffect, transform.position, transform.rotation);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void GenerateRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(minMoveDistance, maxMoveDistance);
        float randomY = UnityEngine.Random.Range(minMoveDistance, maxMoveDistance);
        targetPosition = new Vector2(randomX, randomY);
    }
    
    private void MoveLeftWithGameWorld()
    {
        float moveX = GameManager.Instance.WorldSpeed * PlayerController.Instance.BoostSpeed * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
    }

    private void HandleRotation()
    {
        Vector3 relativePos = targetPosition - transform.position;
        if (relativePos != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        if (moveTimer > 0)
            moveTimer -= Time.deltaTime;
        else
        {
            GenerateRandomPosition();
            moveInterval = UnityEngine.Random.Range(minMoveInterval, maxMoveInterval);
            moveTimer = moveInterval;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void CheckReturnToPoolXValue()
    {
        if (transform.position.x < returnToPoolXValue)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}