using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A0011_Controll : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    #region Resource
    public Transform _Body, _WholeHeart,
        _HalfHeart, _HalfHeart1, _HalfHeart2,
        _Operation1, _Operation2, _Operation3,
        _Billboard1, _Billboard2, _Billboard3;
    public List<Transform> labelList1, labelList2, arrowList1, arrowList2, arrowList3;

    public List<AudioClip> _AudioList;
    public AudioSource _AudioSourse_Tip;
    public AudioSource _AudioSourse_HeartBeat;
    public AudioSource _AudioSourse_HeartBeatSlow;

#if UNITY_STANDALONE_WIN
    public List<Button> _ButtonList;
#endif
#endregion

    /// <summary>
    /// 0 whole body，
    /// 1 whole heart, 2 half heart 1, 3 half heart 2
    /// 4 contraction, 5 diastole, 6 whole diastole
    /// </summary>
    int curStep = 0;
    int targetStep = 0;
    /// <summary>
    /// 0 enter, 1 excute, 2 exit
    /// </summary>
    int State = 1;
    bool getExit = false;

    float timeAC = 0;
    bool wholeBodyLeft = true;

#region 0
    /// <summary>
    /// 0 little, 1 little to large, 2 large
    /// </summary>
    int enterDetail = 0;
    Vector3 wholeLittlePos = new Vector3(-0.005f, 0.366f, -0.082f);
    Vector3 wholeLargePos = new Vector3(0, 0.2f, 0.5f);
    Vector3 wholeLittleScale = new Vector3(1, 1, 1);
    Vector3 wholeLargeScale = new Vector3(2f, 2f, 2f);
#endregion

#region 1
    Quaternion halfHeartAngle;
    int labelShake = 0;
    bool canGoExit = false;
#endregion

    // Use this for initialization
    void Start()
    {
        networkview = GetComponent<NetworkView>();

        SetButtonInteractable(false);
        halfHeartAngle = _HalfHeart.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case 0:
                EnterStep();
                break;

            case 1:
                ExcuteStep();
                break;

            case 2:
                ExitStep();
                break;
        }
    }

    public void ButtonChangeStep(int index)
    {
        networkview.RPC("RpcButtonChangeStep", RPCMode.All, index);
    }
    [RPC]
    void RpcButtonChangeStep(int index, NetworkMessageInfo info)
    {
        if (targetStep != index)
        {
            targetStep = index;
            getExit = true;
            SetButtonInteractable(false);
        }
    }

    void SetButtonInteractable(bool active)
    {
#if UNITY_STANDALONE_WIN
        if (active)
        {
            switch (curStep)
            {
                case 0:
                    _ButtonList[1].gameObject.SetActive(true);
                    break;
                case 1:
                    _ButtonList[0].gameObject.SetActive(true);
                    _ButtonList[2].gameObject.SetActive(true);
                    _ButtonList[3].gameObject.SetActive(true);
                    _ButtonList[4].gameObject.SetActive(true);
                    _ButtonList[5].gameObject.SetActive(true);
                    _ButtonList[6].gameObject.SetActive(true);
                    break;
                case 2:
                    _ButtonList[1].gameObject.SetActive(true);
                    _ButtonList[3].gameObject.SetActive(true);
                    break;
                case 3:
                    _ButtonList[1].gameObject.SetActive(true);
                    _ButtonList[2].gameObject.SetActive(true);
                    break;
                case 4:
                    _ButtonList[1].gameObject.SetActive(true);
                    _ButtonList[5].gameObject.SetActive(true);
                    _ButtonList[6].gameObject.SetActive(true);
                    break;
                case 5:
                    _ButtonList[1].gameObject.SetActive(true);
                    _ButtonList[4].gameObject.SetActive(true);
                    _ButtonList[6].gameObject.SetActive(true);
                    break;
                case 6:
                    _ButtonList[1].gameObject.SetActive(true);
                    _ButtonList[4].gameObject.SetActive(true);
                    _ButtonList[5].gameObject.SetActive(true);
                    break;
            }
        }
        else
        {
            for (int i = 0; i < _ButtonList.Count; i++)
            {
                _ButtonList[i].gameObject.SetActive(false);
            }
        }
#endif
    }

#region EnterStep
    void EnterStep()
    {
        timeAC = 0;

        switch (curStep)
        {
#region 0
            case 0:
                if (enterDetail == 2)
                {
                    if (_WholeHeart.position.y < wholeLargePos.y)
                    {
                        _WholeHeart.position = _WholeHeart.position + new Vector3(0, 0.005f, 0);
                    }
                    else
                    {
                        _HalfHeart.gameObject.SetActive(false);
                        _Body.gameObject.SetActive(true);
                        enterDetail = 1;
                    }
                }
                else if (enterDetail == 1)
                {
                    if (Vector3.Distance(_WholeHeart.position, wholeLittlePos) > 0.001f)
                    {
                        _WholeHeart.position = Vector3.MoveTowards(_WholeHeart.position, wholeLittlePos, 0.005f);
                        if (_WholeHeart.localScale.x > wholeLittleScale.y)
                        {
                            _WholeHeart.localScale = Vector3.MoveTowards(_WholeHeart.localScale, wholeLittleScale, 0.05f);
                        }
                    }
                    else
                    {
                        enterDetail = 0;
                        State = 1;
                        _WholeHeart.gameObject.SetActive(false);
                    }
                }
                break;
#endregion

#region 1
            case 1:
                State = 1;
                SetButtonInteractable(true);
                _HalfHeart.gameObject.SetActive(true);
                _HalfHeart.rotation = halfHeartAngle;
                break;
#endregion

#region 2
            case 2:
                State = 1;
                timeAC = 3;
                canGoExit = false;
                labelShake = 0;
                _HalfHeart1.gameObject.SetActive(true);
                _HalfHeart1.rotation = halfHeartAngle;
                for (int i = 0; i < labelList1.Count; i++)
                {
                    labelList1[i].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                }
                break;
#endregion

#region 3
            case 3:
                State = 1;
                timeAC = 3;
                canGoExit = false;
                labelShake = 0;
                _HalfHeart2.gameObject.SetActive(true);
                _HalfHeart2.rotation = halfHeartAngle;
                for (int i = 0; i < labelList2.Count; i++)
                {
                    labelList2[i].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                }
                break;
#endregion

#region 4
            case 4:
                State = 1;
                timeAC = 3;
                canGoExit = false;

                _Operation1.gameObject.SetActive(true);
                _Billboard1.gameObject.SetActive(true);
                _AudioSourse_Tip.clip = _AudioList[1];
                _AudioSourse_Tip.Play();

                for (int i = 0; i < arrowList1.Count; i++)
                {
                    arrowList1[i].GetComponent<A0011_Arrow>().SetStart();
                }
                break;
#endregion

#region 5
            case 5:
                State = 1;
                timeAC = 3;
                canGoExit = false;

                _Operation2.gameObject.SetActive(true);
                _Billboard2.gameObject.SetActive(true);
                _AudioSourse_Tip.clip = _AudioList[2];
                _AudioSourse_Tip.Play();

                for (int i = 0; i < arrowList2.Count; i++)
                {
                    arrowList2[i].GetComponent<A0011_Arrow>().SetStart();
                }
                break;
#endregion

#region 6
            case 6:
                State = 1;
                timeAC = 3;
                canGoExit = false;

                _Operation3.gameObject.SetActive(true);
                _Billboard3.gameObject.SetActive(true);
                _AudioSourse_Tip.clip = _AudioList[3];
                _AudioSourse_Tip.Play();

                for (int i = 0; i < arrowList3.Count; i++)
                {
                    arrowList3[i].GetComponent<A0011_Arrow>().SetStart();
                }
                break;
#endregion
        }
    }
#endregion

#region ExcuteStep
    void ExcuteStep()
    {
        switch (curStep)
        {
#region 0
            case 0:
                if (enterDetail == 0)
                {
                    timeAC += Time.deltaTime;

                    if (wholeBodyLeft)
                    {
                        if (_Body.eulerAngles.y < 180 && _Body.eulerAngles.y > 30)
                        {
                            wholeBodyLeft = false;
                        }
                        else
                        {
                            _Body.Rotate(Vector3.up, 0.2f);
                        }
                    }
                    else
                    {
                        if (_Body.eulerAngles.y > 180 && _Body.eulerAngles.y < 330)
                        {
                            wholeBodyLeft = true;
                        }
                        else
                        {
                            _Body.Rotate(Vector3.up, -0.2f);
                        }
                    }
                    if (timeAC > 3 && Mathf.Abs(_Body.eulerAngles.y) < 0.1f)
                    {
                        enterDetail = 1;
                        _WholeHeart.gameObject.SetActive(true);
                        _WholeHeart.position = wholeLittlePos;
                        _WholeHeart.eulerAngles = Vector3.zero;

                        _AudioSourse_Tip.clip = _AudioList[0];
                        _AudioSourse_Tip.Play();
                    }
                }
                else if (enterDetail == 1)
                {
                    if (Vector3.Distance(_WholeHeart.position, wholeLargePos) > 0.001f)
                    {
                        _WholeHeart.position = Vector3.MoveTowards(_WholeHeart.position, wholeLargePos, 0.005f);
                        if (_WholeHeart.localScale.x < wholeLargeScale.y)
                        {
                            _WholeHeart.localScale = Vector3.MoveTowards(_WholeHeart.localScale, wholeLargeScale, 0.05f);
                        }
                    }
                    else
                    {
                        enterDetail = 2;
                        SetButtonInteractable(true);
                    }
                }
                else
                {
                    if (wholeBodyLeft)
                    {
                        if (_WholeHeart.eulerAngles.y < 180 && _WholeHeart.eulerAngles.y > 20)
                        {
                            wholeBodyLeft = false;
                        }
                        else
                        {
                            _WholeHeart.Rotate(Vector3.up, 0.2f);
                        }
                    }
                    else
                    {
                        if (_WholeHeart.eulerAngles.y > 180 && _WholeHeart.eulerAngles.y < 340)
                        {
                            wholeBodyLeft = true;
                        }
                        else
                        {
                            _WholeHeart.Rotate(Vector3.up, -0.2f);
                        }
                    }
                    if (getExit && Mathf.Abs(_WholeHeart.eulerAngles.y) < 0.1f)
                    {
                        State = 2;
                        _HalfHeart.gameObject.SetActive(true);
                        _Body.gameObject.SetActive(false);
                        _WholeHeart.GetComponent<Animation>().Stop();
                        getExit = false;
                    }
                }
                break;
#endregion

#region 1
            case 1:
                if (wholeBodyLeft)
                {
                    if (_HalfHeart.eulerAngles.y < 180 && _HalfHeart.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _HalfHeart.Rotate(new Vector3(0, 1, 0.5f), 0.5f);
                    }
                }
                else
                {
                    if (_HalfHeart.eulerAngles.y > 180 && _HalfHeart.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _HalfHeart.Rotate(new Vector3(0, 1, 0.5f), -0.5f);
                    }
                }
                if (getExit && !(targetStep == 0 && Mathf.Abs(_HalfHeart.eulerAngles.y) > 0.1f))
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion

#region 2
            case 2:
                if (wholeBodyLeft)
                {
                    if (_HalfHeart1.eulerAngles.y < 180 && _HalfHeart1.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _HalfHeart1.Rotate(new Vector3(0, 1, 0.5f), 0.5f);
                    }
                }
                else
                {
                    if (_HalfHeart1.eulerAngles.y > 180 && _HalfHeart1.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _HalfHeart1.Rotate(new Vector3(0, 1, 0.5f), -0.5f);
                    }
                }

                timeAC += Time.deltaTime;
                if (timeAC > 2)
                {
                    timeAC = 0;
                    labelList1[labelShake].localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    if (labelShake == 0)
                    {
                        labelList1[labelList1.Count - 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    }
                    else
                    {
                        labelList1[labelShake - 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    }
                    labelShake++;
                    if (labelShake == labelList1.Count)
                    {
                        labelShake = 0;
                        if (!canGoExit)
                        {
                            SetButtonInteractable(true);
                            canGoExit = true;
                        }
                    }
                }
                if (getExit)
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion

#region 3
            case 3:
                if (wholeBodyLeft)
                {
                    if (_HalfHeart2.eulerAngles.y < 180 && _HalfHeart2.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _HalfHeart2.Rotate(new Vector3(0, 1, 0.5f), 0.5f);
                    }
                }
                else
                {
                    if (_HalfHeart2.eulerAngles.y > 180 && _HalfHeart2.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _HalfHeart2.Rotate(new Vector3(0, 1, 0.5f), -0.5f);
                    }
                }

                timeAC += Time.deltaTime;
                if (timeAC > 2)
                {
                    timeAC = 0;
                    if (labelShake > 4)
                    {
                        labelList2[labelShake].localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        labelList2[labelShake + 1].localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    else
                    {
                        labelList2[labelShake].localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    if (labelShake == 0)
                    {
                        labelList2[labelList2.Count - 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                        labelList2[labelList2.Count - 2].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    }
                    else if (labelShake > 5)
                    {
                        labelList2[labelShake - 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                        labelList2[labelShake - 2].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    }
                    else
                    {
                        labelList2[labelShake - 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    }
                    if (labelShake > 4)
                    {
                        labelShake += 2;
                    }
                    else
                    {
                        labelShake++;
                    }
                    if (labelShake == labelList2.Count)
                    {
                        labelShake = 0;
                        if (!canGoExit)
                        {
                            SetButtonInteractable(true);
                            canGoExit = true;
                        }
                    }
                }
                if (getExit)
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion

#region 4
            case 4:
                if (wholeBodyLeft)
                {
                    if (_Operation1.eulerAngles.y < 180 && _Operation1.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _Operation1.Rotate(new Vector3(0, 1, 0), 0.1f);
                    }
                }
                else
                {
                    if (_Operation1.eulerAngles.y > 180 && _Operation1.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _Operation1.Rotate(new Vector3(0, 1, 0), -0.1f);
                    }
                }

                timeAC += Time.deltaTime;
                if (timeAC > 3)
                {
                    timeAC = 0;
                    _Operation1.GetComponent<Animation>().Play();
                    _AudioSourse_HeartBeatSlow.Play();
                    if (!canGoExit)
                    {
                        SetButtonInteractable(true);
                        canGoExit = true;
                    }

                    for (int i = 0; i < arrowList1.Count; i++)
                    {
                        arrowList1[i].GetComponent<A0011_Arrow>().SetStart();
                    }
                }
                if (getExit)
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion

#region 5
            case 5:
                if (wholeBodyLeft)
                {
                    if (_Operation2.eulerAngles.y < 180 && _Operation2.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _Operation2.Rotate(new Vector3(0, 1, 0), 0.1f);
                    }
                }
                else
                {
                    if (_Operation2.eulerAngles.y > 180 && _Operation2.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _Operation2.Rotate(new Vector3(0, 1, 0), -0.1f);
                    }
                }

                timeAC += Time.deltaTime;
                if (timeAC > 3)
                {
                    timeAC = 0;
                    _Operation2.GetComponent<Animation>().Play();
                    _AudioSourse_HeartBeatSlow.Play();
                    if (!canGoExit)
                    {
                        SetButtonInteractable(true);
                        canGoExit = true;
                    }

                    for (int i = 0; i < arrowList2.Count; i++)
                    {
                        arrowList2[i].GetComponent<A0011_Arrow>().SetStart();
                    }
                }
                if (getExit)
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion

#region 6
            case 6:
                if (wholeBodyLeft)
                {
                    if (_Operation3.eulerAngles.y < 180 && _Operation3.eulerAngles.y > 30)
                    {
                        wholeBodyLeft = false;
                    }
                    else
                    {
                        _Operation3.Rotate(new Vector3(0, 1, 0), 0.1f);
                    }
                }
                else
                {
                    if (_Operation3.eulerAngles.y > 180 && _Operation3.eulerAngles.y < 330)
                    {
                        wholeBodyLeft = true;
                    }
                    else
                    {
                        _Operation3.Rotate(new Vector3(0, 1, 0), -0.1f);
                    }
                }

                timeAC += Time.deltaTime;
                if (timeAC > 3)
                {
                    timeAC = 0;
                    _Operation3.GetComponent<Animation>().Play();
                    _AudioSourse_HeartBeatSlow.Play();
                    if (!canGoExit)
                    {
                        SetButtonInteractable(true);
                        canGoExit = true;
                    }

                    for (int i = 0; i < arrowList3.Count; i++)
                    {
                        arrowList3[i].GetComponent<A0011_Arrow>().SetStart();
                    }
                }
                if (getExit)
                {
                    State = 2;
                    getExit = false;
                }
                break;
#endregion
        }
    }
#endregion

#region ExitStep
    void ExitStep()
    {
        switch (curStep)
        {
#region 0
            case 0:
                if (_WholeHeart.position.y > 0)
                {
                    _WholeHeart.position = _WholeHeart.position - new Vector3(0, 0.005f, 0);
                }
                else
                {
                    _WholeHeart.gameObject.SetActive(false);
                    curStep = targetStep;
                    State = 0;
                }
                break;
#endregion

#region 1
            case 1:
                curStep = targetStep;
                State = 0;

                if (curStep == 0)
                {
                    _WholeHeart.gameObject.SetActive(true);
                    _WholeHeart.GetComponent<Animation>().Stop();
                }
                else
                {
                    _HalfHeart.gameObject.SetActive(false);
                }
                break;
#endregion

#region 2
            case 2:
                curStep = targetStep;
                State = 0;

                labelList1[labelShake].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                _HalfHeart1.gameObject.SetActive(false);
                break;
#endregion

#region 3
            case 3:
                curStep = targetStep;
                State = 0;

                if (labelShake > 4)
                {
                    labelList2[labelShake].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    labelList2[labelShake + 1].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                }
                else
                {
                    labelList2[labelShake].localScale = new Vector3(0.06f, 0.06f, 0.06f);
                }
                _HalfHeart2.gameObject.SetActive(false);
                break;
#endregion

#region 4
            case 4:
                curStep = targetStep;
                State = 0;
                _Operation1.gameObject.SetActive(false);
                _Billboard1.gameObject.SetActive(false);
                break;
#endregion

#region 5
            case 5:
                curStep = targetStep;
                State = 0;
                _Operation2.gameObject.SetActive(false);
                _Billboard2.gameObject.SetActive(false);
                break;
#endregion

#region 6
            case 6:
                curStep = targetStep;
                State = 0;
                _Operation3.gameObject.SetActive(false);
                _Billboard3.gameObject.SetActive(false);
                break;
#endregion
        }
    }
#endregion
}
