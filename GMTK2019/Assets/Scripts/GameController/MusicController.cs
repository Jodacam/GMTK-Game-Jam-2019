﻿using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update


    private static MusicController instance;
    public static MusicController Instance {get{
        if(instance){
            instance = FindObjectOfType<MusicController>();
        }
        return instance;
    }}
 
    [ReorderableList]
    public List<Sound> music;
    public AudioSource audio;
    
    string actual = "";
    void Awake()
    {
        if(instance){
            Destroy(gameObject);
        }else{
            instance=this;
            audio = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame

    public void SetMusic(string name){
        var clip = music.Find((e) => e.name.Equals(name));
        if (clip != null && !actual.Equals(name))
        {
            actual = name;
            audio.Stop();

            audio.clip=clip.clip;

            audio.Play();
        }
    }
}
