/*
 * Here is the network class which is really used. it is a UDPNetworkingUnity or UDPNetworkingHoloLens subclass depending on the context
 * 
 * To know why we have both HoloLens and Unity network class, please read the documentation. (Warning: the documentaion is in french.)
 * 
 * This class is not abstract because we need this script with a Unity object (Otherwise, her Start method won't run).
 * 
 * This class describe how the network will work, what it have to do.
 */

#if UNITY_EDITOR
public class UDPNetworking : UDPNetworkingUnity
#else
public class UDPNetworking : UDPNetworkingHoloLens
#endif
{
    private static int tentatives = 0;  // After sending a message, we count the number of message received which are different from the sent to know if we receive an echo

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
            /*
             * Here are tests to know handle the message received.
             */
            if (messageReceived == messageSent)
            {
                log += "echo réussit : " + messageSent + "\n";
                messageSent = null;
                messageReceived = "";
                tentatives = 0;
            }
            else if(messageReceived != "")
            {
                log += "Message receive : " + messageReceived + "\n";
                if (messageSent != null)
                {
                    tentatives++;
                }
                messageReceived = "";
            }
            if (tentatives == 3)
            {
                log += "echo échoué : " + messageSent + "\n";
                tentatives = 0;
                messageSent = null;
            }
            if (messageReceived == "endC")
            {
                Disconnection();
            }
        }
    }
}
