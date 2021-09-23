using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item[] spawnItems;

    private void Start()
    {
        Transform[] spawnPoints = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Item randomItem = spawnItems[Random.Range(0, spawnItems.Length)];

            ItemObject newItem = randomItem.GetDropObject();
            newItem.SetItem(randomItem);
            newItem.transform.position = spawnPoints[i].position;
            newItem.transform.SetParent(transform);
        }
    }
}
