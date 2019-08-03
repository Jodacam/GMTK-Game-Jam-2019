using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string X_DIR = "xDir";
    const string Y_DIR = "yDir";

    const string SPEED = "velocity";
    Animator animator;

    public Vector2 minSpeed = new Vector2(1,1);

    public float coolDown = 1;


    private float innerCoolDown = 0;
    public Vector2 dir;

    public Vector2 speed;
    public float LAVARIABLE = 100;

    public Weapon actualWeapon;
    public bool isAttacking;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
        HandleMovement();
    }

    private void HandleAttack()
    {
        if(innerCoolDown <= 0){
            bool pressed = Input.GetButtonDown("Jump");
            if(pressed){
                innerCoolDown = coolDown;
                animator.SetTrigger("attack");
                actualWeapon.Attack(this);
            }
        }else{
            innerCoolDown-=Time.deltaTime;
        }
    }

    void HandleMovement(){
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");


        if(x != 0){
            y = 0;
            dir.x = x;
            dir.y = 0;
        }else if(y !=0 ){
            x = 0;
            dir.y = y;
            dir.x = 0;
        }

        var normalized = new Vector2(x,y).normalized;
        speed = Vector2.Scale(normalized,minSpeed);
        transform.Translate(speed*Time.deltaTime);

        animator.SetFloat(X_DIR,dir.x);
        animator.SetFloat(Y_DIR,dir.y);
        animator.SetFloat(SPEED,speed.sqrMagnitude);
    }
}
