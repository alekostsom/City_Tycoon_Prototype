using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	// Script to handle the general funcionality of the game (i.e. menus, scene changing)
	
	/*// Number of horizontal and vertical blocks
	int num_horizontalBlocks, num_verticalBlocks;
	// Reference to the dropdown UI elements that handle the above values
	public Dropdown dd_horizontalBlock, dd_vertivalBlock;
	*/
	//Reference to the continue game button
	public Button btn_Continue;
	
	void Start () {
		// Keep this object active during gameplay
		//DontDestroyOnLoad(gameObject);
		
		// Get the values from the horizontal and the vertical input dropdown UI elements
		//GetHorizontalBlocksNumber();
		//GetVerticalBlocksNumber();
		
		// Check if there is a game session to continue playing
		if (PlayerPrefs.GetInt("GameSessions") != 0){
			btn_Continue.interactable = true;
		}
	}
	
	// Load the demo game level
	public void LoadDemoScene(){
		SceneManager.LoadScene ("demo_scene");
	}
	
	// Create a new game session
	public void CreateNewGame(){
		// Delete all previous progress in the game
		PlayerPrefs.DeleteAll();
		
		// Save the values from the dropdown UI elements as the game's grid size
		//PlayerPrefs.SetInt("HorizontalGridSize", num_horizontalBlocks);
		//PlayerPrefs.SetInt("VerticalGridSize", num_verticalBlocks);
		
		// Initialise player's balance, so he can buy and start constructing his first building
		PlayerPrefs.SetInt("Balance", 20);
		
		// Store a game session
		PlayerPrefs.SetInt("GameSessions", 1);
		
		// Load the scene
		LoadDemoScene();
	}
	
	/*
	// Get the value from the horizontal input dropdown
	public void GetHorizontalBlocksNumber(){
		num_horizontalBlocks = int.Parse(dd_horizontalBlock.options[dd_horizontalBlock.value].text);
		Debug.Log("Horizontal: " + num_horizontalBlocks);
	}
	
	// Get the value from the vertical input dropdown
	public void GetVerticalBlocksNumber(){
		num_verticalBlocks = int.Parse(dd_vertivalBlock.options[dd_vertivalBlock.value].text);
		Debug.Log("Vertical: " + num_verticalBlocks);
	}
	*/
}
