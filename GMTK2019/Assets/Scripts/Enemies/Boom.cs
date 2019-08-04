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
        float range = Mathf.Lerp(2,4,PlayerController.Player.getLimit());
        GetComponentInChildren<PlayExplosion>().Play(range);
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().enabled = false;
        var touched = Physics2D.OverlapBoxAll(transform.position,new Vector3(range,range,0)*16,0);
        for(int i = 0; i<touched.Length;i++){
            var player = touched[i].transform.GetComponent<PlayerController>();
            if(player){
                player.RecibeDamage(damage,type);
            }
        }
        yield return new WaitForSeconds(2);
        Die();
    }


    public override void Die(){
        StopAllCoroutines();
        base.Die();
    }
}