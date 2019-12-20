using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0001_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public AudioSource _AudioSource;
    public Animation _Earth, _Moon, _Satellite;
    public GameObject _SIdle;
    public List<AudioClip> details;
    public AudioSource _BGM;
    bool startbigin = false;
    float time = 0;
    List<Vector3> rigionPos = new List<Vector3>();
    List<Quaternion> rigionRot = new List<Quaternion>();

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
        rigionPos.Add(_Earth.transform.position);
        rigionRot.Add(_Earth.transform.rotation);
        rigionPos.Add(_Moon.transform.position);
        rigionRot.Add(_Moon.transform.rotation);
        rigionPos.Add(_Satellite.transform.position);
        rigionRot.Add(_Satellite.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAni()
    {
        networkview.RPC("RpcAircraftStartAni", RPCMode.All, 0);
    }
    [RPC]
    void RpcAircraftStartAni(int index, NetworkMessageInfo info)
    {
        _SIdle.SetActive(false);
        _Satellite.gameObject.SetActive(true);

        _BGM.Play();
        _Earth.Play();
        _Moon.Play();
        _Satellite.Play();
        startbigin = true;
        time = 0;

        transform.GetComponent<MoveCameraToFocus>().SetCamera(0);
    }

    public void StopAni(int index)
    {
        networkview.RPC("RpcAircraftStopAni", RPCMode.All, index);
    }
    [RPC]
    void RpcAircraftStopAni(int index, NetworkMessageInfo info)
    {
        _Earth.Stop();
        _Moon.Stop();
        _Satellite.Stop();
        _SIdle.SetActive(true);
        _Satellite.gameObject.SetActive(false);
        _BGM.Stop();
        startbigin = false;

        _AudioSource.clip = details[index];
        _AudioSource.Play();
        transform.GetComponent<MoveCameraToFocus>().SetCamera(index + 1);
    }
}
