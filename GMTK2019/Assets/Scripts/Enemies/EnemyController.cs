using System;
using UnityEngine;
using NaughtyAttributes;
public abstract class EnemyController : MonoBehaviour {
    

    public enum State
    {
        Moving,
        Attacking,

        
    }
    public float life = 100;

    public DamageType type;
    public Animator animator;

    public float coolDown = 1;

    public float innerCoolDown = 0;

    [ReadOnly]
    protected Vector2 dir = new Vector2(0,0);
    public State state;
    public void Start() {
        animator = GetComponent<Animator>();
    }
    public virtual void RecibeDamage(float damage){
        life-=damage;
        if(life<=0){
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public abstract void Attack();
}