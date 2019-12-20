using System.Collections.Generic;
using UnityEngine;

public class FocusOnDetail : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public List<Transform> CameraPos;

    public AudioSource audioPlayer;
    public List<AudioClip> audioList;

    Transform target;
    bool isMove = false;

#if UNITY_ANDROID
    public Transform vrRoot;

    public List<GameObject> tipList;
    public List<GameObject> markList;
#endif
    
    void Start()
    {
        networkview = GetComponent<NetworkView>();
    }
    
    void Update()
    {
        if (isMove)
        {
#if UNITY_STANDALONE_WIN
            Vector3 pos = Camera.main.transform.position;
            Vector3 targetpos = target.position;
            Quaternion rot = Camera.main.transform.rotation;
            Quaternion targetrot = target.rotation;
            if (Vector3.Distance(pos, targetpos) > 0.1f || Quaternion.Angle(rot, targetrot) > 1)
            {
                Camera.main.transform.position = Vector3.Lerp(pos, targetpos, 0.1f);
                Camera.main.transform.rotation = Quaternion.Slerp(rot, targetrot, 1f);
            }
            else
            {
                isMove = false;
            }
#endif

#if UNITY_ANDROID
            Vector3 pos = vrRoot.transform.position;
            Vector3 targetpos = target.position;
            Quaternion rot = vrRoot.transform.rotation;
            Quaternion targetrot = target.rotation;
            if (Vector3.Distance(pos, targetpos) > 0.1f || Quaternion.Angle(rot, targetrot) > 1)
            {
                vrRoot.transform.position = Vector3.Lerp(pos, targetpos, 0.1f);
                vrRoot.transform.rotation = Quaternion.Slerp(rot, targetrot, 1f);
            }
            else
            {
                isMove = false;
            }
#endif
        }
    }

    public void SetCamera(int index)
    {
        networkview.RPC("RpcSetCamera", RPCMode.All, index);
    }
    [RPC]
    void RpcSetCamera(int index, NetworkMessageInfo info)
    {
        target = CameraPos[index];
        isMove = true;
        audioPlayer.clip = audioList[index];
        audioPlayer.Play();

#if UNITY_ANDROID
        for (int i = 0; i < markList.Count; i++)
        {
            markList[i].SetActive(false);
        }
        for (int i = 0; i < tipList.Count; i++)
        {
            tipList[i].SetActive(false);
        }

        tipList[index].SetActive(true);
#endif
    }

    public void EnableTip()
    {
        audioPlayer.Stop();
#if UNITY_ANDROID
        for (int i = 0; i < markList.Count; i++)
        {
            markList[i].SetActive(true);
        }
        for (int i = 0; i < tipList.Count; i++)
        {
            tipList[i].SetActive(false);
        }
#endif
    }
}
