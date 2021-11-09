using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAudio : MonoBehaviour
{
    public static SingletonAudio Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
