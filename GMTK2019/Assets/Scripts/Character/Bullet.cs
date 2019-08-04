using UnityEngine;

public class Bullet : MonoBehaviour {

    private Vector2 dir;

    public float speed = 1;

    public float damage = 10;
    public void Init(Vector2 dir){
        this.dir =dir;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Enemy")){
            other.transform.GetComponent<EnemyController>().RecibeDamage(PlayerController.Player.GetDamage(),dir);
        }
        if(!other.tag.Equals("DamageDealer")){
            Destroy(gameObject);
        }

    }

    private void Update() {
        transform.Translate(dir*speed,Space.World);
    }
}