using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/Skills")]
public class SkillSO: ScriptableObject {
    public string skillName;
    public Sprite skillImage;
    public GameObject skillPrefeb;
    public ProjectileSO projectileDef;
    public WarheadSO warheadDef;

    public float damage;
    public float cost;
    public float cooldown;
}
