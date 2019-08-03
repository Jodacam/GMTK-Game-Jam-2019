using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "GMTK2019/Gun", order = 0)]
public class Gun : Weapon
{
    public GameObject projectile;
    public override void Attack(PlayerController controller)
    {
        float z = 0;
        Vector2 dir = controller.dir;

        if(dir.x == 1){
            z = 0;
        }else if(dir.y==1){
            z = 90;
        }else if(dir.x == -1)
        {
            z = 180;    
        }else if(dir.y == -1){
            z = 270;
        }

        var obj = Instantiate(prefab,controller.transform);
        obj.GetComponent<Animator>().SetFloat(Const.X_DIR,dir.x);
        obj.GetComponent<Animator>().SetFloat(Const.Y_DIR,dir.y);
        Destroy(obj,0.15f);
        var bullet = Instantiate(projectile,controller.transform.position + new Vector3(dir.x,dir.y,0)*20,Quaternion.Euler(0,0,z));
        bullet.GetComponent<Bullet>().Init(controller.dir);
    }
}