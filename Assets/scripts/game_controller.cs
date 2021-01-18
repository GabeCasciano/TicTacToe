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

    // Start is called before the first frame update
    void Start()
    {   
        display_text.enabled = false;
        game_grid.resetGrid(1);
    }

    // Update is called once per frame
    void Update()
    {  
        game_grid.updateGrid();
        if(!game_grid.hasChanged()){
            game_grid.updateActivePlayer();
            winner = game_grid.checkForWin();
            if(winner > 0){
                display_text.text = "Player " + winner +" Won!";
                display_text.enabled = true;
            }
            else if(game_grid.checkForDraw()){
                display_text.text = "Draw game";
                display_text.enabled = true;
            }
            
        }
    }
}
