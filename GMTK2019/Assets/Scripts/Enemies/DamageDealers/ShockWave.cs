using UnityEngine;

public class ShockWave : Projectile {
    


    void Update() {
        base.Update();
        transform.localScale = transform.localScale+new Vector3(0.3f*Time.deltaTime,0.3f*Time.deltaTime,0);
    }

    public override void Init(Vector2 initialSpeed){
        base.Init(initialSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }
}