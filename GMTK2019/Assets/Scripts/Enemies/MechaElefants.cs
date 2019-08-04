using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MechaElefants : EnemyController
{

    enum mState
    {
        Waiting,
        SockWave,
        Projectile
    }

    mState mstate;
    public Projectile sockwave;

    public Projectile projectile;


    [MinMaxSlider(2f, 10)]
    public Vector2 sockwaveTime;

    [MinMaxSlider(1f, 10)]
    public Vector2 projectileTime;

    [MinMaxSlider(1f, 3)]
    public Vector2 waitTime;

    public float innerCooldown;

    public float timeShoot = 1;
    public float timeShock = 0.5f;

    bool left = false;

    public Transform[] leftRigth;
    public float nextCoolDown;
    public override void Attack()
    {
        if (innerCoolDown >= timeShoot)
        {
            innerCoolDown = 0;
            var obj = Instantiate(projectile, transform.position + new Vector3(dir.x, dir.y, 0) * 16, Quaternion.identity);
            obj.Init(dir);
        }
        else
        {
            innerCoolDown += Time.deltaTime;
        }
    }

    private void ShockWaveAttack()
    {
        if (innerCoolDown >= timeShock)
        {
            GameController.Instance.ScreenShake(0.1f, 2);
            PlayClip("shockwave");

            innerCoolDown = 0;
            Transform t = null;

            if (left)
            {
                t = leftRigth[0];
            }
            else
            {
                t = leftRigth[1];
            }
            var obj = Instantiate(sockwave, t.position, Quaternion.identity);
            obj.Init(dir);
            left = !left;
        }
        else
        {
            innerCoolDown += Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        innerCoolDown = 0;
        mstate = mState.Waiting;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        switch (mstate)
        {
            case mState.Projectile:
                if (innerCooldown >= nextCoolDown)
                {
                    innerCooldown = 0;
                   
                    int n = Random.Range(0, 2);
                    if (n == 0)
                    {
                        mstate = mState.Waiting;
                        nextCoolDown = Random.Range(waitTime.x, waitTime.y);

                    }
                    else
                    {
                        animator.SetTrigger("attack");
                        mstate = mState.SockWave;
                        nextCoolDown = Random.Range(sockwaveTime.x, sockwaveTime.y);
                    }
                }
                else
                {
                    innerCooldown += Time.deltaTime;
                    dir = (PlayerController.Player.transform.position - transform.position).normalized;
                    Attack();
                }
                break;
            case mState.SockWave:
                if (innerCooldown >= nextCoolDown)
                {
                    innerCooldown = 0;
                    animator.SetTrigger("calm");
                    int n = Random.Range(0, 2);
                    if (n == 0)
                    {
                        mstate = mState.Waiting;
                        nextCoolDown = Random.Range(waitTime.x, waitTime.y);

                    }
                    else
                    {
                        mstate = mState.Projectile;
                        nextCoolDown = Random.Range(projectileTime.x, projectileTime.y);
                    }
                }
                else
                {
                    innerCooldown += Time.deltaTime;
                    dir = (PlayerController.Player.transform.position - transform.position).normalized;
                    ShockWaveAttack();
                }
                break;
            case mState.Waiting:

                if (innerCooldown >= nextCoolDown)
                {
                    innerCooldown = 0;
                    int n = Random.Range(0, 2);
                    if (n == 0)
                    {

                        animator.SetTrigger("attack");
                        mstate = mState.SockWave;
                        nextCoolDown = Random.Range(sockwaveTime.x, sockwaveTime.y);
                    }
                    else
                    {
                        mstate = mState.Projectile;
                        nextCoolDown = Random.Range(projectileTime.x, projectileTime.y);
                    }

                }
                else
                {

                    innerCooldown += Time.deltaTime;
                }
                break;

        }
    }
}
