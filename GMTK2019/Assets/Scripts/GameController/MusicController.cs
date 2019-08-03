using System.Collections;
using System.Collections.Generic;
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
 
    public List<Sound> music;
    public AudioSource audio;
    
    void Start()
    {
        if(instance){
            Destroy(gameObject);
        }else{
            instance=this;
            audio = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMusic(string name){
        
    }
}
