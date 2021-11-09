using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/Buffs")]
public class BuffSO: ScriptableObject {
    public string buffName;
    public GameObject buffPrefeb;
    public int poolingAmount;
    public float duration;
}
