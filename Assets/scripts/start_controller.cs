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
    public Button connect_button;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        connect_button.onClick.AddListener(connectToServer);
    }
    void connectToServer(){
        client =  new TCPClient(hostAddr, hostPort, 1024);
        client.sendToServer(username_text.text);
        string msg = client.recieveFromServer();
        if(msg.CompareTo("ack") == 0){
            connect_button.enabled = false;
            display_text.text = "Waiting for other player";
            username_text.enabled = false;
            while(!client.serverUpdate()){}
            readyToStart();
        }
        else{
            client.disconnectFromServer();
        }
    }

    void readyToStart(){
        SceneManager.LoadScene("GameScene");
        game_controller controller = GameObject.Find("GameController").GetComponent<game_controller>();

    }
}
