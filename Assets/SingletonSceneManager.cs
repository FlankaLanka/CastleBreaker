using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSceneManager : MonoBehaviour
{
    public static SingletonSceneManager Instance;

    private void Awake()
    {
        if(Instance == null)
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
