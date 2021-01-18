using System;

public class TestServer{

    public static int Main(String[] args){
        int port = 8888;
        TCPServer Server = new TCPServer(port, 2, 1024);
        bool player_turn = false;
        string username1, username2;

        Server.acceptClient();
        // accept and ack first player
        username1 = Server.readFromClient(0);
        Console.WriteLine("New Player, " + username1 + " has joined");
        Server.sendToClient(0, "ack");
        
        Server.broadcastToClients("ready");
        return 0;
    }

}