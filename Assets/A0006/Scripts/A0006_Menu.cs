using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0006_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public AudioSource _AudioListener;
    public List<AudioClip> _AudioClips;

    public Transform _ModelRoot;

    bool isRotate;
    int target;

#if UNITY_ANDROID
    VRHotspot hotspot;
#endif

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
#if UNITY_ANDROID
        hotspot = GetComponent<VRHotspot>();
#endif
    }

    private void Update()
    {
        if (isRotate)
        {
            _ModelRoot.localEulerAngles = new Vector3(
                _ModelRoot.localEulerAngles.x,
                Mathf.Lerp(_ModelRoot.localEulerAngles.y, target, Time.deltaTime),
                _ModelRoot.localEulerAngles.z);
            if (Mathf.Abs(_ModelRoot.localEulerAngles.y - target) < 1)
            {
                isRotate = true;
            }
        }

#if UNITY_ANDROID
        int trigger = hotspot.Trigger();
        if (trigger >= 0)
        {
            _AudioListener.clip = _AudioClips[trigger];
            _AudioListener.Play();
        }
#endif
    }

    public void ChooseModel(int index)
    {
        networkview.RPC("RpcChooseModel", RPCMode.All, index);
    }
    [RPC]
    void RpcChooseModel(int index, NetworkMessageInfo info)
    {
       
            switch (index)
            {
                case 0:
                    target = 0;
                    break;
                case 1:
                    target = 270;
                    break;
                case 2:
                    target = 135;
                    break;
        }
        _AudioListener.clip = _AudioClips[index];
        _AudioListener.Play();

        isRotate = true;
    }
}
