using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class UfoCat : EnemyController
{

    public Projectile projectile;

    private IAstarAI aiController;

    public float shootDistance;

    public float hiddeDistance;
    public override void Attack()
    {
        PlayClip("Shoot");
        var obj = Instantiate(projectile,transform.position+new Vector3(dir.x,dir.y,0)*16,Quaternion.identity);
        obj.Init(dir);
    }

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        aiController = GetComponent<IAstarAI>();
        state = EnemyController.State.Stacionary;
        if (aiController != null) aiController.onSearchPath += Update;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        float distance = Vector3.Distance(PlayerController.Player.transform.position, transform.position);
        switch (state)
        {
            case EnemyController.State.Moving:
            dir = aiController.velocity.normalized;
            dir = (PlayerController.Player.transform.position - transform.position).normalized;
            aiController.destination = transform.position-new Vector3(dir.x,dir.y,0)*16;
            aiController.maxSpeed = Mathf.Lerp(24,48,(PlayerController.Player.currentLAVARIABLE-40)/500);
            animator.SetFloat(Const.X_DIR, -dir.x);
            animator.SetFloat(Const.Y_DIR, -dir.y);

            if(distance>hiddeDistance){
                state = EnemyController.State.Stacionary;
            }
            break;
            case EnemyController.State.Attacking:


            if(distance>shootDistance){
                state = EnemyController.State.Stacionary;
            }else if(distance<hiddeDistance){
                state = EnemyController.State.Moving;
            }
            else
            {
                dir = (PlayerController.Player.transform.position - transform.position).normalized;
                animator.SetFloat(Const.X_DIR, -dir.x);
                animator.SetFloat(Const.Y_DIR, -dir.y);
                if (innerCoolDown <= 0)
                {
                    innerCoolDown = coolDown;
                    Attack();
                }
                else
                {
                    innerCoolDown -= Time.deltaTime;
                } 
            }
                
            
            break;
            case EnemyController.State.Stacionary:
            if(distance<=shootDistance){
                state = EnemyController.State.Attacking;
            }
            if(distance<=hiddeDistance){
                 state = EnemyController.State.Moving;
            }
            break;
        }
    }
}
