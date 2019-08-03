using UnityEngine;

public class Projectile : DamageDealer {
    

    private Vector2 speed;
    private void Update() {
        transform.Translate(speed*Time.deltaTime);
    }

    public void Init(Vector2 initialSpeed){
        speed = initialSpeed;
    }

}