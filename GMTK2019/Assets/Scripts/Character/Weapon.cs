using UnityEngine;
public abstract class Weapon : ScriptableObject {
    
    public abstract void Attack(PlayerController controller);
    public  Sprite sprite;

}