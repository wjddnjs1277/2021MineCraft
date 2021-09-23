using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            StartCoroutine(Switching(child.gameObject));
        }
    }

    IEnumerator Switching(GameObject target)
    {
        bool isBefore = target.activeSelf;
        target.SetActive(true);
        yield return new WaitForEndOfFrame(); // 1�������� ��ٸ���.
        target.SetActive(isBefore);
    }
}
