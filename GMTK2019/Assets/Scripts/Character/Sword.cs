using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "GMTK2019/Sword", order = 0)]
public class Sword : Weapon
{
    public override void Attack(PlayerController controller)
    {
        controller.PlayClip("mele");
        //Si hay varios Frames, usar corutina.
        var touched = Physics2D.BoxCastAll(controller.transform.position+new Vector3(controller.dir.x,controller.dir.y,0)*16,new Vector2(16,16),0,controller.dir);
        for(int i = 0; i<touched.Length;i++){
            var enemy = touched[i].transform.GetComponent<EnemyController>();
            if(enemy){
                enemy.RecibeDamage(controller.GetDamage());
            }
        }
    }
} 