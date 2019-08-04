using UnityEngine;

public class OcTurret : EnemyController
{

     public Projectile projectile;

    


    public override void Attack()
    {
         var obj = Instantiate(projectile,transform.position+new Vector3(dir.x,dir.y,0)*16,Quaternion.identity);
         obj.Init(dir);
    }

    private void Update() {
        dir = (PlayerController.Player.transform.position - transform.position).normalized;
        dir = new Vector2(Mathf.RoundToInt(dir.x),Mathf.RoundToInt(dir.y));
        animator.SetFloat(Const.X_DIR, dir.x);
        animator.SetFloat(Const.Y_DIR, dir.y);
        if(innerCoolDown <=0){
            innerCoolDown+=coolDown;
            Attack();
        }else{
            innerCoolDown-=Time.deltaTime;
        }
    }
}