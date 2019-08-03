using UnityEngine;
using System;
using System.Collections;
public class Boom : Seaker {


    public float explodeTime = 2;
    public override void Attack(){
        StartCoroutine(Explode());
        
    }

    IEnumerator Explode(){
        state = EnemyController.State.Exploding;
        yield return new WaitForSeconds(explodeTime);
        var touched = Physics2D.OverlapBoxAll(transform.position+new Vector3(dir.x,dir.y,0)*16,new Vector3(range.x,range.y,0)*16,0);
        GameController.Instance.ScreenShake(0.2f,20);
        for(int i = 0; i<touched.Length;i++){
            var player = touched[i].transform.GetComponent<PlayerController>();
            if(player){
                player.RecibeDamage(damage,type);
            }
        }
        Die();
    }

    public override void Die(){
        StopAllCoroutines();
        base.Die();
    }
}