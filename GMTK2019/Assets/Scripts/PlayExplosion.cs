using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayExplosion : MonoBehaviour
{
    public ParticleSystem inicio;
    public ParticleSystem[] circles;

    public float radius;

    readonly float radiusToSpeed = 50f/14f;
    
    public void Play(float radius)
    {
        inicio.Play();
        foreach (ParticleSystem p in circles)
        {
            var main = p.main;
            main.startSpeed = ((radius*16*2)+8)*radiusToSpeed;
            p.Play();
        }
        GameController.Instance.ScreenShake(0.3f,10f);
    }


}
