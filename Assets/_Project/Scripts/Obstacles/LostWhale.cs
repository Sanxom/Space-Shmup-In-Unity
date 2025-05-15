using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostWhale : MonoBehaviour
{
    private const string LEVEL_1_COMPLETE_SCENE_NAME = "Level 1 Complete";

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer player))
        {
            SceneManager.LoadScene(LEVEL_1_COMPLETE_SCENE_NAME);
        }
    }
}