using UnityEngine;

public class Projectile : DamageDealer {
    

    private Vector2 speed;

    public float multiplier;
    private void Update() {
        transform.Translate(speed.normalized*multiplier*Time.deltaTime);
    }

    public void Init(Vector2 initialSpeed){
        speed = initialSpeed;
    }

}