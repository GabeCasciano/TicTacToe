using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    public GameObject[] squares = new GameObject[9];
    private int[] grid = new int[9];
    private int[] prev_grid = new int[9];
    private int counter = 0;
    private int activePlayer = 0;
    
    public void resetGrid(int active){
        grid = new int[9];
        prev_grid = new int[9];
        counter = 0;
        setActivePlayer(active);
        setEnabledPlayer(true);
    }

    public int checkForWin(){
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
        counter++;
        return 0;
    }

    public bool checkForDraw(){
        return counter == 9 && checkForWin() == 0;
    }

    public int getLastChange(){
        for(int i = 0; i < 9; i++){
            if(prev_grid[i] != grid[i]){
                return i;
            }
        }
        return -1;
    }

    public bool hasChanged(){
        return !prev_grid.SequenceEqual(grid);
    }

    public void updatePrev()
    {
        grid.CopyTo(prev_grid, 0);
    }

    public void updateGrid(){
        for(int i = 0; i < 9; i++){
            click_square current_square = (click_square)squares[i].GetComponent<click_square>();
            if(current_square.isClicked()){
                grid[i] = current_square.getPlayerClicked();
            }
        }
        //counter++;
    }

    public bool setNetClick(int square, int player){
        if(square < 9){
            if(!squares[square].GetComponent<click_square>().isClicked()){
                squares[square].GetComponent<click_square>().forceClick(player);
                return true;
            }
        }
        return false;
    }

    public void setEnabledPlayer(bool en){
        foreach(GameObject square in squares){
            square.GetComponent<click_square>().setEnabled(en);
        }
    }
    public void setActivePlayer(int player){
        activePlayer = player;
        foreach (GameObject square in squares){
            square.GetComponent<click_square>().setActivePlayer(activePlayer);
        }
    }

    public int updateActivePlayer(){
        if(activePlayer == 1){
            setActivePlayer(2);
        }
        else if(activePlayer == 2){
            setActivePlayer(1);
        }
        return activePlayer;
    }

}