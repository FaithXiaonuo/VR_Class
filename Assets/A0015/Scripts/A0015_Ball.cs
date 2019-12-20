using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0015_Ball : MonoBehaviour
{
    public bool isUP = false;
    public float height = 0f;
    float x = 0, z = 0;

    // Use this for initialization
    void Start()
    {
        x = transform.position.x;
        z = transform.position.z;
        transform.position = new Vector3(x, height, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUP)
        {
            transform.position += new Vector3(0, (1.1f - transform.position.y) * 0.1f, 0);
            transform.Rotate(Vector3.left, (1.1f - transform.position.y) * 30);
            if ((1.1f - transform.position.y) < 0.05f)
            {
                isUP = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, (1.1f - transform.position.y) * 0.1f, 0);
            transform.Rotate(Vector3.left, (transform.position.y - 1.1f) * 30);
            if ((transform.position.y - 0.1f) < 0.05f)
            {
                isUP = true;
            }
        }
    }
}
