using System;
using System.Net.Sockets;
using System.Text;

public class TCPClient{
    private int dataLength;
    private int portNumber;
    private string hostname;
    private TcpClient client;
    private NetworkStream netStream;

    public TCPClient(string hostname, int port, int maxDataLength){
        this.portNumber = port;
        this.hostname = hostname;
        this.dataLength = maxDataLength;

        client = new TcpClient(this.hostname, this.portNumber);
        netStream = client.GetStream();
    }

    public void sendToServer(string data){
        byte[] bytes = Encoding.ASCII.GetBytes(data);
        if(bytes.Length < this.dataLength){
            this.netStream.Write(bytes, 0, bytes.Length);
        }
    }

    public string recieveFromServer(){
        byte[] bytes = new byte[this.dataLength];
        this.netStream.Read(bytes, 0, bytes.Length);
        return Encoding.ASCII.GetString(bytes);
    }

    public void disconnectFromServer(){
        this.netStream.Close();
        this.client.Close();
    } 

    public bool serverUpdate(){
        return netStream.DataAvailable;
    }
}   