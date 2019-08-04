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
        animator.SetFloat("weapon", weapon.id);
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
    Dictionary<Coin.Type, ParticleSystem> getParticles;

    public ParticleSystem getHit;

    public ParticleSystem loseCoins;

    public PlayExplosion explosion;

    public Transform raycastInit;

    public GameObject[] maldicionesObject;
    public GameObject[] bendicionesObject;

    Dictionary<string, bool> maldiciones = new Dictionary<string, bool>(){
        {"speed",false},
        {"cooldown",false},
        {"thunder",false},
        {"fire",false},
        {"ice",false},
        {"costMoney",false}
    };

    Dictionary<string, bool> bendiciones = new Dictionary<string, bool>(){
        {"coins",false},
        {"plusDamage",false}
    };
    void Start()
    {
        currentLAVARIABLE = LAVARIABLE;
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        waitForRestart = new WaitForSeconds(2.5f);
        text = GameObject.FindGameObjectWithTag("LAVARIABLE").GetComponent<TextMeshProUGUI>();
        text.text = currentLAVARIABLE.ToString();
        getParticles = new Dictionary<Coin.Type, ParticleSystem>();
        getParticles.Add(Coin.Type.Moneda, getCoins);
        getParticles.Add(Coin.Type.Pan, getBread);
        getParticles.Add(Coin.Type.PocionMana, getPocionMana);
        getParticles.Add(Coin.Type.PocionSalud, getPocionSalud);
        getParticles.Add(Coin.Type.Pollo, getChicken);
        getBuffs();
    }


    private void getBuffs()
    {


        foreach (var item in maldicionesObject)
        {
            item.SetActive(false);
        }


        foreach (var item in bendicionesObject)
        {
            item.SetActive(false);
        }
        maldiciones = new Dictionary<string, bool>(){
        {"speed",false},
        {"cooldown",false},
        {"thunder",false},
        {"fire",false},
        {"ice",false},
        {"costMoney",false}
    };
        bendiciones = new Dictionary<string, bool>(){
        {"coins",false},
        {"plusDamage",false}
    };

        var s = new string[6];
        maldiciones.Keys.CopyTo(s, 0);

        int next = UnityEngine.Random.Range(0, 6);
        maldiciones[s[next]] = true;

        int next2 = UnityEngine.Random.Range(0, 6);
        if (next2 == next)
            next2 = (next2 + 1) % 6;
        maldiciones[s[next2]] = true;

        maldicionesObject[next].SetActive(true);
        maldicionesObject[next2].SetActive(true);

        s = new string[2];
        bendiciones.Keys.CopyTo(s, 0);

        next = UnityEngine.Random.Range(0, 2);
        bendiciones[s[next]] = true;
        bendicionesObject[next].SetActive(true);
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
        return currentLAVARIABLE;
    }

    private void HandleAttack()
    {
        if (innerCoolDown <= 0)
        {
            bool pressed = Input.GetButtonDown("Jump");
            if (pressed)
            {
                innerCoolDown = maldiciones["cooldown"] ? coolDown + Mathf.Lerp(0, 2, currentLAVARIABLE - 40 / 350) : coolDown;
                animator.SetTrigger("attack");

                if (maldiciones["costMoney"])
                    currentLAVARIABLE = Mathf.Min(1, currentLAVARIABLE - ((currentLAVARIABLE * 5) / 100));
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
        if (maldiciones["speed"])
            speed *= 1 / Mathf.Lerp(1, 5, (currentLAVARIABLE - 40) / 500);
        if (speed.magnitude >= 0.01)
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
        //if(!Physics2D.Raycast(raycastInit.position,dir,17,LayerMask.GetMask("Enemies","Walls") ){

        transform.Translate(speed * Time.deltaTime);
        //}

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
                DamageType vulnerable = DamageType.None;
                //Calcular resistencias.
                if (maldiciones["fire"])
                    vulnerable = DamageType.Fire;
                else
                if (maldiciones["thunder"])
                    vulnerable = DamageType.Thunder;
                else
                if (maldiciones["ice"])
                    vulnerable = DamageType.Ice;
                if(vulnerable==type){
                    damage*=2;
                }

                if(armor){
                    if(actualArmor.vulnerableTo == type){
                        damage*=2;
                    }else{
                        if(actualArmor.resistanceTo == type){
                            damage/=2;
                        }
                    }
                }
                currentLAVARIABLE -= damage;
            }
            invencible = true;
            currentLAVARIABLE = Mathf.Clamp(currentLAVARIABLE, 0, float.MaxValue);
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
        getBuffs();
        GameController.Instance.Restart();


    }

    public void GrabArmor()
    {
        animator.SetLayerWeight(1, 1);
        armor = true;
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
        currentLAVARIABLE += coins.coins;
        text.text = currentLAVARIABLE.ToString();
        PlayClip("coin");
        getParticles[coins.type].Play();
    }

    public void LoseCoins(float coins)
    {
        currentLAVARIABLE -= coins;
        text.text = currentLAVARIABLE.ToString();
        PlayClip("losecoins");
        loseCoins.Play();
        currentLAVARIABLE = Mathf.Clamp(currentLAVARIABLE,0,float.MaxValue);
        if(currentLAVARIABLE == 0){
            Die();
        }
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
            inmune += Time.deltaTime;
            renderer.color = Color.Lerp(e, c, Mathf.PingPong(Time.time * 10, 1));
        }
        invencible = false;
        renderer.color = e;
    }
}
