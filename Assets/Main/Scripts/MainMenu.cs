using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Network
    NetworkView networkview;
    #endregion

    [HideInInspector]
    public NetworkGlobal net;

#if UNITY_STANDALONE_WIN
    #region Course
    public Transform subjectListI;
    public Transform courseList;
    [HideInInspector]
    public List<Toggle> languageList;
    
    int curSubjectIIndex;//, curSubjectIIIndex;
    #endregion
#endif

    #region Camera Variable
    Vector3 mainPosition;
    Quaternion mainRotation;

#if UNITY_STANDALONE_WIN
    public Transform clientCamera;

    public Dropdown dropDown;
    [HideInInspector]
    public int clientCameraPos = 0;
#endif

#if UNITY_ANDROID
    [HideInInspector]
    public Transform vrRoot;
    [HideInInspector]
    public Transform vrHead;
#endif
    #endregion

    void Start()
    {
        networkview = GetComponent<NetworkView>();

#if UNITY_STANDALONE_WIN
        //set for surveillance cameras
        if (dropDown)
        {
            dropDown.captionText.text = (clientCameraPos + 1).ToString();
            if (clientCamera)
            {
                clientCamera.position = Camera.main.transform.position;
                clientCamera.rotation = Camera.main.transform.rotation;
            }
        }

        #region course
        net.GetComponent<CourseInformaticaStore>().Initial();
        //subjectIBack = subjectListI.GetChild(0).GetComponent<Image>().color;
        //subjectIIBack = subjectListII.GetChild(0).GetComponent<Image>().color;
        curSubjectIIndex = 0;// curSubjectIIIndex = 0;
        //subjectListI.GetChild(0).GetComponent<Image>().color = Color.white;
        //subjectListII.GetChild(0).GetComponent<Image>().color = Color.white;
        courseList.GetComponent<RectTransform>().sizeDelta = new Vector2(courseList.GetComponent<RectTransform>().sizeDelta.x, 
            Mathf.Max(840, 390 * Mathf.CeilToInt(net.GetComponent<CourseInformaticaStore>().codeList.Count * 1.0f / 3) + 30));
        #endregion
#endif

#if UNITY_ANDROID
        vrRoot = GameObject.Find("SvrCamera").transform;
        vrHead = vrRoot.Find("Head");
        mainPosition = vrRoot.transform.position;
        mainRotation = vrRoot.transform.rotation;
#endif
    }

    void Update()
    {
#if UNITY_ANDROID
        if (vrHead)
            AndroidCamera(vrHead.position, vrHead.rotation);
#endif
    }

    #region Quit Function
    public void QuitApplication()
    {
        Application.Quit();
    }
    #endregion

    #region Display
    public void DisplayPanel(GameObject playGo)
    {
        playGo.SetActive(true);
    }
    public void HidePanel(GameObject hideGo)
    {
        hideGo.SetActive(false);
    }
    #endregion

    #region Course
#if UNITY_STANDALONE_WIN
    public void CourseSubjectChooseI(int index)
    {
        if (index == curSubjectIIndex)
            return;
        curSubjectIIndex = index;
        CourseChoose(curSubjectIIndex * 100);
    }

    public void CourseChoose(int index)
    {
        int lgt = 0;
        if (index / 100 == 0)
        {
            for (int i = 1; i < courseList.childCount; i++)
            {
                courseList.GetChild(i).gameObject.SetActive(true);
            }
            lgt = courseList.childCount;
        }
        else
        {
            List<string> subjectL = net.GetComponent<CourseInformaticaStore>().subjectList;
            for (int i = 1; i < courseList.childCount; i++)
            {
                List<int> sub = new List<int>();
                for (int j = 0; j < subjectL[i - 1].Length; j += 4)
                {
                    sub.Add(int.Parse(subjectL[i - 1].Substring(j, 4)));
                }

                bool isshow = false;
                for (int k = 0; k < sub.Count; k++)
                {
                    Debug.Log(sub[k]);
                    if (sub[k] / 100 == index / 100)
                    {
                        isshow = true;
                    }

                    if (index % 100 != 0 && isshow)
                    {
                        isshow = false;
                        for (int l = 0; l < sub.Count; l++)
                        {
                            if (sub[l] % 100 == index % 100)
                            {
                                isshow = true;
                            }
                        }
                    }
                }

                if (isshow)
                {
                    courseList.GetChild(i).gameObject.SetActive(true);
                    lgt += 1;
                }
                else
                {
                    courseList.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        courseList.GetComponent<RectTransform>().sizeDelta = new Vector2(courseList.GetComponent<RectTransform>().sizeDelta.x,
            Mathf.Max(840, 390 * Mathf.CeilToInt(lgt * 1.0f / 3) + 30));
    }
#endif
    #endregion

    #region Change Language
    public void DisplayLanguage(int index)
    {
    }

    public string WordsChoose(int index)
    {
        return "";
    }
    #endregion

    #region For Loading another scene or class
    public void ChangeScene(string scenename)
    {
        networkview.RPC("RpcChangeSceneByName", RPCMode.AllBuffered, scenename);
    }
    [RPC]
    void RpcChangeSceneByName(string scenename, NetworkMessageInfo info)
    {
#if UNITY_STANDALONE_WIN
        SceneManager.LoadScene(scenename+""+"_Server");
#endif

#if UNITY_ANDROID
        SceneManager.LoadScene(scenename+""+"_Client");
#endif
    }
    #endregion

    #region For client to upload the positon and rotation of the camera
    public void AndroidCamera(Vector3 position, Quaternion rotation)
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            networkview.RPC("RpcAndroidCamera", RPCMode.All, int.Parse(Network.player.ipAddress.Split('.')[3]) - 100, position, rotation);
        }
    }
    [RPC]
    void RpcAndroidCamera(int index, Vector3 position, Quaternion rotation, NetworkMessageInfo info)
    {
#if UNITY_STANDALONE_WIN
        if (index == clientCameraPos && clientCamera)
        {
            clientCamera.position = position;
            clientCamera.rotation = rotation;
        }
#endif
    }
#endregion

    #region set which student to be viewed and monitored
    public void SetClientPos(int index)
    {
#if UNITY_STANDALONE_WIN
        clientCameraPos = index;
#endif
    }
    #endregion
}
