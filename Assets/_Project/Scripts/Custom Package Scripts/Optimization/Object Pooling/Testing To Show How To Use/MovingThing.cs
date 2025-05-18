using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThing : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 4f;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        PingPong();
    }

    private void PingPong()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPos + new Vector3(x, 0, 0);
    }
}
