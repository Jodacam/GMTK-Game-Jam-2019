using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyController : MonoBehaviour {
    

    public enum State
    {
        Moving,
        Attacking,

        Exploding,

        Stacionary

        
    }
    public float life = 100;

    public DamageType type;
    public Animator animator;

    public float coolDown = 1;

    public float onDamageTime;

    public float innerCoolDown = 0;

    public float timeToDie = 0.2f;

    [ReorderableList]
    public List<Sound> clips;
    public AudioSource audio;

    private float time = 0;
    private float timeToPlay;

    public bool stacionary;

    [ReadOnly]
    protected Vector2 dir = new Vector2(0,0);
    public State state;

    public ParticleSystem deadEffect;

    public ParticleSystem hitEffect;

    public void Start() {
        dir = new Vector2(0,-1);

        timeToPlay = UnityEngine.Random.Range(3.0f, 6.0f);
        animator = GetComponent<Animator>();
        animator.SetFloat(Const.X_DIR, dir.x);
        animator.SetFloat(Const.Y_DIR, dir.y);
        audio = GetComponent<AudioSource>();

    }

    public void Update(){
        time += Time.deltaTime;

        if (time >= timeToPlay)
        {
            time = 0.0f;
            timeToPlay = UnityEngine.Random.Range(5.0f, 7.0f);
            PlayClip("idle");
        }
    }

    public virtual void RecibeDamage(float damage,Vector2 dir){
        life-=damage;
        
        if(onDamageTime<=0)
            StartCoroutine(Damaged());
        onDamageTime = 0.5f;
        
        if(life<=0){
            Die();
        }
    }

    public virtual void Die()
    {
        GameController.Instance.EnemyDead();
        PlayClip("death");       
        int r = UnityEngine.Random.Range(0,GameController.Instance.dropeables.Count);
        Instantiate(deadEffect,transform.position,Quaternion.Euler(0,0,0));
        Destroy(gameObject,timeToDie);
        Instantiate(GameController.Instance.dropeables[r],transform.position,Quaternion.identity);
    }

    public abstract void Attack();

    public  IEnumerator Damaged(){
        hitEffect.Play();
        yield return null;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color c = renderer.color;
        while(onDamageTime>=0){
            renderer.color = Color.Lerp(Color.white,Color.red,Mathf.PingPong(Time.time,1f));
            yield return null;
            onDamageTime-=Time.deltaTime;

        }
        renderer.color =c;//Color.white;
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