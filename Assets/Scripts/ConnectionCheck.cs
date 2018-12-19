using UnityEngine;

public class ConnectionCheck : MonoBehaviour {

    public GameObject start;
    public GameObject shutdown;

    bool previousState;

    private void Start()
    {
        ChangeState();
        previousState = UDPNetworking.isConnected;
    }

    void ChangeState()
    {
        start.SetActive(!UDPNetworking.isConnected);
        shutdown.SetActive(UDPNetworking.isConnected);
    }

    // Update is called once per frame
    void Update () {
        if (UDPNetworking.isConnected != previousState)
        {
            ChangeState();
        }
        previousState = UDPNetworking.isConnected;
    }
}
