using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;



public class BuffManager : MonoBehaviour
{
    private ObjectPoolManager opm; 

    public List<BuffSO> buffRegList; 

    private Dictionary<string, BuffSO> buffTable;



    public static BuffManager Instance { get; private set;    }


    private void Awake() {
        buffTable = new Dictionary<string, BuffSO>();

        Instance = this;
        opm = gameObject.GetComponent<ObjectPoolManager>();

    }

    private void Start(){
        int b_amount;
        string b_name; 
        GameObject b_pfb; 

        foreach (BuffSO bso in buffRegList)
        {
            b_amount = bso.poolingAmount;
            b_name = bso.buffName;
            b_pfb = bso.buffPrefeb;
            buffTable[b_name] = bso;
            opm.addNewObjPool(b_pfb,b_name,b_amount);
        }

        buffRegList.Clear();

    }

    public void addBuffTo(string buffName, GameObject victim) {
        GameObject buffSprite = opm.getPooledObject(buffName);
        Buff b = buffSprite.GetComponent<Buff>();

        b.setBuffAttrs(victim, buffTable[buffName].duration, opm.getObjectPoolByName(buffName));
    }
}

