using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class UDPNetworking : UDPNetworkingUnity
#else
public class UDPNetworking : UDPNetworkingHoloLens
#endif
{
    int tentatives = 0;

    new protected void Start()
    {
        base.Start();
        Connection();
    }

    new private void Update()
    {
        base.Update();
        if(isConnected)
        {
            if (messageReceive == messageSend)
            {
                LogField.text = "echo réussit : " + messageSend + "\n";
                messageSend = null;
                messageReceive = "";
                tentatives = 0;
            }
            else if(messageReceive != "")
            {
                log += "Message receive : " + messageReceive + "\n";
                if (messageSend != null)
                {
                    tentatives++;
                }
                messageReceive = "";
            }
            if (tentatives == 3)
            {
                log += "echo échoué : " + messageSend + "\n";
                tentatives = 0;
                messageSend = null;
            }
            if (messageReceive == "endC")
            {
                Disconnection();
            }
        }
    }
}
