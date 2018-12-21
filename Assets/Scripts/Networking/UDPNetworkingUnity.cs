#if UNITY_EDITOR
using System;
using System.Threading;
using System.Text;

using System.Net;
using System.Net.Sockets;


/*
 * Here is the Unity network class.
 * 
 * To know why we have both HoloLens and Unity network types, please read the documentation. (Warning: the documentaion is in french.)
 * 
 * This class is abstract for the same reasons than the super class.
 * 
 * This class describe how the device can Connect, Disconnect, Send or Receive a message from to the server.
 * 
 * The ManualResetEvent class is very useful to stop an background method when it takes to much time.
 * How does it work ?
 *      First step: Create a new ManualResetEvent object with a default value "false" before you want.
 *      Second step: Start the background method.
 *      Third step: Make the background method use the "Set" method on the ManualResetEvent created before at the end.
 *      Fourth step: Use the "WaitOne(int)" method on the MnaulResetEvent created before to stop the program until the background method do the Set instruction or the time in millisecond is done. (if you do not set parameter to the WaitOne method, the program can wait indefinitely.)
 */

public abstract class UDPNetworkingUnity : UDPNetworkingBase
{
    private static ManualResetEvent connectDone;
    private static ManualResetEvent sendDone;
    private static ManualResetEvent receiveDone;

    private static Socket externalSocket;           // This object represent the connection.
    private static IPAddress externalIpAddress;     // These two objects represent the IP and the port adapted to the used library.
    private static IPEndPoint externalRemoteEP;

    private class StateObject    // State object for receiving data from remote device. 
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    private static StateObject state;

    
    new protected void Start()
    {
        base.Start();
        externalIpAddress = IPAddress.Parse(externalIP);
        externalRemoteEP = new IPEndPoint(externalIpAddress, externalPort);
    }

    new protected void Update()
    {
        base.Update();
        Socket_MessageReceived();       // Permanent listening for a message from the server
    }

    public new static void Connection()
    {
        if(!isConnected)
        {
            connectDone = new ManualResetEvent(false);      // First step: Create a new ManualResetEvent object with a default value "false" before you want.
            externalSocket = new Socket(externalIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            externalSocket.BeginConnect(externalRemoteEP, new AsyncCallback(ConnectCallback), externalSocket);      // Second step: Start the background method.
            isConnected = connectDone.WaitOne(1000);    // Use the "WaitOne(int)" method on the MnaulResetEvent created before to stop the program until the background method do the Set instruction or the time in millisecond is done.
            if (!isConnected)
            {
                log += "Connexion impossible" + "\n";
                return;
            }

            // Initialize objects to start listening message from the server
            receiveDone = new ManualResetEvent(false);
            state = new StateObject();
            state.workSocket = externalSocket;
            externalSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            Socket_SendMessage("test connection");      // Send a first message to the server.
        }
    }

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the externalSocket from the state object.  
            Socket client = (Socket)ar.AsyncState;
            log += "connection in progress..." + "\n";
            // Complete the connection.  
            client.EndConnect(ar);
            log += "Socket connected to " + client.RemoteEndPoint.ToString() + "\n";
            
            connectDone.Set();      // Third step: Make the background method use the "Set" method on the ManualResetEvent created before at the end.
        }
        catch (Exception e)
        {
            log += "/!\\ ERREUR2 /!\\" + "\n";
            log += e.ToString() + "\n";
        }
    }

    public new static void Disconnection()
    {
        if(isConnected)
        {
            isConnected = false;
            externalSocket.Shutdown(SocketShutdown.Both);
            externalSocket.Close();
            log += "disconnected" + "\n";
        }
    }

    public new static void Socket_SendMessage(string message)
    {
        if (!isConnected)
        {
            log += "La connexion n'a pas été établie. Envoie de message impossible." + "\n";
            return;
        }

        sendDone = new ManualResetEvent(false);

        message += "\n";    // Cariage return to simulate the "WriteLineAsync" method in UDPNetworkHoloLens

        messageSent = message;

        log += "encoding and sending message: " + message + "\n";

        byte[] msg = Encoding.UTF8.GetBytes(message);
        externalSocket.BeginSend(msg, 0, msg.Length, 0, new AsyncCallback(SendCallback), externalSocket);

        if (!sendDone.WaitOne(1000))
        {
            log += "Envoie du message impossible" + "\n";
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the externalSocket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            log += "Sent " + bytesSent + " bytes to server." + "\n";

            // Signal that all bytes have been sent.  
            sendDone.Set();
        }
        catch (Exception e)
        {
            log += "/!\\ ERREUR5 /!\\" + "\n";
            log += e.ToString() + "\n";
        }
    }

    public new static void Socket_MessageReceived()
    {
        if (isConnected)
        {
            if (receiveDone.WaitOne(100))
            {
                receiveDone = new ManualResetEvent(false);
                state = new StateObject();
                state.workSocket = externalSocket;
                externalSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            log += "Receive " + bytesRead.ToString() + " bytes from server." + "\n";

            messageReceived = (Encoding.UTF8.GetString(state.buffer, 0, bytesRead)).ToString();       
            
            receiveDone.Set();
        }
        catch (Exception e)
        {
            log += e.ToString();
        }
    }
}
#endif