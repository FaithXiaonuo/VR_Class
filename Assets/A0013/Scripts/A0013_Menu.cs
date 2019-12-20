using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0013_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion
#if UNITY_ANDROID
    public Transform vrRoot;
#endif

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeRepairPict(int index)
    {
        networkview.RPC("RpcRepairPict", RPCMode.All, index);
    }
    [RPC]
    void RpcRepairPict(int index, NetworkMessageInfo info)
    {
#if UNITY_STANDALONE_WIN
        Camera.main.transform.position = new Vector3(10f * index, 0, 0);
#endif
#if UNITY_ANDROID
        vrRoot.position = new Vector3(10f * index, 0, 0);
#endif
    }
}
