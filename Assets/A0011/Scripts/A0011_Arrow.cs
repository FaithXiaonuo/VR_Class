using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0011_Arrow : MonoBehaviour
{
    //   public Vector3 startPos, endPos;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {
    //       transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, 0.01f);
    //   }

    //   public void SetStart()
    //   {
    //       transform.localPosition = startPos;
    //   }

    void Update()
    {
        transform.GetComponent<MeshRenderer>().material.mainTextureOffset -= new Vector2(0.02f, 0);
    }

    public void SetStart()
    {
        transform.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2();
    }
}
