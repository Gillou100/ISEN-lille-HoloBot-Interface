using UnityEngine;
using UnityEngine.UI;


/*
 * Here is the super class for the two network types we create.
 * 
 * To know why we have both HoloLens and Unity network types, please read the documentation. (Warning: the documentaion is in french.)
 * 
 * This class is abstract for two reasons:
 *      it is useless without a subclass.
 *      We decide to not create network object, but to use directly the class and these class variable or method because having severals network is nonsense.
 *  So all methods and variables must be static to use them but the first three variable because we need to access them from Unity and the log which can be used by any script.
 *  
 *  This class lay the foundations to create the network subclasses. (retrieval the IP and port, create a log, and so on...)
 */

public abstract class UDPNetworkingBase : MonoBehaviour
{ 
    public Text LogField;                                       // Text field use for debug.
    public string externalIP;                                   // IP put in Unity.
    public int externalPort;                                    // Port put in Unity.

    public static string log = "";                              // Text which will appear in LogField.
    public static bool isConnected { get; protected set; }      // Variable use to communicate if the device is connected or not between all scripts.

    protected static string messageReceived;                    // Memory of the last message received.
    protected static string messageSent;                        // Memory of the last message sent.


    protected void Start()
    {
        log += "IP: " + externalIP.ToString() + "\n";
        log += "Port: " + externalPort.ToString() + "\n";

        isConnected = false;
        LogField.text = "";

        messageReceived = "";
        messageSent = null;
    }

    protected void Update()
    {   // In each update, the script will add the log string in the log field and clean the log string.
        if (log != "")
        {
            LogField.text += log;
            log = "";
        }
    }

    /*
     * These following methods are create here as debug if a programmer forget to override them in a subclass.
     * These is required because a method can not be both static and abstract.
     */
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
        log += "Fonction Socket_SendMessage(string) is not defined for this type of network";
    }

    public static void Socket_MessageReceived()
    {
        log += "Fonction Socket_MessageReceived() is not defined for this type of network";
    }
}