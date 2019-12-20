using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0004_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    public Transform _TWSoldierRoot;
    public List<AudioClip> audioClips;
    public AudioSource audioPlay;
    public List<Transform> _Soldier;

    bool isRotate;
    int target;

#if UNITY_ANDROID
    public List<Texture> targetImageList;
    public List<Material> targetMaterialList;
   public  Camera vrheadleft;
    int curTarget = 0;
    int timeM = 0;
#endif

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();

#if UNITY_ANDROID
        curTarget = 0;

        for (int i = 0; i < targetMaterialList.Count; i++)
        {
            targetMaterialList[i].mainTexture = targetImageList[0];
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (_TWSoldierRoot.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < _Soldier.Count; i++)
            {
                _Soldier[i].eulerAngles += new Vector3(0, 0.5f, 0);
            }
        }

        if (isRotate)
        {
            _TWSoldierRoot.localEulerAngles = new Vector3(
                _TWSoldierRoot.localEulerAngles.x,
                Mathf.Lerp(_TWSoldierRoot.localEulerAngles.y, target * 60, Time.deltaTime),
                _TWSoldierRoot.localEulerAngles.z);
            if (Mathf.Abs(_TWSoldierRoot.localEulerAngles.y - target * 60) < 1)
            {
                isRotate = true;
            }
        }

#if UNITY_ANDROID
        Ray ray = vrheadleft.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Target")))
        {
            if (hit.collider.gameObject.name != curTarget.ToString())
            {
                timeM = 0;
                curTarget = int.Parse(hit.collider.gameObject.name);
            }
            else
            {
                if (timeM == 12 * 5 - 1)
                {
                    targetMaterialList[curTarget].mainTexture = null;
                    audioPlay.clip = audioClips[curTarget];
                    audioPlay.Play();
                }

                timeM = Mathf.Min(timeM + 1, 12 * 5);
            }
            targetMaterialList[curTarget].mainTexture = targetImageList[timeM / 5];
        }
        else
        {
            timeM = 0;
            targetMaterialList[curTarget].mainTexture = targetImageList[0];
        }

#endif
    }

public void ChooseSoldier(int index)
    {
        networkview.RPC("RpcChooseSoldier", RPCMode.All, index);
    }
    [RPC]
    void RpcChooseSoldier(int index, NetworkMessageInfo info)
    {
        audioPlay.clip = audioClips[index];
        audioPlay.Play();

        target = index;
        isRotate = true;

#if UNITY_ANDROID
        curTarget = index;
#endif
}

void SetSoldierRot()
    {
        for (int i = 0; i < _Soldier.Count; i++)
        {
            _Soldier[i].eulerAngles = Vector3.zero;
        }
    }
}

