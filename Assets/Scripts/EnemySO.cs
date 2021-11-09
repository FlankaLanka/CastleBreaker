using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Enemies")]
public class EnemySO : ScriptableObject {
    public GameObject enemyPrefeb;
    public string enemyName; 
    public float hitPoint;
    public float speed;
    public float attack;
    public List<ProjectileSO> weaponProjs;
}
