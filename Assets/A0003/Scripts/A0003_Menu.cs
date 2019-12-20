using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0003_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public Transform  _CameraPosRoot;

    bool isRotate;
    int targetRot;

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseCamPos(int index)
    {
        networkview.RPC("RpcChooseCamPos", RPCMode.All, index);
    }
    [RPC]
    void RpcChooseCamPos(int index, NetworkMessageInfo info)
    {
        SetCamPos(index);
    }
    void SetCamPos(int index)
    {
#if UNITY_STANDALONE_WIN
        Camera.main.transform.position = _CameraPosRoot.GetChild(index).position;
        Camera.main.transform.rotation = _CameraPosRoot.GetChild(index).rotation;
#endif
#if UNITY_ANDROID
        transform.GetComponent<MainMenu>().vrRoot.position = _CameraPosRoot.GetChild(index).position;
        transform.GetComponent<MainMenu>().vrRoot.rotation = _CameraPosRoot.GetChild(index).rotation;
#endif
    }
}
