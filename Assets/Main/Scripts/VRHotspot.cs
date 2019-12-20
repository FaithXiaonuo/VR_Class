using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHotspot : MonoBehaviour
{
#if UNITY_ANDROID
    public Camera vrheadleft;

    int curTarget = 0;
    int timeM = 0;
    public int _Speed = 5;
    
    public List<Transform> targetObjectList;
    public List<Texture> targetImageList;
    int imageLgt = 12;


    // Use this for initialization
    void Start()
    {
        imageLgt = targetImageList.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Trigger()
    {
        int answer = -1;

        Ray ray = vrheadleft.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2, 1 << LayerMask.NameToLayer("Target")))
        {
            if (hit.collider.gameObject.name != curTarget.ToString())
            {
                timeM = 0;

                targetObjectList[curTarget].GetComponent<Renderer>().material.mainTexture = targetImageList[0];
                curTarget = int.Parse(hit.collider.gameObject.name);

            }
            else
            {
                if (timeM == imageLgt * _Speed - 1)
                {
                    answer = curTarget;
                }

                timeM = Mathf.Min(timeM + 1, imageLgt * _Speed);
            }
            Debug.Log(timeM / _Speed);
            targetObjectList[curTarget].GetComponent<Renderer>().material.mainTexture = targetImageList[timeM / _Speed];
        }
        else
        {
            if (timeM != 0)
            {
                targetObjectList[curTarget].GetComponent<Renderer>().material.mainTexture = targetImageList[0];
            }
            timeM = 0;
        }
        return answer;
    }
#endif
}
