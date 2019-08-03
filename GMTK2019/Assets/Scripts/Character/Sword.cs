using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "GMTK2019/Sword", order = 0)]
public class Sword : Weapon
{
    public override void Attack(PlayerController controller)
    {
        var obj = Instantiate(prefab,Vector3.zero ,Quaternion.identity,controller.transform);
        obj.GetComponent<Animator>().SetFloat(Const.X_DIR,controller.dir.x);
        obj.GetComponent<Animator>().SetFloat(Const.Y_DIR,controller.dir.y);
        Destroy(obj,0.4f);
        controller.PlayClip("mele");
        //Si hay varios Frames, usar corutina.
        var touched = Physics2D.OverlapBoxAll(controller.transform.position+new Vector3(controller.dir.x,controller.dir.y,0)*16,new Vector2(15*(1+(2*Mathf.Abs(controller.dir.y))),15*(1+(2*Mathf.Abs(controller.dir.x)))),0);
        for(int i = 0; i<touched.Length;i++){
            var enemy = touched[i].transform.GetComponent<EnemyController>();
            if(enemy){
                enemy.RecibeDamage(controller.GetDamage());
            }
        }
    }
} 