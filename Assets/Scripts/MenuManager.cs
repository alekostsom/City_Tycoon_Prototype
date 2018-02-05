using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	// Script to handle the general funcionality of the menu (i.e. menus, scene changing)
	
	//Reference to the continue game button
	public Button btn_Continue;
	
	void Start () {		
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
		
		// Initialise player's balance, so he can buy and start constructing his first buildings
		PlayerPrefs.SetInt("Balance", 60);
		
		// Store a game session
		PlayerPrefs.SetInt("GameSessions", 1);
		
		// Load the scene
		LoadDemoScene();
	}
}
