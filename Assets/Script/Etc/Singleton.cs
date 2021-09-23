using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            // instance가 없을 때 씬에서 검색한다.
            if(instance == null)
            {
                instance = FindObjectOfType<T>(true);
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = GetComponent<T>();
    }
}
