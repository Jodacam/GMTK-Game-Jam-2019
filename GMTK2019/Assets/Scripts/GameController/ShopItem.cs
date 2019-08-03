﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int cost;

    public Weapon weapon;

    public Armor armor;

    public PowerUp powerUp;

    public SpriteRenderer renderer;

    public bool onShop = false;
    private void Update() {
        if(onShop){
            if(Input.GetButtonDown("Jump")){
               PlayerController.Player.LAVARIABLE -=cost;
               if(weapon){
                   PlayerController.Player.actualWeapon = weapon;
               }else if(armor){
                   PlayerController.Player.GrabArmor();
                   PlayerController.Player.actualArmor = armor;
               }else
               {
                   PlayerController.Player.actualPowerUp = powerUp;
               }
               Destroy(gameObject);
           }
        }
    }
    // Update is called once per frame
   private void OnTriggerEnter2D(Collider2D other) {
       if(other.tag.Equals("Player")){
           onShop = true;
       }
   }

    private void OnTriggerExit2D(Collider2D other) {
       if(other.tag.Equals("Player")){
           onShop = false;
       }
   }

    public void Init(Weapon weapon,int cost)
    {
        

        this.weapon = weapon;
        renderer.sprite = weapon.sprite;
    }

    public void Init(Armor weapon,int cost)
    {
        
        this.armor = weapon;
        renderer.sprite = weapon.sprite;
    }
        public void Init(PowerUp weapon,int cost)
    {
        
        this.powerUp = weapon;
        renderer.sprite = weapon.sprite;
    }
}
