using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHP : MonoBehaviour
{

    [ReorderableList]
    public List<Sound> clips;

    public AudioSource audio;
    bool playing;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        playing = false;
        InvokeRepeating("checkAudio", 1.0f, 0.5f);
    }
    void checkAudio()
    {
        if (PlayerController.Player.currentLAVARIABLE < 40 && !audio.isPlaying)
        {
            PlayClip("lowhp");
        }
        else
        {
            playing = false;
            audio.Stop();
        }
    }


    public void PlayClip(string name)
    {
        var clip = clips.Find((e) => e.name.Equals(name));
        if (clip != null)
        {
            audio.PlayOneShot(clip.clip);
        }
    }
}
