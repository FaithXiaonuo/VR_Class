using UnityEngine;

public class BoxRotate : MonoBehaviour
{
    public float rot_Speed = 1;

    void Update()
    {
        transform.Rotate(-Vector3.up * rot_Speed);
    }
}
