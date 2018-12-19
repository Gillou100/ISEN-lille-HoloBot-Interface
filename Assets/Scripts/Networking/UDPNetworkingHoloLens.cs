#if !UNITY_EDITOR
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;


public class UDPNetworkingHoloLens : UDPNetworkingBase
{
    static Windows.Networking.Sockets.StreamSocket streamSocket;
    static Windows.Networking.HostName hostName;
    static string externalPortS;

    static Stream outputStream;
    static StreamWriter streamWriter;

    static CancellationTokenSource cts;

    static Stream inputStream;
    static StreamReader streamReader;
    static string msg;

    static ManualResetEvent used;

    // Use this for initialization
    new protected void Start()
    {
        base.Start();
        hostName = new Windows.Networking.HostName(externalIP);
        externalPortS = externalPort.ToString();
	}

    public new static async void Connection()
    {
        if(!isConnected)
        {
            streamSocket = new Windows.Networking.Sockets.StreamSocket();
            log += "connection attempt" + "\n";
        
            cts = new CancellationTokenSource();
            try
            {
                cts.CancelAfter(TimeSpan.FromSeconds(1));
                await streamSocket.ConnectAsync(hostName, externalPortS).AsTask(cts.Token);
            }
            catch (TaskCanceledException)
            {
                Disconnection();
                log += "connexion impossible";
                return;
            }

            isConnected = true;
            log += "connected" + "\n";
            /*inputStream = streamSocket.InputStream.AsStreamForRead();
            streamReader = new StreamReader(inputStream);*/

            Socket_SendMessage("test connection");
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

    //send message
    public new static async void Socket_SendMessage(string message)
    {
        if(!isConnected)
        {
            log += "La connexion n'a pas été établie. Envoie de message impossible." + "\n";
            return;
        }

        //used = new ManualResetEvent(false);
        messageSend = message;
        log += "Sending message: " + message + "\n";
        outputStream = streamSocket.OutputStream.AsStreamForWrite();
        streamWriter = new StreamWriter(outputStream);
        
        try
        {
            await streamWriter.WriteAsync(message);
            await streamWriter.FlushAsync();
        }
        catch (Exception e)
        {
            log += "Envoie du message impossible" + "\n" + e;
        }
        
        //used.Set();
    }

    //recieve message
    new protected async void Update()
    {
        base.Update();
        /*if (isConnected)
        {
            if (streamReader.Peek() > -1)
            {
                log += "1\n";
                /*do
                {
                    messageReceive += ((char)streamReader.Read()).ToString();
                } while (streamReader.Peek() > -1);
                log += "2\n";
                messageReceive = (streamReader.ReadLineAsync()).ToString();
                /*await streamReader.BaseStream.FlushAsync();
                log += "3\n";

                inputStream = streamSocket.InputStream.AsStreamForRead();
                log += "4\n";
                streamReader = new StreamReader(inputStream);
                log += "5\n";
            }
        }*/
    }

    private static async void Socket_MessageReceived()
    {
        Stream inputStream = streamSocket.InputStream.AsStreamForRead();
        StreamReader streamReader = new StreamReader(inputStream);
        messageReceive = await streamReader.ReadLineAsync();
        log += messageReceive;
    }

    private void OnApplicationFocus(bool boolean)
    {
        if(boolean)
        {
            UDPNetworking.Connection();
            return;
        }
        UDPNetworking.Disconnection();
    }
}
#endif