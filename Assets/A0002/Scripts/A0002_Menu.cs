using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0002_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public AudioSource _AudioSource;
    public Animation flight;
    public List<AudioClip> details;
    public AudioSource _BGM;
    bool startbigin = false;
    float time = 0;
    Vector3 rigionPos;
    Quaternion rigionRot;

#if UNITY_ANDROID
    public Transform vrRoot;
#endif


    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
        rigionPos = flight.transform.position;
        rigionRot = flight.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (startbigin)
        {
            time += Time.deltaTime;
            if (time > 130f)
            {
                _AudioSource.clip = details[5];
                _AudioSource.Play();
                startbigin = false;
            }
        }
    }

    public void StartAni()
    {
        networkview.RPC("RpcAircraftStartAni", RPCMode.All, 0);
    }
    [RPC]
    void RpcAircraftStartAni(int index, NetworkMessageInfo info)
    {
        _BGM.Play();
        flight.Play("A0002_Flight");
        startbigin = true;
        time = 0;

        _AudioSource.clip = details[4];
        _AudioSource.Play();

#if UNITY_STANDALONE_WIN
        Camera.main.transform.position = transform.GetComponent<MoveCameraToFocus>().CameraPos[0].position;
        Camera.main.transform.rotation = transform.GetComponent<MoveCameraToFocus>().CameraPos[0].rotation;
#endif

#if UNITY_ANDROID
        vrRoot.transform.position = transform.GetComponent<MoveCameraToFocus>().CameraPos[0].position;
#endif
    }

    public void StopAni(int index)
    {
        networkview.RPC("RpcAircraftStopAni", RPCMode.All, index);
    }
    [RPC]
    void RpcAircraftStopAni(int index, NetworkMessageInfo info)
    {
        if (index == 1)
        {
            flight.Play("A0002_Detail");
        }
        else
        {
            flight.Play("A0002_Stop");
        }
        _BGM.Stop();
        flight.transform.position = rigionPos;
        flight.transform.rotation = rigionRot;
        startbigin = false;

        _AudioSource.clip = details[index];
        _AudioSource.Play();
        transform.GetComponent<MoveCameraToFocus>().SetCamera(index);
    }
}
