using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWave : MonoBehaviour
{
    float scale = 1;
    float heightScale = 0.3f;

    bool isGround = false;

    float x;
    float z;

    private void Start()
    {
        gameObject.layer = 0;
    }

    private void FixedUpdate()
    {
        if(isGround)
        {
            float moveY = heightScale * Mathf.PerlinNoise(Time.time + (x * scale),
                                                        Time.time + (z * scale));
            transform.position = new Vector3(x, moveY + 0.7f, z);
        }
    }

    void ChangeLayer()
    {
        gameObject.layer = 8;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGround == false && collision.gameObject.layer.Equals(7))
        {
            x = transform.position.x;
            z = transform.position.z;
            transform.GetComponent<Rigidbody>().useGravity = false;
            isGround = true;
            Invoke("ChangeLayer", 0.5f);
        }
    }
}
