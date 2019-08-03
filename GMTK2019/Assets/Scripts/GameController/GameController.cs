﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> totalMapList;

    public List<GameObject> mapList;

    public GameObject bossRoom;

    public int actualMap;

    public GameObject Shop;
    public int mapsPerShop;

    public int maxMaps;

    private Random r;

    private ScreenShake camShake;

    public PlayerController playerPrefab;

    public AstarPath aStarController;
    public static GameController Instance;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        camShake = FindObjectOfType<ScreenShake>();
    }

    void Start()
    {
        for (int i = 0; i <maxMaps ; i++)
        {   
            if(i%mapsPerShop == 0 && i!=0){
                mapList.Add(Instantiate(Shop));
            }else{
                int randomNumber = Random.Range(0,totalMapList.Count-1);
                mapList.Add(Instantiate(totalMapList[randomNumber],transform.position,Quaternion.identity));
                
            }
            mapList[i].SetActive(false);
            
        }
        mapList.Add(bossRoom);
        mapList[0].SetActive(true);
        aStarController.Scan();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextMap(){
        mapList[actualMap].SetActive(false);
        actualMap++;
        if(actualMap < mapList.Count)
            mapList[actualMap].SetActive(true);
    }

    public void ScreenShake(float duration, float strength){
        camShake.StartShake(duration,strength);
    }
}