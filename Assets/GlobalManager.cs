using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour {
	
	// Class that holds global settings configuration
	
	// Singleton object
	public static GlobalManager gm_Instance;
	
	// Reference to the Canvas object
	public Canvas canvasObj;
	
	// The exact reference date, to calculate timestamps
	System.DateTime referenceDate;
	public System.DateTime ReferenceDate
	{
		get {return referenceDate;}
	}
	
	// An enum that consists of the building types
	public enum BuildingType {None, Apartment, LuxApartment, Hotel};
	// The selected building type
	BuildingType selectedBType;
	public BuildingType SelectedBType
	{
		get {return selectedBType;}
		set {selectedBType = value;}
	}
	
	// Buildings prefabs
	public GameObject apartmentBuildingObj;
	public GameObject luxAptBuildingObj;
	public GameObject hotelBuildingObj;
	
	// Under Construction UI prefab
	public GameObject underConstructionUI;
	// Generated Income UI prefab
	public GameObject generatedIncomeUI;
	
	// Editor exposed variable to get reference of the status text object
	public Text statusText;
	
	/**** 
		Grid related variables
	****/	
	// Number of ground celss on each line
	public int cellsPerLine = 4;
	// Number of ground celss on each row
	public int cellsPerRow = 4; 
	// An array of the grid's ground celss to manage their functions
	GroundCell[,] groundCells;
	// The grid parent object in hierarchy
	public GameObject gridParent;
	//The base prefab of each grid cell
	public GroundCell groundCellPrefab;
	
	// Player's balance
	int balance;
	int totalBalance;
	
	public Text balanceText;
	
	// Use this for initialization
	void Awake () {
		// Create the singleton instance
		gm_Instance = this;
		
		// Set the 2nd of Febryary 2018 as the exact ref date, to calculate timestamps. (Keeps the variables' values small)
		referenceDate = new System.DateTime(2018, 2, 1, 0, 0, 0, System.DateTimeKind.Utc);
		
		// A usage example of the reference date. Get current timestamps in seconds
		//int cur_time = (int)(System.DateTime.UtcNow - referenceDate).TotalSeconds;
		
		// Initialise the array of ground cells and create the grid
		groundCells = new GroundCell[cellsPerRow, cellsPerLine];
		for (int i=0; i<cellsPerRow; i++) 
		{
			for (int j=0; j<cellsPerLine; j++)
			{
				// Intstantiate the grid cells
				groundCells[i,j] = Instantiate(groundCellPrefab,gridParent.transform, false) as GroundCell;
				// Calculate the position in the grid like a visual representaion of a 2D array. (0,0,0) is the central point.
				groundCells[i,j].transform.localPosition = new Vector3(-cellsPerLine/2 + j, 0, cellsPerRow/2 - i); 
				
				// Set the x,y coordinates
				groundCells[i,j].X_coord = i;
				groundCells[i,j].Y_coord = j;
				
				// Rename ground cell object, to indicate its coordinates in the editor (not really important)
				groundCells[i,j].name += "_" + i.ToString() + "_" + j.ToString();			
			}
		}
		
		StartCoroutine(CalculateBalance());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// 
	public void SelectBuilding(BuildingType bt){
		selectedBType = bt;
		string article = "a";
		if (selectedBType == BuildingType.Apartment){ article = "an";}
		statusText.text = "Click on a land area to build " + article + " " + selectedBType;
	}
	
	public void NewGame()
	{
		PlayerPrefs.DeleteAll();
	}
	
	// Calculate total balance, with income and expenses
	IEnumerator CalculateBalance(){
		totalBalance = balance;
		foreach (GroundCell gc in groundCells){
			totalBalance += PlayerPrefs.GetInt("Income_" + gc.X_coord.ToString() + "_" + gc.Y_coord.ToString());
		}
		balanceText.text = "Coins: " + totalBalance;
		yield return new WaitForSecondsRealtime(1f);
		StartCoroutine(CalculateBalance());
	}
}
