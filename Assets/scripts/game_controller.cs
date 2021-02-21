using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_controller : MonoBehaviour
{

    public Text display_text;
    public Grid game_grid;
    private int winner;
    private int mode = 0;
    private int number = 0;
    private string otherUser;
    private int playerNumber, otherNumber;
    private bool active;
    private TCPClient client;


    // Start is called before the first frame update
    void Start()
    {   
        // Need to properly get this gameobject from the start controller
        start_controller startController = GameObject.FindWithTag("StartController").GetComponent<start_controller>(); 
            //GameObject.Find("DontDestroyOnLoad/Start_Controller").GetComponent<start_controller>();
        display_text.gameObject.SetActive(false);
        game_grid.resetGrid(1);
        mode = startController.getMode();

        if(mode == 2){
            // init get multiplayer stuff
            client = startController.getClient();
            otherUser = startController.getOtherUser();
            playerNumber = startController.getPlayerNumber();
            if (playerNumber == 1)
                otherNumber = 2;
            else
                otherNumber = 1;
        }

        startController.enabled = false;
        Destroy(startController);
        Debug.Log("Stats: num:" + playerNumber + ", other:" + otherUser);
    }
    
    public void doSinglePlayer(){
        if(game_grid.hasChanged()){
            game_grid.updatePrev();
            game_grid.updateActivePlayer();
            winner = game_grid.checkForWin();
            if(winner > 0){
                display_text.gameObject.SetActive(true);
                display_text.text = "Player " + winner +" Won!";
            }
            else if(game_grid.checkForDraw()){
                display_text.gameObject.SetActive(true);
                display_text.text = "Draw game";
            }
        }
        game_grid.updateGrid();
    }

    public void doMultiPlayer(){
        // get information from server
        string msg;
        string[] data;
        int lastSquare;
        
        if(client.serverUpdate()){
            // pasrse information from server
            msg = client.recieveFromServer();
            data = msg.Split(',');
            
            //check if it is a move
            if (data[0].CompareTo("move") == 0)
            {
                int square = Int32.Parse(data[1]);
                int activeP = Int32.Parse(data[2]);
                string gameState = data[3];

                if (gameState.Contains("winner") || gameState.Contains("draw"))
                {
                    display_text.text = gameState;
                    active = false;
                }
                else
                {
                    if (activeP == playerNumber) // my turn
                    {
                        active = true;
                        game_grid.setActivePlayer(playerNumber);
                    
                        if (square == -1) // first move
                        {
                            display_text.text = "";
                        }
                        else if (square >= 0 && square < 9) // not first move
                        {
                            game_grid.setNetClick(square, otherNumber);
                            game_grid.updateGrid();
                            game_grid.updatePrev();
                        }  
                    
                    }
                    else // not my turn
                    {
                        active = false;
                        game_grid.setActivePlayer(otherNumber);
                    }
                    game_grid.setEnabledPlayer(active);
                }
                Debug.Log("Stats: num: " + playerNumber + " Lmove: " + square + " activeP: " + activeP + " state: " + gameState);
            }
        }
        if(active){
            // check if the board has changed or wait for a net click
            if(game_grid.hasChanged()){
                lastSquare = game_grid.getLastChange();
                game_grid.updatePrev();
                client.sendToServer(lastSquare.ToString());
            }
        }
        else
        {
            game_grid.setEnabledPlayer(false);
            game_grid.setActivePlayer(otherNumber);
        }
        game_grid.updateGrid();

        
    }

    public void initMultiPlayer(TCPClient client, int number, string other){
        this.client = client;
        this.number = number;
        this.otherUser = other;
    }


    public void setMode(int mode){
        this.mode = mode;
    }
    
    // Update is called once per frame
    void Update()
    {  
        if(mode == 1){ // single player
            doSinglePlayer();
        }
        else if(mode == 2){ // multi player
            doMultiPlayer();
        }
    }
}
