using System.Collections;
using Pathfinding;
using UnityEngine;
using static EnemyController;

public class Seaker : EnemyController
{


    private IAstarAI aiController;


    public float damage;

    public Vector2 range;

    [Tooltip("Distancia maxima a la que tiene que estar el jugador para que el seaker le persiga.")]
    public float maxDistance;

    [Tooltip("Distancia a la que se queda la IA para atacar.")]
    public float minDistance;
    public new void Start()
    {
        base.Start();
        aiController = GetComponent<IAstarAI>();
        if (aiController != null) aiController.onSearchPath += Update;
    }


    private void Update()
    {
        aiController.maxSpeed = Mathf.Lerp(15,25,(PlayerController.Player.currentLAVARIABLE-40)/300);
        base.Update();
        float distance = Vector3.Distance(PlayerController.Player.transform.position, transform.position);
        switch (state)
        {
            case EnemyController.State.Moving:
                dir = aiController.velocity.normalized;
                animator.SetFloat(Const.X_DIR, dir.x);
                animator.SetFloat(Const.Y_DIR, dir.y);
                animator.SetFloat(Const.SPEED, dir.sqrMagnitude);
                if (distance <= maxDistance)
                {
                    if (aiController != null)
                        aiController.destination = PlayerController.Player.transform.position;
                }

                if (distance <= minDistance)
                {
                    state = EnemyController.State.Attacking;
                    aiController.canMove = false;
                    aiController.canSearch = false;
                    animator.SetFloat(Const.SPEED, 0);
                }

                break;
            case EnemyController.State.Attacking:
                if (distance >= minDistance)
                {
                    state = EnemyController.State.Moving;
                    aiController.canMove = true;
                    aiController.canSearch = true;
                }

                dir = (PlayerController.Player.transform.position - transform.position).normalized;
                if (innerCoolDown <= 0)
                {
                    innerCoolDown = Mathf.Lerp(2,0.5f,PlayerController.Player.getLimit());
                    Attack();
                    animator.SetTrigger("attack");
                }
                else
                {
                    innerCoolDown -= Time.deltaTime;
                }

                break;
        }
        animator.SetFloat(Const.X_DIR, dir.x);
        animator.SetFloat(Const.Y_DIR, dir.y);

    }

    public override void RecibeDamage(float damage, Vector2 dir)
    {
        base.RecibeDamage(damage,dir);
        if(!stacionary){
            aiController.destination = PlayerController.Player.transform.position;
            aiController.Teleport(transform.position+new Vector3(dir.x,dir.y,0)*16,true);
            StartCoroutine(Stop());
        }
    }

    public IEnumerator Stop() {
        aiController.canMove=false;
        yield return new WaitForSeconds(0.1f);
        aiController.canMove = true;
    }
    public override void Attack()
    {
        var touched = Physics2D.OverlapBoxAll(transform.position + new Vector3(dir.x, dir.y, 0) * 16, new Vector3(range.x, range.y, 0) * 16, 0);
        for (int i = 0; i < touched.Length; i++)
        {
            var player = touched[i].transform.GetComponent<PlayerController>();
            if (player)
            {
                player.RecibeDamage(damage, type);
            }
        }
    }
}