using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SpawnBackground : MonoBehaviour
{
    public GameObject groundPrefab; // Reference to the ground prefab that you want to spawn
    public GameObject minePrefab; // Reference to the mine prefab that you want to spawn
    private float spawnDistance = 10f; // Distance at which the next ground object will be spawned
    public GameObject player;
    private bool create_background = true;
    private bool create_mines = true;

    public float spawnTime = 1.2f;
    public float minX = 5;
    public float maxX = 7;
    public float minY = -5;
    public float maxY = 5;

    private void Start()
    {
        spawnTime = 1.2f;

        //creem primer unes les mines del principi
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Instantiate(minePrefab, new Vector3(spawnDistance + randomX + 10f, randomY, 0), Quaternion.identity);
        randomX = Random.Range(minX, maxX);
        randomY = Random.Range(minY, maxY);
        Instantiate(minePrefab, new Vector3(spawnDistance + randomX + 15f, randomY, 0), Quaternion.identity);
        randomX = Random.Range(minX, maxX);
        randomY = Random.Range(minY, maxY);
        Instantiate(minePrefab, new Vector3(spawnDistance + randomX + 20f, randomY, 0), Quaternion.identity);

        //creem el terra i el sostre de la cova del principi
        Instantiate(groundPrefab, new Vector3(spawnDistance + 150f, -7f, 0), Quaternion.identity);
        Instantiate(groundPrefab, new Vector3(spawnDistance + 154f, 6.88f, 0), Quaternion.Euler(180, 0, 0));
    }

    private void Update()
    {
        if (create_background) StartCoroutine(SpawnGround());
        if (create_mines) StartCoroutine(SpawnMines());
        spawnDistance = player.transform.position.x;
    }

    IEnumerator SpawnGround()
    {
        create_background = false;
        Instantiate(groundPrefab, new Vector3(spawnDistance + 300f, -7f, 0), Quaternion.identity);
        Instantiate(groundPrefab, new Vector3(spawnDistance + 304f, 6.88f, 0), Quaternion.Euler(180, 0, 0));

        yield return new WaitForSeconds(30f); // cada 30 segons
        create_background = true;
    }

    IEnumerator SpawnMines()
    {
        create_mines = false;
        spawnTime -= 0.003f;
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Instantiate(minePrefab, new Vector3(spawnDistance + randomX + 25f, randomY, 0), Quaternion.identity);

        yield return new WaitForSeconds(spawnTime); // every x seconds
        create_mines = true;
    }
}
