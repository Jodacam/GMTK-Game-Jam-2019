using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int cost;

    public Weapon weapon;

    public Armor armor;

    public PowerUp powerUp;

    public SpriteRenderer renderer;

    public TextMeshPro price;

    public bool onShop = false;
    private void Update() {
        if(onShop){
            if(Input.GetButtonDown("Jump")){
               PlayerController.Player.LoseCoins(cost);
               if(weapon){
                   
                   PlayerController.Player.ActualWeapon(weapon);
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
        
        this.price.text = cost.ToString();
        this.cost = cost;
        this.weapon = weapon;
        renderer.sprite = weapon.sprite;
    }

    public void Init(Armor weapon,int cost)
    {
        this.price.text = cost.ToString();
        this.cost = cost;
        this.armor = weapon;
        renderer.sprite = weapon.sprite;
    }
        public void Init(PowerUp weapon,int cost)
    {
        this.price.text = cost.ToString();
        this.cost = cost;
        this.powerUp = weapon;
        renderer.sprite = weapon.sprite;
    }
}
