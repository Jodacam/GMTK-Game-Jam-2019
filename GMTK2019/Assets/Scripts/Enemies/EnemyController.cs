using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    
    public float life = 100;
    Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }
    public virtual void RecibeDamage(float damage){
        life-=damage;
        if(life==0){
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}