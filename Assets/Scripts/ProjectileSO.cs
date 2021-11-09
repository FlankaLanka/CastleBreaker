using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/Projectiles")]
public class ProjectileSO : ScriptableObject
{
    public string projectileName;
    public int initPoolingAmount;
    public GameObject projectilePrefeb;
    public float projSpeed;
    public float lifeTime;
    public int maxHit;
}
