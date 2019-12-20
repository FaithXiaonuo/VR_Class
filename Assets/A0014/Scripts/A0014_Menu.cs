using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0014_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public Animation _Robot;

    public AudioSource _AudioSource;
    public List<AudioClip> _AudioClips;
    float time = 0;

#if UNITY_ANDROID
    public List<GameObject> targetRootList;
#endif

    void Start()
    {
        networkview = GetComponent<NetworkView>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 300)
        {
            _AudioSource.clip = _AudioClips[0];
            _AudioSource.Play();
        }

#if UNITY_ANDROID
        int trigger = transform.GetComponent<VRHotspot>().Trigger();
        if (trigger >= 0)
        {
            trigger = trigger % 3 + 1;
            _AudioSource.clip = _AudioClips[trigger];
            _AudioSource.Play();
        }
#endif
    }

    public void StartAni()
    {
        networkview.RPC("RpcStartAni", RPCMode.All, 0);
    }
    [RPC]
    void RpcStartAni(int index, NetworkMessageInfo info)
    {
        _Robot.Play();
    }

    public void StopAni()
    {
        networkview.RPC("RpcStopAni", RPCMode.All, 0);
    }
    [RPC]
    void RpcStopAni(int index, NetworkMessageInfo info)
    {
        _Robot.Stop();
    }

    public void SetCamPos(int index)
    {
        networkview.RPC("RpcSetCamPosforTarget", RPCMode.All, 0);
    }
    [RPC]
    void RpcSetCamPosforTarget(int index, NetworkMessageInfo info)
    {
#if UNITY_ANDROID
        for (int i = 0; i < targetRootList.Count; i++)
        {
            if (targetRootList[i].activeInHierarchy == true)
            {
                targetRootList[i].SetActive(false);
            }
        }
        targetRootList[index].SetActive(true);
#endif
    }
}
