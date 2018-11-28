using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    #region Fields

    private int cubeCount;

    public Material material;

    public Camera CameraSource;

    private float duration;

    private Queue<GameObject> spawnedCubes;

    #endregion

    #region Methods

    private void SpawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector2 positionInCirle = Random.insideUnitCircle;
        positionInCirle *= 100;
        cube.transform.position = new Vector3(positionInCirle.x, 100, positionInCirle.y);
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material = this.material;
        Rigidbody rigidBody = cube.AddComponent<Rigidbody>();
        rigidBody.velocity = new Vector3((Random.value - 0.5F) * 10, 0, (Random.value - 0.5F) * 10);
        rigidBody.angularVelocity = new Vector3((Random.value - 0.5F) * Mathf.PI * 10, (Random.value - 0.5F) * Mathf.PI * 10, (Random.value - 0.5F) * Mathf.PI * 10);
        this.cubeCount++;
        this.spawnedCubes.Enqueue(cube);
        if (this.spawnedCubes.Count > 300)
        {
            GameObject spawnedCube = this.spawnedCubes.Dequeue();
            Destroy(spawnedCube);
        }
    }

    private void Start()
    {
        this.spawnedCubes = new Queue<GameObject>();
    }

    private void Update()
    {
        this.duration += Time.deltaTime;
        if (this.duration > 0.1F)
        {
            this.duration = 0;
            this.SpawnCube();
        }
    }

    #endregion
}