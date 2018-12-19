#if UNITY_EDITOR
using System;
using System.Threading;
using System.Text;

using System.Net;
using System.Net.Sockets;

public abstract class UDPNetworkingUnity : UDPNetworkingBase
{

    // RESEAU
    static ManualResetEvent connectDone;

    // SERVEUR
    static ManualResetEvent sendDone;
    static IPAddress externalIpAddress;
    static IPEndPoint externalRemoteEP;
    static Socket externalSocket;
    
    // CLIENT
    static ManualResetEvent receiveDone;

    public class StateObject    // State object for receiving data from remote device. 
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

    static StateObject state;

    // Use this for initialization
    new protected void Start()
    {
        base.Start();
        externalIpAddress = IPAddress.Parse(externalIP);
        externalRemoteEP = new IPEndPoint(externalIpAddress, externalPort);
    }

    public new static void Connection()
    {
        if(!isConnected)
        {
            connectDone = new ManualResetEvent(false);
            externalSocket = new Socket(externalIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            externalSocket.BeginConnect(externalRemoteEP, new AsyncCallback(ConnectCallback), externalSocket);
            isConnected = connectDone.WaitOne(1000);
            if (!isConnected)
            {
                log += "Connexion impossible" + "\n";
                //Disconnection();
                return;
            }
            
            receiveDone = new ManualResetEvent(false);
            state = new StateObject();
            state.workSocket = externalSocket;
            externalSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            Socket_SendMessage("test connection");
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

            // Signal that the connection has been made.  
            connectDone.Set();
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

            messageReceive = (Encoding.UTF8.GetString(state.buffer, 0, bytesRead)).ToString();       
            
            receiveDone.Set();
        }
        catch (Exception e)
        {
            log += e.ToString();
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

        messageSend = message;
        
        log += "encoding and sending message: " + message + "\n";
        byte[] msg = Encoding.UTF8.GetBytes(message);
        externalSocket.BeginSend(msg, 0, msg.Length, 0, new AsyncCallback(SendCallback), externalSocket);

        if (!sendDone.WaitOne(1000))
        {
            log += "Envoie du message impossible" + "\n";
        }
    }

    new protected void Update()
    {
        base.Update();
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
}
#endif