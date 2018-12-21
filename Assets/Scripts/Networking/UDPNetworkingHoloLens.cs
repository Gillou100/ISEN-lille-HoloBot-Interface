#if !UNITY_EDITOR
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Windows.Networking.Sockets;
using Windows.Networking;


/*
 * Here is the HoloLens network class.
 * 
 * To know why we have both HoloLens and Unity network types, please read the documentation. (Warning: the documentaion is in french.)
 * 
 * This class is abstract for the same reasons than the super class.
 * 
 * This class describe how the device can Connect, Disconnect, Send or Receive a message from to the server.
 */

public class UDPNetworkingHoloLens : UDPNetworkingBase
{
    private static StreamSocket streamSocket;       // This object represent the connection.
    private static HostName hostName;               // These two objects represent the IP and the port adapted to the used library.
    private static string externalPortS;
    private static CancellationTokenSource cts;     // These object will stop an instruction if it won't stop after some time.
    
    private static Stream outputStream;             // These objects are required to send message with the used library.
    private static StreamWriter streamWriter;
    
    private static Stream inputStream;              // These objects are required to receive message with the used library.
    private static StreamReader streamReader;
    

    new protected void Start()
    {
        base.Start();
        
        hostName = new HostName(externalIP);
        externalPortS = externalPort.ToString();
	}

    new protected void Update()
    {// Only to be able to use the UDPNetWorkingBase.Update in all subclass.
        base.Update();
    }

    public new static async void Connection()
    {
        if(!isConnected)
        {
            streamSocket = new StreamSocket();      // Create the object for the connection.
            log += "connection attempt" + "\n";     // This message will appear in the log field (defined in UDPNetworkingBase) after the next Update.
        
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(1));       // Define the time before stop the instruction.
            try
            {
                await streamSocket.ConnectAsync(hostName, externalPortS).AsTask(cts.Token);     // Try to connect to the server. Will make an error if it takes to much time.
            }
            catch (TaskCanceledException)       // If the connection didn't succed in the time...
            {
                log += "connexion impossible" + "\n";
                return;
            }

            isConnected = true;
            log += "connected" + "\n";

            Socket_SendMessage("test connection");      // Send a first message to the server.
        }
    }

    public new static void Disconnection()
    {
        if(isConnected)
        {
            isConnected = false;
            
            streamSocket.Dispose();
            log += "disconnected" + "\n";
        }
    }
    
    public new static void Socket_SendMessage(string message)
    {
        if(!isConnected)
        {
            log += "The connction is not etablished. Sending message impossible." + "\n";
            return;
        }
        
        messageSent = message;
        log += "Sending message: " + message + "\n";

        outputStream = streamSocket.OutputStream.AsStreamForWrite();    // Create the objects to send the message
        streamWriter = new StreamWriter(outputStream);

        streamWriter.WriteLine(message);        // Send the message
        streamWriter.Flush();                   // Clear the buffer

        Socket_MessageReceived();   // Wait for the echo message from the server
    }
    
    /*
     * We would like to call this method not in the sending method but in the update method. However, we haven't succeeded to make it work.
     */
    public new static void Socket_MessageReceived()
    {
        if(isConnected)
        {
            Stream inputStream = streamSocket.InputStream.AsStreamForRead();    // Create the objects to receive the message
            StreamReader streamReader = new StreamReader(inputStream);
            messageReceived = streamReader.ReadLine();                          // Read the message
        }
    }

    /*
     * The goal is to automatically disconnect the application from the server when the application is stop.
     * But we don't have succeeded to do it, but we have succeeded to connect and disconnect each time the application go on the foreground or background
     */
    private void OnApplicationFocus(bool boolean)
    {
        if(boolean)
        {
            Connection();
            return;
        }
        Disconnection();
    }
}
#endif