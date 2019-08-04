using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public List<Sound> clips;

    public AudioSource audio;

    public DamageType damageType;

    public float damageAmount;
    public virtual float GetDamage()
    {
        PlayClip("Hit");
        Destroy(gameObject,0.5f);//También se puede hacer que desaparezca, para optimizar, pero el juego es pequeño, asi que no creo que haya Spikes.
        return damageAmount;
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

