using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A0005_Menu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    #region Billboard;
    public TextMesh _Billboard;
    public List<string> _TipsList;
    #endregion

    #region Animation
    public Animation ani;
    int step = -1;
    public List<GameObject> details;
    bool isAni = false;
    public Transform _Glass;
    Vector3 glass0 = new Vector3(-18.30834f, 29.94403f, -17.10459f), glass1 = new Vector3(-0.2373582f, -0.3370998f, 1.571825f);
    #endregion

    #region Audio
    public AudioSource _AudioSourse;
    public List<AudioClip> _AudioClips;
    #endregion

    #region Camera
    public Transform _CamPosRoot;
    Transform target;
    bool isMove = false;
#if UNITY_ANDROID
    public Transform vrRoot;
#endif

#if UNITY_STANDALONE_WIN
    public Button priousBtn, nextBtn;
#endif
    #endregion

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Camera
        if (isMove)
        {
#if UNITY_STANDALONE_WIN
            Vector3 pos = Camera.main.transform.position;
            Vector3 targetpos = target.position;
            Quaternion rot = Camera.main.transform.rotation;
            Quaternion targetrot = target.rotation;
            if (Vector3.Distance(pos, targetpos) > 0.1f || Quaternion.Angle(rot, targetrot) > 0.5f)
            {
                Camera.main.transform.position = Vector3.Lerp(pos, targetpos, 0.01f);
                Camera.main.transform.rotation = Quaternion.Slerp(rot, targetrot, 0.01f);
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

            if (!isMove && isAni)
            {
                ani.Play("A0005_" + step.ToString());
            }
        }
        #endregion

        #region Animation
        else if (isAni)
        {
            if (!ani.isPlaying)
            {
                isAni = false;
                target = _CamPosRoot;
                isMove = true;
            }
        }
        #endregion
    }

    string HandleStringTab(string str)
    {
        string[] strlist = str.Split('+');
        str = strlist[0];
        if (strlist.Length > 1)
        {
            for (int i = 1; i < strlist.Length; i++)
            {
                str += '\n' + strlist[i];
            }
        }
        return str;
    }

    public void StepButton(int index)
    {
        networkview.RPC("RpcStepButton", RPCMode.All, index);
    }
    [RPC]
    void RpcStepButton(int index, NetworkMessageInfo info)
    {
        #region Detail
        if (step == 5)
        {
            details[0].SetActive(false);
        }
        else if (step == 10)
        {
            details[1].SetActive(false);
        }
        #endregion

        #region step
        switch (index)
        {
            case 0:
                step = 0;
                break;
            case 1:
                step = Mathf.Max(step - 1, 0);
                break;
            case 2:
                step = Mathf.Min(step + 1, 12);
                break;
            case 3:
                step = Mathf.Clamp(step, 0, 12);
                break;
        }
        #endregion

        if (step < 3)
        {
            _Glass.localPosition = glass0;
        }
        else
        {
            _Glass.localPosition = glass1;
        }

#if UNITY_STANDALONE_WIN
        if (step == 0)
        {
            priousBtn.interactable=false;
        }
        else
        {
            priousBtn.interactable = true;
        }

        if (step == 12)
        {
            nextBtn.interactable = false;
        }
        else
        {
            nextBtn.interactable = true;
        }
#endif

        _AudioSourse.clip = _AudioClips[step];
        _AudioSourse.Play();
        _Billboard.text = HandleStringTab(_TipsList[step]);

        target = _CamPosRoot.GetChild(step);
        isMove = true;
        isAni = true;
        #region Detail
        if (step == 5)
        {
            details[0].SetActive(true);
        }
        else if (step == 10)
        {
            details[1].SetActive(true);
        }
        #endregion
    }
}