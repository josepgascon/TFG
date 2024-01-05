using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackground : MonoBehaviour
{
    public GameObject groundPrefab; // Reference to the ground prefab that you want to spawn
    public GameObject minePrefab; // Reference to the mine prefab that you want to spawn
    private float spawnDistance = 10f; // Distance at which the next ground object will be spawned
    private bool create_background = true;
    private bool create_mines = true;
    private float timer = 0f;
    private float spawnTime = 2f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (create_background) StartCoroutine(SpawnGround());
        if (create_mines) StartCoroutine(SpawnMines());

    }

    IEnumerator SpawnGround()
    {
        create_background = false;
        spawnDistance = timer * Main.speed;
        GameObject bottom_background = Instantiate(groundPrefab, new Vector3(spawnDistance+150, -7f, 0), Quaternion.identity);
        GameObject top_background = Instantiate(groundPrefab, new Vector3(spawnDistance+154, 6.88f, 0), Quaternion.Euler(180, 0, 0));

        yield return new WaitForSeconds(35f); // every 35 seconds
        create_background = true;
    }

    IEnumerator SpawnMines()
    {
        create_mines = false;
        spawnDistance = timer * 3f;
       // GameObject bottom_background = Instantiate(groundPrefab, new Vector3(spawnDistance + 150, -7f, 0), Quaternion.identity);
       // GameObject top_background = Instantiate(groundPrefab, new Vector3(spawnDistance + 154, 6.88f, 0), Quaternion.Euler(180, 0, 0));

        yield return new WaitForSeconds(spawnTime); // every x seconds
        create_mines = true;
    }
}
