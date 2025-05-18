using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnerTestScript : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Rigidbody2D rbObjectToSpawn;
    [SerializeField] private Transform parentObjectToSpawnOn;
    [SerializeField] private bool spawnOnParentObject;

    private void Update()
    {
        if (spawnOnParentObject)
            SpawnObjectOnParentObject();
        else
            SpawnObjectWithMousePosition();
    }

    /// <summary>
    /// Dummy function to test spawning every frame while left Mouse is held down. 
    /// Shows how to use SpawnObject function at a Vector3 position.
    /// </summary>
    private void SpawnObjectWithMousePosition()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            ObjectPoolManager.SpawnObject(objectToSpawn, worldPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Dummy function to test spawning every frame while left Mouse is held down. 
    /// Shows how to use SpawnObject function to spawn objects under a specific parent instead of a Vector3 position.
    /// </summary>
    private void SpawnObjectOnParentObject()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Rigidbody2D rb = ObjectPoolManager.SpawnObject(rbObjectToSpawn, parentObjectToSpawnOn, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);
            rb.gravityScale = 1f;
        }
    }
}
