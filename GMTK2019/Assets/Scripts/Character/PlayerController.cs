using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Player
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerController>();
            return instance;
        }
    }
    Animator animator;

    [ReorderableList]
    public List<Sound> clips;
    public Vector2 minSpeed = new Vector2(1, 1);

    public float coolDown = 1;

    private float innerCoolDown = 0;

    public float footStep = 0.1f;

    private float innerFoot;
    public Vector2 dir;

    public Vector2 speed;
    public float LAVARIABLE = 100;

    public Weapon actualWeapon;
    public Armor actualArmor;
    public PowerUp actualPowerUp;
    public bool isAttacking;

    public AudioSource audio;

    WaitForSeconds waitForRestart;

    TextMeshProUGUI text;
    public bool dead = false;

    public bool armor = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        waitForRestart = new WaitForSeconds(2f);
        text = GameObject.FindGameObjectWithTag("LAVARIABLE").GetComponent<TextMeshProUGUI>();
        text.text = LAVARIABLE.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            HandleAttack();
            HandleMovement();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            GrabArmor();
        }
    }

    /// <summary>
    /// Aqui se calcula el daño que hacemos dependiendo de la variable.
    /// </summary>
    /// <returns></returns>
    public float GetDamage()
    {
        return LAVARIABLE;
    }

    private void HandleAttack()
    {
        if (innerCoolDown <= 0)
        {
            bool pressed = Input.GetButtonDown("Jump");
            if (pressed)
            {
                innerCoolDown = coolDown;
                animator.SetTrigger("attack");
                actualWeapon.Attack(this);
            }
        }
        else
        {
            innerCoolDown -= Time.deltaTime;
        }
    }

    void HandleMovement()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");


        if (x != 0)
        {
            y = 0;
            dir.x = x;
            dir.y = 0;
        }
        else if (y != 0)
        {
            x = 0;
            dir.y = y;
            dir.x = 0;
        }

        var normalized = new Vector2(x, y).normalized;
        speed = Vector2.Scale(normalized, minSpeed);

        if (speed.magnitude >= 0.1)
        {
            if (innerFoot <= 0)
            {
                PlayClip("footStep");
                innerFoot = footStep;
            }
            else
            {
                innerFoot -= Time.deltaTime;
            }
        }
        transform.Translate(speed * Time.deltaTime);

        animator.SetFloat(Const.X_DIR, dir.x);
        animator.SetFloat(Const.Y_DIR, dir.y);
        animator.SetFloat(Const.SPEED, speed.sqrMagnitude);



    }


     private void OnTriggerEnter2D(Collider2D other)  
    {
        string tag = other.transform.tag;
        if (tag.Equals(Const.DAMAGE_DEALER))
        {
            var e = other.transform.GetComponent<DamageDealer>();
            if (e)
            {
                DamageType type = e.damageType;
                RecibeDamage(e.GetDamage(), type);
            }
        }
    }

    //Aqui se puede calcular el daño que recibimos.
    public void RecibeDamage(float damage, DamageType type)
    {
        text.text = LAVARIABLE.ToString();
        LAVARIABLE -= damage;
        if (LAVARIABLE <= 0 && !dead)
        {
            Die();
        }
    }

    private void Die()
    {
        //animator.SetTrigger(Const.DIE);
        dead = true;
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return waitForRestart;
        LAVARIABLE = 100;
        text.text = LAVARIABLE.ToString();
        dead = false;
        GameController.Instance.Restart();
    }

    public void GrabArmor(){
        if(!armor){
            animator.SetLayerWeight(1,1);
            armor = true;
        }else{
            animator.SetLayerWeight(1,0);
            armor = false;
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
