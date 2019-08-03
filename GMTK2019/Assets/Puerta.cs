using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    Animator anim;
    int numEnemies = 0;
    bool open = false;

    public float shakeDuration;
    public float shakeStrength;

    public void EnemyKilled(){
        this.numEnemies--;
        TryOpen();
    }

    public void SetEnemies(int numEnemies){
        this.numEnemies = numEnemies;
        anim = GetComponent<Animator>();
        TryOpen();
    }

    public void TryOpen(){
        if(numEnemies == 0){
            anim.SetTrigger("OpenDoor");
        }
    }

    public void SetOpen(){
        open = true;
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") && open){
            //Change Scene
        }
    }

    public void TriggerShake(){
        GameController.Instance.ScreenShake(shakeDuration,shakeStrength);
    }
}
