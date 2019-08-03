using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public abstract class EnemyController : MonoBehaviour {
    

    public enum State
    {
        Moving,
        Attacking,

        Exploding

        
    }
    public float life = 100;

    public DamageType type;
    public Animator animator;

    public float coolDown = 1;

    public float onDamageTime;

    public float innerCoolDown = 0;
    
    
    public bool stacionary;

    [ReadOnly]
    protected Vector2 dir = new Vector2(0,0);
    public State state;
    public void Start() {
        animator = GetComponent<Animator>();
    }
    public virtual void RecibeDamage(float damage,Vector2 dir){
        life-=damage;
        if(!stacionary)
            transform.Translate(dir*16);
        
        if(onDamageTime<=0)
            StartCoroutine(Damaged());
        onDamageTime = 0.5f;
        
        if(life<=0){
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public abstract void Attack();

    public  IEnumerator Damaged(){
        yield return null;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        while(onDamageTime>=0){
            renderer.color = Color.Lerp(Color.white,Color.red,Mathf.PingPong(Time.time,1f));
            yield return null;
            onDamageTime-=Time.deltaTime;

        }
        renderer.color =Color.white;
    }
}