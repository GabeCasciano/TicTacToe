using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_square : MonoBehaviour
{

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

    public void forceClick(int player)
    {
        clickedPlayer = player;
        setSquare();
    }

    public bool doClick(int player){
        if(!isClicked() && playerEnabled){
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
