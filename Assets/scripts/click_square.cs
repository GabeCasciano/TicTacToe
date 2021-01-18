using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_square : MonoBehaviour
{

    public int location = 0;
    private int clickedPlayer = 0;
    private int activePlayer = 0;
    private bool playerEnabled = false;
    public GameObject player1;
    public GameObject player2;
    //public game_controller = GameObject.Find("Game_controller").GetComponent<game_controller>()

    public int getPlayerClicked(){
        return clickedPlayer;
    }

    public bool isClicked(){
        return clickedPlayer > 0;
    }
    
    public int getLocation(){
        return location;
    }

    public void setActivePlayer(int player){
        if(player > 0){
            activePlayer = player;
        }
    }

    public int getActivePlayer(){
        return activePlayer;
    }

    public void setEnabled(bool en){
        playerEnabled = en;
    }

    public void setSquare(){
        GameObject new_square;
        if(clickedPlayer == 1){
            new_square = Instantiate(player1, transform);
        }
        else if (clickedPlayer == 2){
            new_square = Instantiate(player2, transform);
        }
    }

    public bool doClick(int player){
        if(!isClicked()){
            clickedPlayer = player;
            setSquare();
            return true;
        }
        Debug.Log("Ty Again");
        return false;
    }

    private void OnMouseDown() {
        if(enabled){
            doClick(activePlayer);
        }
    }

}
