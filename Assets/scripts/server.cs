using System;
using System.Collections;
using System.Text;
using System.Threading;

public class TestServer{

    public static int Main(String[] args){
        int port = 8888;
        TCPServer Server = new TCPServer(port, 2, 1024);
        int playerTurn = 0, currentPlayerMove = 0;
        string username1, username2;
        string recvData;

        int[] grid = new int[9];
        int moveCounter = 0;

        Console.WriteLine("Beginning Server");
        
        Server.acceptClient(); //accept the first player
        username1 = Server.readFromClient(0); //get player1's username
        Server.sendToClient(0, "ack");
        Console.WriteLine("New Player, " + username1 + " has joined");
        
        Server.acceptClient(); //accept the second player
        username2 = Server.readFromClient(1); //get player2's username
        Server.sendToClient(1, "ack");
        Console.WriteLine("New Player, " + username2 + " has joined");

        Thread.Sleep(1000); // stop messages from merging together
        
        // select which player goes first
        playerTurn = new Random().Next(1, 3); // chane to (0,2)
        // tell the clients who they are and their opponents
        Server.sendToClient(0,"ready,1," + username2); // ready,playerNum,username(opposing)
        Server.sendToClient(1,"ready,2,"+username1);
        
        Console.WriteLine("Player " + playerTurn + " will begin");

        Thread.Sleep(1000);
        
        // tell both players the game has begun
        Server.broadcastToClients("move,-1,"+playerTurn+",none"); // command,lastMove,currentTurn,gameState
        
        while(checkForWin(grid) == 0 && moveCounter < 9){
            // wait for player to move
            while(!Server.clientReady(playerTurn - 1)){}
            
            // expecting a single number representing where the player went
            currentPlayerMove = Int32.Parse(Server.readFromClient(playerTurn - 1));
            if(currentPlayerMove < grid.Length){
                grid[currentPlayerMove] = playerTurn;
            }

            // switch player turns
            if(playerTurn == 1){
                playerTurn = 2;
            }
            else{
                playerTurn = 1;
            }

            // update both players on the grid state, and who is next
            // command,lastMove,currentTurn,gameState
            Server.broadcastToClients("move,"+currentPlayerMove+","+playerTurn+","+gameState(grid, moveCounter));
            moveCounter++;
        }
        
        Thread.Sleep(1000);
        // tell both players game is over and who won
        Server.broadcastToClients("gameover,"+gameState(grid, moveCounter));

        return 0;
    }

    public static string gameState(int[] _grid, int counter){
        int winner = checkForWin(_grid);
        if(winner == 1){
            return "winner1";
        }
        else if(winner == 2){
            return "winner2";
        }

        if(counter == 9){
            return "draw";
        }
        return "none";
    }

    public static int checkForWin(int[] _grid){
        //check row
        for(int row = 0; row < 9; row+=3){
            if(_grid[row] == _grid[row + 1] && _grid[row + 1] == _grid[row + 2]){
                return _grid[row];
            }
        }
        //check col
        for(int col = 0; col < 3; col++){
            if(_grid[col] == _grid[col+3] && _grid[col + 3] == _grid[col + 6]){
                return _grid[col];
            }
        }
        //check cross
        if(_grid[0] == _grid[4] && _grid[4] == _grid[8]){
            return _grid[4];
        }
        if(_grid[2] == _grid[4] && _grid[4] == _grid[6]){
            return _grid[4];
        }
        return 0;
    }

}