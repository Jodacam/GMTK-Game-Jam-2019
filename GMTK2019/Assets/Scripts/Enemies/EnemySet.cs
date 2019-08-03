using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySet", menuName = "GMTK2019/EnemySet", order = 0)]
public class EnemySet : ScriptableObject
{
    public List<EnemyController> enemies;
    
}
