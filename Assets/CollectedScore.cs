using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedScore : MonoBehaviour
{
    private void OnEnable()
    {
        int score = GameObject.FindGameObjectsWithTag("HealthPot").Length;
        gameObject.transform.Find("HealthImage").Find("Text").GetComponent<Text>().text = (3 - score).ToString() + "/3";
    }
}
