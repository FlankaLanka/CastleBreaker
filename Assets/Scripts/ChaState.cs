using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaState: MonoBehaviour
{
    public float hitPoint;
    public float magicPoint;
    public float speed;

    public List<SkillSO> skills;

    public bool isAIControled = true;

    public bool blockMoving = false;
    public bool blockDamage = false;
    public bool blockSkills = false;
    public bool blockAnim = false;
    public bool takingDamageAnim = false;

    //public Item item;
    
}
