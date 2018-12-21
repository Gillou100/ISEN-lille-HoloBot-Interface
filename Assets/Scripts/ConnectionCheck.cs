using UnityEngine;


/*
 * Here is the script which exchange the connection button and the disconnection button according to the variable isConnected.
 */

public class ConnectionCheck : MonoBehaviour
{
    public GameObject connectionButton;
    public GameObject disconnectionButton;

    bool previousState;

    private void Start()
    {
        previousState = UDPNetworking.isConnected;
        ChangeState();
    }

    void ChangeState()
    {
        connectionButton.SetActive(!UDPNetworking.isConnected);
        disconnectionButton.SetActive(UDPNetworking.isConnected);
    }
    
    void Update ()
    {
        if (UDPNetworking.isConnected != previousState)
        {
            ChangeState();
            previousState = UDPNetworking.isConnected;
        }
    }
}
