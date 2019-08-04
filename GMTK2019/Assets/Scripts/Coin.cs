using UnityEngine;

public class Coin : MonoBehaviour {
    public float coins;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Player")){
            //PlayerController.Player.RecibeDamage(-coins,DamageType.None);
            PlayerController.Player.GetCoins(coins);
            Destroy(gameObject);
        }
    }
}