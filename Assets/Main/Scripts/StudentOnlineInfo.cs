using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentOnlineInfo : MonoBehaviour
{
    List<Text> onlineText = new List<Text>();
    List<Image> onlinePict = new List<Image>();
    public Sprite _OnLinePict, _OffLinePict;

    private void OnEnable()
    {
        
    }
    
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            onlineText.Add(transform.GetChild(i).FindChild("Online").GetComponent<Text>());
            onlinePict.Add(transform.GetChild(i).FindChild("OnlineImage").GetComponent<Image>());
        }
    }

    void Update()
    {
        for (int i = 0; i < onlineText.Count; i++)
        {
            onlineText[i].text = "OFF";
            onlinePict[i].sprite = _OffLinePict;
        }

        int length = Network.connections.Length;
        for (int i = 0; i < length; i++)
        {
            int num = int.Parse(Network.connections[i].ipAddress.Split('.')[3]) - 100;
            onlineText[num].text = "ON";
            onlinePict[num].sprite = _OnLinePict;
        }
    }
}
