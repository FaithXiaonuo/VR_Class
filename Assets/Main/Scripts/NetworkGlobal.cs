using UnityEngine;

public class NetworkGlobal : MonoBehaviour
{
    public MainMenu menu;

    private int serverPort = 2424;
    private string serverIP = "192.168.1.100";
    private bool useNAT = false;
    private int limitUserCount = 30;
    private NetworkView view;
    //public bool isClient;
    

    void Start()
    {
        CreateServer();
    }

    void Update()
    {

    }

    void OnApplicationQuit()
    {
        Network.Disconnect();
    }

    public void CreateServer()
    {
#if UNITY_STANDALONE_WIN
        Network.InitializeServer(limitUserCount, serverPort, useNAT);
#endif

#if UNITY_ANDROID
        Network.Connect(serverIP, serverPort);
#endif
    }

    void OnFailedToConnect()
    {
        CreateServer();
    }

    void OnDisconnectedFromServer()
    {
        Application.Quit();
    }
}
