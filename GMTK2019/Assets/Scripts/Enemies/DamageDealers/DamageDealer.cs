using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public DamageType damageType;

    public float damageAmount;
    public virtual float GetDamage()
    {
        Destroy(gameObject);//También se puede hacer que desaparezca, para optimizar, pero el juego es pequeño, asi que no creo que haya Spikes.
        return damageAmount;
    }
}

