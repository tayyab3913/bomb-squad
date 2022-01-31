using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float leftRightBounds;
    public float upDownBounds;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefabs;
    private GameObject playerReference;
    public List<GameObject> enemiesAlive;
    private int waveNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        InstantiatePlayer();
        InvokeRepeating("InstantiateRandomPowerUp", 1, 3);
        InstantiateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive.ToArray().Length < 1)
        {
            waveNumber++;
            SpawnEnemyWave();
        }
    }

    Vector3 GetRandomPosition()
    {
        float locationX =  Random.Range(-leftRightBounds, leftRightBounds);
        float locationZ = Random.Range(-upDownBounds, upDownBounds);
        return new Vector3(locationX, 0.5f, locationZ);
    }

    void InstantiateEnemy()
    {
        GameObject tempGameObject = Instantiate(enemyPrefab, GetRandomPosition(), enemyPrefab.transform.rotation);
        Enemy tempEnemy = tempGameObject.GetComponent<Enemy>();
        tempEnemy.GetGameManager(this);
        tempEnemy.SetPlayerReference(playerReference);
        enemiesAlive.Add(tempGameObject);
    }

    void InstantiatePlayer()
    {
        playerReference = Instantiate(playerPrefab, GetRandomPosition(), playerPrefab.transform.rotation);
        playerReference.GetComponent<PlayerController>().GetGameManager(this);
    }

    void SpawnEnemyWave()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            InstantiateEnemy();
        }
    }

    public void RemoveEnemyFromEnemiesAlive(GameObject enemytoRemove)
    {
        GameObject tempMarker = null;
        foreach (GameObject member in enemiesAlive)
        {
            if (enemytoRemove == member)
            {
                tempMarker = enemytoRemove;
            }
        }
        enemiesAlive.Remove(tempMarker);
    }

    void InstantiateRandomPowerUp()
    {
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomIndex], GetRandomPosition(), powerUpPrefabs[randomIndex].transform.rotation);
    }
}
