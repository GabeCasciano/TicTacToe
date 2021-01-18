using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class client : MonoBehaviour
{
    public Button player_connect_button;
    public Button player_action_button;
    public Text player_username;
    public Text player_display_text;

    private TCPClient netClient;
    public int port;
    public string hostName;
    private bool connected = false;
    private bool players_turn = false;

    // Start is called before the first frame update
    void Start()
    {
        player_connect_button.onClick.AddListener(connectToServer);
        player_action_button.onClick.AddListener(playerAction);
    }

    void connectToServer(){
        string username = player_username.text;
        if(username.Length > 0){
            netClient = new TCPClient(hostName, port, 1024);
            netClient.sendToServer(username);
            string temp = netClient.recieveFromServer();
            if(string.Compare(temp, "ack") == 0){
                player_display_text.text = "connected";
                Debug.Log("connected to server");
                connected = true;
            }
            else{
                player_display_text.text = "error connecting";
                Debug.Log("error connecting to server");
            }
        }
        else{
            player_display_text.text = "enter a username";
            Debug.Log("no username entered");
        }

    }

    void playerAction(){
        if(connected){
            if(players_turn){
                player_display_text.text = "Player Action";
                netClient.sendToServer("action");
            }
        }
        Debug.Log("player Action");
    }

    // Update is called once per frame
    void Update()
    {
        if(connected){
            if(netClient.serverUpdate()){
                players_turn = string.Compare(netClient.recieveFromServer(), "yes") == 0;
            }
            Debug.Log((players_turn)?"turn":"not turn");
        }
    }
}
