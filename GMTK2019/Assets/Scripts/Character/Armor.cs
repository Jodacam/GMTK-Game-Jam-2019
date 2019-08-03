using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "GMTK2019/Armor", order = 0)]
public class Armor : ScriptableObject {
    
    public DamageType resistanceTo;
    public DamageType vulnerableTo;
    public Sprite sprite;
}