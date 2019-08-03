using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int cost;

    public Weapon weapon;

    public Armor armor;

    public PowerUp powerUp;

    private SpriteRenderer renderer;

private void Start() {
    renderer = GetComponent<SpriteRenderer>();
}
    // Update is called once per frame
   private void OnTriggerStay2D(Collider2D other) {
       if(other.tag.Equals("Player")){
           if(Input.GetButton("Jump")){
               PlayerController.Player.LAVARIABLE -=cost;
               if(weapon){
                   PlayerController.Player.actualWeapon = weapon;
               }else if(armor){
                   PlayerController.Player.actualArmor = armor;
               }else
               {
                   PlayerController.Player.actualPowerUp = powerUp;
               }
           }
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
