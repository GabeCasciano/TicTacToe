using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

public class TCPServer{

    private int dataLength;
    private int port;
    private int clientCounter = 0;
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();
    private List<NetworkStream> netStreams = new List<NetworkStream>();

    public TCPServer(int port, int numClients, int maxDataLength){
        this.port = port;
        this.dataLength = maxDataLength;
        this.clientCounter = numClients;
        this.listener = new TcpListener(IPAddress.Any, this.port);
        this.listener.Start();
    }
    public void acceptClient(){
        if(this.clientCounter > 0){
            TcpClient cli = this.listener.AcceptTcpClient();
            this.clients.Add(cli);
            this.netStreams.Add(cli.GetStream());
            this.clientCounter--;
        }
    }

    public void broadcastToClients(string data){
        byte[] bytes = Encoding.ASCII.GetBytes(data);
        if(bytes.Length < this.dataLength){
            foreach(NetworkStream ns in this.netStreams){
                ns.Write(bytes, 0, bytes.Length);
            }
        }
    }

    public string[] readFromClients(){
        string[] data = new string[this.clients.Count];
        byte[] bytes;

        for(int i = 0; i < this.clients.Count; i++){
            bytes = new byte[this.dataLength];
            this.netStreams[i].Read(bytes, 0, bytes.Length);
            data[i] = Encoding.ASCII.GetString(bytes);
        }
        return data;
    }

    public void sendToClient(int client, string data){
        byte[] bytes = Encoding.ASCII.GetBytes(data);
        if(client < this.clients.Count && bytes.Length < this.dataLength){
            this.netStreams[client].Write(bytes, 0, bytes.Length);
        }
    }

    public string readFromClient (int client){
        byte[] bytes = new byte[this.dataLength];
        if(client < this.clients.Count){
            this.netStreams[client].Read(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(bytes);
        }
        return null;
    }

    public void disconnetClient(int client){
        if(client < this.clients.Count){
            this.clients[client].Close();
            this.clients.RemoveAt(client);
            this.netStreams[client].Close();
            this.netStreams.RemoveAt(client);
            this.clientCounter++;
        }
    }

    public void disconnectAll(){
        for(int i = 0; i < this.clients.Count; i++){
            this.clients[i].Close();
            this.netStreams[i].Close();
            this.clientCounter++;
        }
        this.clients = new List<TcpClient>();
        this.netStreams = new List<NetworkStream>();
    }

}