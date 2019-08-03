using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "GMTK2019/Sword", order = 0)]
public class Sword : Weapon
{
    [Tooltip("La x es el rango cuando se apunta a la dirrección contrraria, por ejemplo, si se esta mirando arriba, el rango en horizontal se multiplica por x.")]
    public Vector2 range;
    public override void Attack(PlayerController controller)
    {
        var obj = Instantiate(prefab,Vector3.zero ,Quaternion.identity,controller.transform);
        obj.GetComponent<Animator>().SetFloat(Const.X_DIR,controller.dir.x);
        obj.GetComponent<Animator>().SetFloat(Const.Y_DIR,controller.dir.y);
        Destroy(obj,0.4f);
        controller.PlayClip("mele");
        //Si hay varios Frames, usar corutina.
        float xRange = controller.dir.x == 0 ? 15*range.x:15*range.y;
        float yRange = controller.dir.y == 0 ? 15*range.x:15*range.y;
        var touched = Physics2D.OverlapBoxAll(controller.transform.position+new Vector3(controller.dir.x,controller.dir.y,0)*16,new Vector2(xRange,yRange),0);
        for(int i = 0; i<touched.Length;i++){
            var enemy = touched[i].transform.GetComponent<EnemyController>();
            if(enemy){
                enemy.RecibeDamage(controller.GetDamage(),controller.dir);
            }
        }
    }
} 