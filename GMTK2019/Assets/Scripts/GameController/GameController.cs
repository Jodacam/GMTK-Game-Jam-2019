﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public List<GameObject> totalMapList;

    List<GameObject> mapList = new List<GameObject>();

    public List<GameObject> dropeables;
    public List<PowerUp> powerUpList;
    public List<Weapon> weaponList;
    public List<Armor> armorList;
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
    public List<EnemySet> setOfBosses;

    public Puerta door;
    public Transform spawnPoint;

    public ScrollUi scroll;

    private bool transitioning = false;
    private float transitionTimeStamp;
    private PostProcessProfile postProcessingProfile;

    float totalMaps = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        camShake = FindObjectOfType<ScreenShake>();
    }

    private void Update() {
        if(Input.GetKey(KeyCode.Escape)){
            MusicController.Instance.SetMusic("main");
            SceneManager.LoadScene(0);
        }
    }
    void Start()
    {
        postProcessingProfile = FindObjectOfType<PostProcessVolume>().profile;
        MusicController.Instance.SetMusic("Game");
        mapList.Add(Instantiate(tutorialRoom, transform.position, Quaternion.identity));
        PrepareMap();
        door = FindObjectOfType<Puerta>();
        door.SetEnemies(0);
    }

    public void Restart()
    {
        foreach (GameObject g in mapList)
        {
            Destroy(g);
        }
        List<EnemyController> enemies = FindObjectsOfType<EnemyController>().ToList();
        foreach (EnemyController e in enemies)
        {
            Destroy(e.gameObject);
        }
        List<Projectile> proyectiles = FindObjectsOfType<Projectile>().ToList();
        foreach (Projectile e in proyectiles)
        {
            Destroy(e.gameObject);
        }
        List<Coin> dropeables = FindObjectsOfType<Coin>().ToList();
        foreach (Coin e in dropeables)
        {
            Destroy(e.gameObject);
        }
        scroll.generated = false;
        scroll.Wipe();
        mapList.Clear();
        actualMap = 0;
        totalMaps = 0;
        mapList.Add(Instantiate(tutorialRoom, transform.position, Quaternion.identity));
        PrepareMap();
        door = FindObjectOfType<Puerta>();
        door.SetEnemies(0);
        MusicController.Instance.SetMusic("Game");
    }

    public void GenerateMapList()
    {
        foreach (GameObject g in mapList)
        {
            Destroy(g);
        }
        mapList.Clear();
        actualMap = 0;
        for (int i = 0; i < maxMaps - 1; i++)
        {
            if (i % mapsPerShop == 0 && i != 0)
            {
                int n = Random.Range(0, shops.Count);
                mapList.Add(Instantiate(shops[n], transform.position, Quaternion.identity));
                scroll.NextTile(ScrollUi.RoomType.Shop);
            }
            else
            {
                int randomNumber = Random.Range(0, totalMapList.Count - 1);
                mapList.Add(Instantiate(totalMapList[randomNumber], transform.position, Quaternion.identity));
                scroll.NextTile(ScrollUi.RoomType.Normal);
            }
            mapList[i].SetActive(false);

        }
        if(totalMaps>=2 && totalMaps < 21)
            PlayerController.Player.addCurse();
        int r = Random.Range(0, shops.Count);
        mapList.Add(Instantiate(bossRooms[r], transform.position, Quaternion.identity));
        scroll.NextTile(ScrollUi.RoomType.Boss);
        scroll.generated = true;
        mapList[mapList.Count - 1].SetActive(false);
        PrepareMap();

    }

    public void PrepareMap()
    {
        mapList[actualMap].SetActive(true);
        aStarController.Scan();
        spawnPoint = FindObjectOfType<SpawnPoint>().transform;
        PlayerController.Player.transform.position = spawnPoint.position;
        PlayerController.Player.SpawnPlayer();
    }

    public System.Collections.IEnumerator NextMap()
    {
        if (transitioning == false)
        {
            List<Projectile> proyectiles = FindObjectsOfType<Projectile>().ToList();
            foreach (Projectile e in proyectiles)
            {
                Destroy(e.gameObject);
            }
            List<Coin> dropeables = FindObjectsOfType<Coin>().ToList();
            foreach (Coin e in dropeables)
            {
                Destroy(e.gameObject);
            }
            transitioning = true;
            transitionTimeStamp = Time.time;
            FindObjectOfType<Camera>().GetComponent<PostProcessLayer>().enabled = true;
            postProcessingProfile.GetSetting<DepthOfField>().focusDistance.value = 50;
            postProcessingProfile.GetSetting<ChromaticAberration>().intensity.value = 0;
        }
        while (Time.time - transitionTimeStamp <= 0.5f)
        {
            postProcessingProfile.GetSetting<DepthOfField>().focusDistance.value = Mathf.Lerp(50, 0.1f, (Time.time - transitionTimeStamp) / 0.5f);
            postProcessingProfile.GetSetting<ChromaticAberration>().intensity.value = Mathf.Lerp(0, 1, (Time.time - transitionTimeStamp) / 0.5f);
            yield return null;
        }
        transitioning = false;
        StartCoroutine(PlayerController.Player.StopMove());

        mapList[actualMap].SetActive(false);
        actualMap++;
        totalMaps++;

        if (mapList.Count > 1)
        {
            scroll.Scroll();
        }

        if (actualMap < mapList.Count)
        {
            PrepareMap();
        }
        else
        {
            GenerateMapList();
        }


        SpawnEnemies();
        yield return null;

    }

    public void ScreenShake(float duration, float strength)
    {
        camShake.StartShake(duration, strength);
    }

    private Weapon getWeapon()
    {
        Weapon n = null;
        while (n == null || n == PlayerController.Player.actualWeapon)
        {
            int r = Random.Range(0, weaponList.Count - 1);
            n = weaponList[r];
        }
        return n;
    }

    private PowerUp getPowerUp()
    {
        PowerUp n = null;
        while (n == null || n == PlayerController.Player.actualPowerUp)
        {
            int r = Random.Range(0, powerUpList.Count - 1);
            n = powerUpList[r];
        }
        return n;
    }

    private Armor getArmor()
    {
        Armor n = null;
        while (n == null || n == PlayerController.Player.actualArmor)
        {
            int r = Random.Range(0, armorList.Count - 1);
            n = armorList[r];
        }
        return n;
    }
    public void SpawnEnemies()
    {
        door = FindObjectOfType<Puerta>();
        if (actualMap < mapList.Count - 1)
        {
            if (actualMap % mapsPerShop == 0 && actualMap != 0)
            {
                door.SetEnemies(0);
                MusicController.Instance.SetMusic("Shop");
                var shop = FindObjectsOfType<ShopItem>();
                var w = getWeapon();
                var a = getArmor();
                var w1 = getWeapon();
                float disc = 1;
                if(PlayerController.Player.bendiciones["coins"]){
                    disc =Mathf.Lerp(0.95f,0.25f,PlayerController.Player.getLimit());
                }
                shop[0].Init(w, Mathf.RoundToInt(Mathf.LerpUnclamped(w.cost, 240, (PlayerController.Player.currentLAVARIABLE - 40) / 300) * disc));
                shop[1].Init(a, Mathf.RoundToInt(Mathf.LerpUnclamped(a.cost, 240, (PlayerController.Player.currentLAVARIABLE - 40) / 300) * disc));
                shop[2].Init(w1, Mathf.RoundToInt(Mathf.LerpUnclamped(w1.cost, 240, (PlayerController.Player.currentLAVARIABLE - 40) / 300) * disc));
            }
            else
            {
                MusicController.Instance.SetMusic("Game");
                List<SpawnEnemy> spawns = FindObjectsOfType<SpawnEnemy>().ToList();
                List<EnemySet> setAllowed = (from x in setOfEnemies where x.roomToStart <= totalMaps && x.roomToStop >= totalMaps select x).ToList();
                spawns.Shuffle();
                int random = Random.Range(0, setAllowed.Count);

                int enemyNum = 0;

                foreach (EnemyController enemy in setAllowed[random].enemies)
                {
                    var e = Instantiate(enemy, spawns[enemyNum].transform.position, Quaternion.Euler(0, 0, 0));
                    e.life = e.life + Mathf.Log10((actualMap + 1) * 10) * 15;
                    enemyNum++;
                }

                door.SetEnemies(enemyNum);
            }
        }
        else
        {
            MusicController.Instance.SetMusic("Boss");
            List<SpawnEnemy> spawns = FindObjectsOfType<SpawnEnemy>().ToList();
            List<EnemySet> setAllowed = (from x in setOfBosses where x.roomToStart <= totalMaps && x.roomToStop >= totalMaps select x).ToList();
            spawns.Shuffle();
            int random = Random.Range(0, setAllowed.Count);

            int enemyNum = 0;

            foreach (EnemyController enemy in setOfBosses[random].enemies)
            {

                Instantiate(enemy, spawns[enemyNum].transform.position, Quaternion.Euler(0, 0, 0));
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
