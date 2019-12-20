using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StudentInformation : MonoBehaviour
{
    OpenFileName pth = new OpenFileName();
    public Transform _StudentList;
    MainMenu menu;

    void Start()
    {
        menu = GetComponent<MainMenu>();
    }

    #region StudentList Function
    public void UpdateStudentList()
    {
        List<string> studentList = menu.net.GetComponent<GlobalStore>().StudentList;

        if (studentList.Count>0)
        {
            HandleListToWords(studentList);
        }
        else
        {
            LoadStudentList();
        }
    }

    public void LoadStudentList()
    {
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "txt (*.txt)";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        pth.title = "标题";
        pth.defExt = "txt";
        pth.flags = 0x00000008;
        //pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (DllTest.GetOpenFileName(pth))
        {
            StartCoroutine(WaitLoad(pth.file));
            Debug.Log("Selected file with full path: {0}" + pth.file);
        }
        else
        {
            LoadStudentList();
        }
    }

    IEnumerator WaitLoad(string fileName)
    {
        StreamReader sr = File.OpenText(fileName);
        string line;
        List<string> arrlist = new List<string>();
        menu.net.GetComponent<GlobalStore>().StudentList = arrlist;
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);
        }
        sr.Close();
        sr.Dispose();

        HandleListToWords(arrlist);
        yield return true;
    }

    void HandleListToWords(List<string> arrlist)
    {
        for (int i = 1; i < arrlist.Count && i <= 30; i++)
        {
            string[] word = arrlist[i].Split(',');
            Transform stu = _StudentList.GetChild(i - 1);
            stu.Find("Num").GetComponent<Text>().text = word[0];
            stu.Find("Name").GetComponent<Text>().text = word[1];
            stu.Find("Sex").GetComponent<Text>().text = word[2];
            stu.Find("Seat").GetComponent<Text>().text = word[3];
        }
    }
    #endregion
}
