using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> totalMapList;

    List<GameObject> mapList = new List<GameObject>();

    public List<GameObject> bossRooms;

    public int actualMap;

    public List<GameObject> shops;
    public GameObject tutorialRoom;
    public int mapsPerShop;

    public int maxMaps;

    private ScreenShake camShake;

    public PlayerController playerPrefab;

    public AstarPath aStarController;
    public static GameController Instance;

    public List<EnemySet> setOfEnemies;

    public Puerta door;
    public Transform spawnPoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        camShake = FindObjectOfType<ScreenShake>();
    }

    void Start()
    {
        mapList.Add(Instantiate(tutorialRoom, transform.position, Quaternion.identity));
        PrepareMap();
        door = FindObjectOfType<Puerta>();
        door.SetEnemies(0);
    }

    void Restart()
    {
        mapList.Clear();
        actualMap = 0;
        mapList.Add(Instantiate(tutorialRoom, transform.position, Quaternion.identity));
        PrepareMap();
        door = FindObjectOfType<Puerta>();
        door.SetEnemies(0);
    }

    public void GenerateMapList()
    {
        mapList.Clear();
        actualMap = 0;
        for (int i = 0; i < maxMaps - 1; i++)
        {
            if (i % mapsPerShop == 0 && i != 0)
            {
                int n = Random.Range(0, shops.Count);
                mapList.Add(Instantiate(shops[n], transform.position, Quaternion.identity));
            }
            else
            {
                int randomNumber = Random.Range(0, totalMapList.Count - 1);
                mapList.Add(Instantiate(totalMapList[randomNumber], transform.position, Quaternion.identity));
            }
            mapList[i].SetActive(false);

        }
        int r = Random.Range(0, shops.Count);
        mapList.Add(Instantiate(bossRooms[r], transform.position, Quaternion.identity));
        mapList[mapList.Count - 1].SetActive(false);
        PrepareMap();

    }

    public void PrepareMap()
    {
        mapList[actualMap].SetActive(true);
        aStarController.Scan();
        spawnPoint = FindObjectOfType<SpawnPoint>().transform;
        PlayerController.Player.transform.position = spawnPoint.position;
    }

    public void NextMap()
    {
        mapList[actualMap].SetActive(false);
        actualMap++;

        if (actualMap < mapList.Count)
        {
            PrepareMap();
        }
        else
        {
            GenerateMapList();
        }

        SpawnEnemies();
    }

    public void ScreenShake(float duration, float strength)
    {
        camShake.StartShake(duration, strength);
    }

    public void SpawnEnemies()
    {
        door = FindObjectOfType<Puerta>();
        if (actualMap < mapList.Count)
        {
            if (actualMap % mapsPerShop == 0 && actualMap != 0)
            {
                door.SetEnemies(0);
            }
            else
            {
                int random = Random.Range(0, setOfEnemies.Count);

                int enemyNum = 0;

                foreach (EnemyController enemy in setOfEnemies[random].enemies)
                {
                    Instantiate(enemy, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    enemyNum++;
                }

                door.SetEnemies(enemyNum);
            }
        }
        else
        {
            int random = Random.Range(0, setOfEnemies.Count);

            int enemyNum = 0;

            foreach (EnemyController enemy in setOfEnemies[random].enemies)
            {
                Instantiate(enemy, Vector3.zero, Quaternion.Euler(0, 0, 0));
                enemyNum++;
            }

            door.SetEnemies(enemyNum);
        }
    }

    public void EnemyDead()
    {
        door.EnemyKilled();
    }
}
