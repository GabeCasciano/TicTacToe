using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_controller : MonoBehaviour
{
    public GameObject[] squares = new GameObject[9];
    public Button resetButton;
    public Text display_text;
    private int[] grid = new int[9];
    private int[] prev_grid = new int[9];
    public int activePlayer = 1;
    public int counter = 0;
    public int winner = 0;

    // Start is called before the first frame update
    void Start()
    {   
        resetButton.GetComponent<Button>().onClick.AddListener(ResetScene);
        display_text.enabled = false;
        foreach (GameObject square in squares){
            square.GetComponent<click_square>().setActivePlayer(activePlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {  
        if(winner == 0 && !checkForDraw()){
            if(!prev_grid.SequenceEqual(grid)){
                changeActivePlayer();
                grid.CopyTo(prev_grid, 0);
            }
            updateGrid();
            winner = checkForWin();
        }
        else{
            Debug.Log("Winner is: " + winner);
            if(winner > 0){
                display_text.text = "Player " + winner + " won";
            }
            else{
                display_text.text = "Draw";
            }
            display_text.enabled = true;
        }
    }


    /*
        Possible win conditions:
        1. row
        2. col
        3. cross
    */
    bool checkForDraw(){
        return counter == 9;
    }

    int checkForWin(){
        //check row
        for(int row = 0; row < 9; row+=3){
            if(grid[row] == grid[row + 1] && grid[row + 1] == grid[row + 2]){
                return grid[row];
            }
        }
        //check col
        for(int col = 0; col < 3; col++){
            if(grid[col] == grid[col+3] && grid[col + 3] == grid[col + 6]){
                return grid[col];
            }
        }
        //check cross
        if(grid[0] == grid[4] && grid[4] == grid[8]){
            return grid[4];
        }
        if(grid[2] == grid[4] && grid[4] == grid[6]){
            return grid[4];
        }
        return 0;
    
    }

    void updateGrid(){
        for(int i = 0; i < 9; i++){
            click_square current_square = (click_square)squares[i].GetComponent<click_square>();
            if(current_square.isClicked()){
                grid[i] = current_square.getPlayerClicked();
            }
        }
    }

    void changeActivePlayer(){
        if(activePlayer == 1){
            activePlayer = 2;
        }
        else{
            activePlayer = 1;
        }

        foreach (GameObject square in squares){
            square.GetComponent<click_square>().setActivePlayer(activePlayer);
        }
        counter++;
        
    }

    void ResetScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
