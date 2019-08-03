using UnityEngine;
public abstract class Weapon : ScriptableObject {
    
    public abstract void Attack(PlayerController controller);

    public GameObject prefab;
    public  Sprite sprite;

}