using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>, IPlayer
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float superBoostSpeed;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    private GameInput gameInput;
    private InputAction boostAction;
    private InputAction superBoostAction;
    private InputAction pauseAction;
    private Vector2 moveVector;
    private float moveX;
    private float moveY;
    private bool isBoosting;
    private bool isSuperBoosting;

    private const string MOVE_X_ANIMATOR = "moveX";
    private const string MOVE_Y_ANIMATOR = "moveY";
    private const string IS_BOOSTING_ANIMATOR = "isBoosting";
    private const string IS_SUPER_BOOSTING_ANIMATOR = "isSuperBoosting";

    public float BoostSpeed { get => boostSpeed; private set => boostSpeed = value; }
    public float SuperBoostSpeed { get => superBoostSpeed; private set => superBoostSpeed = value; }
    public float DefaultMoveSpeed { get => defaultMoveSpeed; private set => defaultMoveSpeed = value; }
    public bool IsBoosting { get => isBoosting; private set => isBoosting = value; }
    public bool IsSuperBoosting { get => isSuperBoosting; private set => isSuperBoosting = value; }

    #region Unity Callback Functions
    private void Start()
    {
        energy = maxEnergy;
        health = maxHealth;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        defaultMaterial = sr.material;
    }

    private void OnEnable()
    {
        currentMoveSpeed = defaultMoveSpeed;
        gameInput = new GameInput();
        gameInput.Enable();
        boostAction = gameInput.Player.Boost;
        superBoostAction = gameInput.Player.SuperBoost;
        pauseAction = gameInput.Player.Pause;
        boostAction.performed += BoostAction_performed;
        boostAction.canceled += BoostAction_canceled;
        superBoostAction.performed += SuperBoostAction_performed;
        superBoostAction.canceled += SuperBoostAction_canceled;
        pauseAction.performed += PauseAction_performed;
    }

    private void PauseAction_performed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.Pause();
    }

    private void OnDisable()
    {
        boostAction.performed -= BoostAction_performed;
        boostAction.canceled -= BoostAction_canceled;
        superBoostAction.performed -= SuperBoostAction_performed;
        superBoostAction.canceled -= SuperBoostAction_canceled;
        pauseAction.performed -= PauseAction_performed;
        gameInput.Disable();
    }

    private void Update()
    {
        ReadMoveInput();
        AnimatePlayerBasedOnInput();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = currentMoveSpeed * new Vector2(moveX, moveY);

        EnergySystemBoostUsage();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollideable collideable))
        {
            collideable.Collide();
            TakeDamage(collideable.DamageAmount);
        }
    }
    #endregion

    #region Input Callbacks
    private void BoostAction_canceled(InputAction.CallbackContext obj)
    {
        ExitBoost();
    }

    private void BoostAction_performed(InputAction.CallbackContext obj)
    {
        EnterBoost();
    }

    private void SuperBoostAction_canceled(InputAction.CallbackContext obj)
    {
        ExitSuperBoost();
    }

    private void SuperBoostAction_performed(InputAction.CallbackContext obj)
    {
        EnterSuperBoost();
    }
    #endregion

    private void ReadMoveInput()
    {
        if (!isBoosting && !isSuperBoosting)
            currentMoveSpeed = defaultMoveSpeed;

        moveVector = gameInput.Player.Move.ReadValue<Vector2>();
        moveX = moveVector.x;
        moveY = moveVector.y;
    }

    private void AnimatePlayerBasedOnInput()
    {
        animator.SetFloat(MOVE_X_ANIMATOR, moveX);
        animator.SetFloat(MOVE_Y_ANIMATOR, moveY);
        animator.SetBool(IS_BOOSTING_ANIMATOR, isBoosting);
        animator.SetBool(IS_SUPER_BOOSTING_ANIMATOR, isSuperBoosting);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        AudioManager.Instance.PlaySound(AudioManager.Instance.Hit);
        StartCoroutine(DamageFlashCoroutine());

        if (health <= 0)
        {
            HandleGameOver();
        }
    }

    private IEnumerator DamageFlashCoroutine()
    {
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(0.2f);
        sr.material = defaultMaterial;
    }

    private void HandleGameOver()
    {
        ResetMoveVariables();
        Instantiate(destroyEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
        AudioManager.Instance.PlaySound(AudioManager.Instance.DeathSound);
    }

    private void ResetMoveVariables()
    {
        currentMoveSpeed = 0f;
        isBoosting = false;
        isSuperBoosting = false;
    }

    public void DisablePlayerInput()
    {
        gameInput.Player.Move.Disable();
        gameInput.Player.Boost.Disable();
        gameInput.Player.SuperBoost.Disable();
        gameInput.Player.Fire.Disable();
    }

    public void EnablePlayerInput()
    {
        gameInput.Player.Move.Enable();
        gameInput.Player.Boost.Enable();
        gameInput.Player.SuperBoost.Enable();
        gameInput.Player.Fire.Enable();
    }

    #region Boost System
    public float BoostChecking()
    {
        if ((IsSuperBoosting && !IsBoosting) ||
            (IsSuperBoosting && IsBoosting))
            return superBoostSpeed;
        else if (IsBoosting && !IsSuperBoosting)
            return boostSpeed;
        else
            return 1f;
    }

    private void EnergySystemBoostUsage()
    {
        if (isBoosting)
        {
            if (energy >= 0.2f)
                energy -= 0.2f;
            else
                ExitBoost();
        }
        else if (isSuperBoosting)
        {
            if (energy >= 0.4f)
                energy -= 0.4f;
            else
                ExitSuperBoost();
        }
        else
        {
            if (energy < maxEnergy)
            {
                energy += energyRegen;
            }
        }

        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }

    private void EnterBoost()
    {
        if (energy <= 10) return;

        isBoosting = true;
        AudioManager.Instance.PlaySound(AudioManager.Instance.Fire);

        if (moveVector == Vector2.zero)
        {
            currentMoveSpeed = boostSpeed;
            return;
        }

        if (isSuperBoosting)
        {
            currentMoveSpeed = superBoostSpeed;
        }
        else
            currentMoveSpeed = boostSpeed;
    }

    private void ExitBoost()
    {
        isBoosting = false;
        if (isSuperBoosting)
            currentMoveSpeed = superBoostSpeed;
        else
            currentMoveSpeed = defaultMoveSpeed;
    }

    private void EnterSuperBoost()
    {
        if (energy <= 30) return;

        isSuperBoosting = true;
        AudioManager.Instance.PlaySound(AudioManager.Instance.Fire);

        if (moveVector == Vector2.zero)
        {
            currentMoveSpeed = SuperBoostSpeed;
            return;
        }

        currentMoveSpeed = superBoostSpeed;
    }

    private void ExitSuperBoost()
    {
        isSuperBoosting = false;
        if (isBoosting)
            currentMoveSpeed = boostSpeed;
        else
            currentMoveSpeed = defaultMoveSpeed;
    }
    #endregion
}