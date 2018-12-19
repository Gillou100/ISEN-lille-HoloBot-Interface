using UnityEngine;
using UnityEngine.UI;

public abstract class UDPNetworkingBase : MonoBehaviour
{ 
    public Text LogField;
    public string externalIP;
    public int externalPort;

    public static string log = "";
    public static bool isConnected { get; protected set; }

    protected static string messageReceive;
    protected static string messageSend;

    protected void Start()
    {
        log += "IP: " + externalIP.ToString() + "\n";
        log += "Port: " + externalPort.ToString() + "\n";

        isConnected = false;
        LogField.text = "";

        messageReceive = "";
        messageSend = null;
    }

    protected void Update()
    {
        if(log != "")
        {
            LogField.text += log;
            log = "";
        }
    }

    public static void Connection()
    {
        log += "Fonction Connection() is not defined for this type of network";
    }

    public static void Disconnection()
    {
        log += "Fonction Disconnection() is not defined for this type of network";
    }

    public static void Socket_SendMessage(string message)
    {
        log += "Fonction Socket_SendMessage(string message) is not defined for this type of network";
    }
}