using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class start_controller : MonoBehaviour
{      
    private TCPClient client;
    public string hostAddr = "127.0.0.1";
    public int hostPort = 8888;
    public Text username_text;
    public Text display_text;
    public Button single_player;
    public Button multi_player;
    private int mode = 0;
    private int playerNumber = 0;
    private string otherUser;
    private bool waiting = false;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        single_player.onClick.AddListener(startSinglePlayer);
        multi_player.onClick.AddListener(startMultiPlayer);
    }

    void startSinglePlayer(){
        SceneManager.LoadScene("GameScene");
        mode = 1;
    }

    void startMultiPlayer()
    {
        Debug.Log("Trying to connect");
        client = new TCPClient(hostAddr, hostPort, 1024);
        client.sendToServer(username_text.text);
        string msg = client.recieveFromServer();
        Debug.Log(msg);
        if(msg.CompareTo("ack") == 0)
        {
            Debug.Log("Connected and waiting");
            display_text.text = "Waiting for other player";

            waiting = true;
            multi_player.gameObject.SetActive(false);
            single_player.gameObject.SetActive(false);
            GameObject.Find("username_field").SetActive(false);

        }
        
    }

    private void Update()
    {
        if (waiting)
        {
            Debug.Log("waiting");
            if (client.serverUpdate())
            {
                string msg = client.recieveFromServer();
                string[] data = msg.Split(',');

                if(data[0].CompareTo("ready") == 0){
                    SceneManager.LoadScene("GameScene");
                    playerNumber = Int32.Parse(data[1]);
                    otherUser = data[2];
                    mode = 2;
                    Debug.Log("Starting game");
                }  
            }
            
        }
    }

    public int getMode(){
        return mode;
    }

    public TCPClient getClient(){
        return client;
    }

    public string getOtherUser(){
        return otherUser;
    }

    public int getPlayerNumber(){
        return playerNumber;
    }


    // void startSinglePlayer(){
    //     SceneManager.LoadScene("GameScene");
    //     Debug.Log(SceneManager.GetActiveScene().name);
    //     loadSinglePlayer();
    // }

    // void loadSinglePlayer(){
    //     Debug.Log(SceneManager.GetActiveScene().name);
    //     Scene game = SceneManager.GetSceneByName("GameScene");
        
    //     List<GameObject> objs = new List<GameObject>(game.rootCount);
    //     game.GetRootGameObjects(objs);

    //     foreach(GameObject o in objs){
    //         Debug.Log(o.name);
    //         if(o.name == "Game_controller"){
    //             o.GetComponent<game_controller>().setMode(1);
    //             break;
    //         }
    //     }
    // }

    // void startMultiPlayer(){
    //     client =  new TCPClient(hostAddr, hostPort, 1024);
    //     client.sendToServer(username_text.text);
    //     string msg = client.recieveFromServer();
    //     if(msg.CompareTo("ack") == 0){

    //         display_text.text = "Waiting for other player";
    //         username_text.enabled = false;

    //         while(!client.serverUpdate()){}
    //         msg = client.recieveFromServer();

    //         string[] data = msg.Split(',');

    //         if(data[0].CompareTo("ready") == 0){
    //             SceneManager.LoadScene("GameScene");
    //             game_controller controller = GameObject.Find("Game_Controller").GetComponent<game_controller>();
    //             controller.setMode(2);
    //             controller.initMultiPlayer(this.client, Int32.Parse(data[1]), data[2]);
    //         }
    //     }    
    // }
}
