using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseInformaticaStore : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    public List<string> codeList = new List<string>();
    public List<string> nameList = new List<string>();
    public List<string> subjectList = new List<string>();
    public List<Sprite> imageList = new List<Sprite>();
    public List<bool> lockList = new List<bool>();

    public void Initial()
    {
        //InitialSubject();
        InitialCourse();
    }

    void InitialCourse()
    {
        Transform courseRoot = transform.GetComponent<NetworkGlobal>().menu.courseList;
        for (int i = 0; i < codeList.Count; i++)
        {
            Transform course = Object.Instantiate(courseRoot.GetChild(0));
            course.SetParent(courseRoot);
            course.gameObject.SetActive(true);
            course.name = codeList[i];
            course.FindChild("Course").GetComponent<Image>().sprite = imageList[i];
            course.FindChild("Text").GetComponent<Text>().text = nameList[i];
            if (lockList[i])
            {
                course.FindChild("Lock").gameObject.SetActive(true);
                course.GetComponent<Button>().interactable = false;
            }

            course.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                transform.GetComponent<NetworkGlobal>().menu.ChangeScene(course.name);
            });
        }
    }
#endif
}
