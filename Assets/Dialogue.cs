using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [SerializeField]
    public Queue<KeyValuePair<string, string>> lines;
}
