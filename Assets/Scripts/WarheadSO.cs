using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Warheads")]
public class WarheadSO: ScriptableObject {
    public string warheadName;
    public int initPoolingAmount;
    public GameObject warheadPrefeb;
}
