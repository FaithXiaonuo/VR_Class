using System.Collections.Generic;
using UnityEngine;

public class AniPlay : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public List<Animator> aniList;
    public AudioSource audioPlayer;
    
    void Start()
    {
        networkview = GetComponent<NetworkView>();

        StopAni();
    }

    public void Ani(int index)
    {
        networkview.RPC("RpcSatelliteAni", RPCMode.All, index);
    }
    [RPC]
    void RpcSatelliteAni(int index, NetworkMessageInfo info)
    {
        switch (index)
        {
            case 0:
                StartAni();
                break;
            case 1:
                PauseAni();
                break;
            case 2:
                StopAni();
                break;
            case 3:
                StopAni();
                StartAni();
                break;
        }
    }

    void StartAni()
    {
        for (int i = 0; i < aniList.Count; i++)
        {
            aniList[i].Play("New State");
            aniList[i].speed = 1;
        }
        if (audioPlayer)
            audioPlayer.Play();
    }

    void PauseAni()
    {
        for (int i = 0; i < aniList.Count; i++)
        {
            aniList[i].speed = 0;
        }
        if (audioPlayer)
            audioPlayer.Pause();
    }

    void StopAni()
    {
        for (int i = 0; i < aniList.Count; i++)
        {
            aniList[i].Play("Stop");
        }
        if (audioPlayer)
            audioPlayer.Stop();
    }
}
