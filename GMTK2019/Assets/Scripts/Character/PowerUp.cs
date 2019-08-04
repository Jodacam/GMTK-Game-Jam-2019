using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "GMTK2019/PowerUp", order = 0)]
public class PowerUp : ScriptableObject {
    public DamageType damageType;

    public int cost;
    public Sprite sprite;
}