using UnityEngine;

public class Coin : MonoBehaviour {

    public enum Type{
        Pollo,
        Moneda,
        PocionMana,
        PocionSalud,
        Pan
    }

    public Type type;

    public float coins;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Player")){
            //PlayerController.Player.RecibeDamage(-coins,DamageType.None);
            PlayerController.Player.GetCoins(this);
            Destroy(gameObject);
        }
    }
}