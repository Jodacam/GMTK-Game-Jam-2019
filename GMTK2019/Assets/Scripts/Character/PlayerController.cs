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

    internal void ActualWeapon(Weapon weapon)
    {
        animator.SetFloat("weapon",weapon.id);
        actualWeapon = weapon;
    }

    public float coolDown = 1;

    private float innerCoolDown = 0;

    private bool invencible = false;

    public float footStep = 0.1f;

    private float innerFoot;
    public Vector2 dir;

    public Vector2 speed;
    public float LAVARIABLE = 40;
    public float currentLAVARIABLE;

    public Weapon actualWeapon;
    public Armor actualArmor;
    public PowerUp actualPowerUp;
    public bool isAttacking;

    public AudioSource audio;

    WaitForSeconds waitForRestart;

    TextMeshProUGUI text;
    public bool dead = false;

    public bool armor = false;

    public ParticleSystem getCoins;
    public ParticleSystem getPocionMana;
    public ParticleSystem getBread;
    public ParticleSystem getPocionSalud;
    public ParticleSystem getChicken;
    Dictionary<Coin.Type,ParticleSystem> getParticles;

    public ParticleSystem getHit;

    public ParticleSystem loseCoins;

    public PlayExplosion explosion;

    void Start()
    {
        currentLAVARIABLE = LAVARIABLE;
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        waitForRestart = new WaitForSeconds(2.5f);
        text = GameObject.FindGameObjectWithTag("LAVARIABLE").GetComponent<TextMeshProUGUI>();
        text.text = LAVARIABLE.ToString();
        getParticles = new Dictionary<Coin.Type, ParticleSystem>();
        getParticles.Add(Coin.Type.Moneda, getCoins);
        getParticles.Add(Coin.Type.Pan, getBread);
        getParticles.Add(Coin.Type.PocionMana, getPocionMana);
        getParticles.Add(Coin.Type.PocionSalud, getPocionSalud);
        getParticles.Add(Coin.Type.Pollo, getChicken);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            HandleAttack();
            HandleMovement();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
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
        if (!invencible)
        {
            if (DamageType.None == type)
                currentLAVARIABLE -= damage;
            else
            {
                //Calcular resistencias.
                currentLAVARIABLE -= damage;
            }
            invencible = true;
            currentLAVARIABLE = Mathf.Clamp(currentLAVARIABLE,0,float.MaxValue);
            StartCoroutine(getInmune());
            text.text = currentLAVARIABLE.ToString();
        }
        getHit.Play();
        PlayClip("hit");
        if (currentLAVARIABLE <= 0 && !dead)
        {
            Die();
        }

    }

    private void Die()
    {
        //animator.SetTrigger(Const.DIE);
        PlayClip("death");
        dead = true;
        explosion.Play(16);
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return waitForRestart;
        currentLAVARIABLE = LAVARIABLE;
        text.text = currentLAVARIABLE.ToString();
        dead = false;
        GameController.Instance.Restart();
    }

    public void GrabArmor()
    {
        if (!armor)
        {
            animator.SetLayerWeight(1, 1);
            armor = true;
        }
        else
        {
            animator.SetLayerWeight(1, 0);
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

    public void GetCoins(Coin coins)
    {
        LAVARIABLE += coins.coins;
        text.text = LAVARIABLE.ToString();
        PlayClip("coin");
        getParticles[coins.type].Play();
    }

    public void LoseCoins(float coins)
    {
        LAVARIABLE -= coins;
        text.text = LAVARIABLE.ToString();
        PlayClip("losecoins");
        loseCoins.Play();
    }

    IEnumerator getInmune()
    {
        float inmune = 0;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color e = renderer.color;
        Color c = e;
        c.a = 0.3f;
        while (inmune < 0.5f)
        {
            
            yield return null;
            inmune+=Time.deltaTime;
            renderer.color = Color.Lerp(e,c,Mathf.PingPong(Time.time*10,1));
        }
        invencible = false;
        renderer.color = e;
    }
}
