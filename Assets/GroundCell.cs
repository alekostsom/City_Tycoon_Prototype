using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundCell : MonoBehaviour {
	
	// reference to the GlobalManager singleton class
	GlobalManager gm_Instance;

	// The original color of the ground cube's material
	Color32 originalColor;
	
	// The x,y coordinates of the grid cell. They will get their value at the time of their instantiation
	int x_coord,y_coord;
	public int X_coord
	{
		get {return x_coord;}
		set {x_coord = value;}
	}
	public int Y_coord
	{
		get {return y_coord;}
		set {y_coord = value;}
	}
	
	
	// Temp object reference
	GameObject objToBuild;
	// The Building object that is built on this spesific grid cell
	GameObject building;
	// The built building's type name
	string buildingTypeString;
	// The income amount to be generated every time unit
	int incomeAmountPerSec;
	// The building type's cost
	int buildingTypeCost;
	// The 'UnderConstruction' UI element
	GameObject underConstruction;
	// The 'Generated Income' UI element
	GameObject incomeUI;
	public GameObject IncomeUI
	{
		get {return incomeUI;}
	}
	
	public bool built = false;
	
	void Start () {
		// Get the reference to the GlobalManager singleton class
		gm_Instance = GlobalManager.gm_Instance;
		
		//Get the color from the 'Top' child object
		originalColor = GetComponentsInChildren<Renderer>()[1].material.color; 
		
		// 
		if (PlayerPrefs.HasKey("CreationTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString()) ){
			if (PlayerPrefs.HasKey("CompletionTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString())){	
				built = true;
				GlobalManager.BuildingType type = (GlobalManager.BuildingType) System.Enum.Parse( typeof( GlobalManager.BuildingType ), PlayerPrefs.GetString("BuildingType_" + x_coord.ToString() + "_" + y_coord.ToString()) );
				InstantiateBuilding(type);
				// Generate and Calculate building's income
				// First calculate the already generated income
				int completionTimestamp = PlayerPrefs.GetInt("CompletionTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString());
				int now = (int)(System.DateTime.UtcNow - gm_Instance.ReferenceDate).TotalSeconds;
				PlayerPrefs.SetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString(), (now - completionTimestamp) * incomeAmountPerSec); 
				// Generate and calculate the rest in real time
				StartCoroutine(CalculateIncome());
			}
			else{
				
				GlobalManager.BuildingType type = (GlobalManager.BuildingType) System.Enum.Parse( typeof( GlobalManager.BuildingType ), PlayerPrefs.GetString("BuildingType_" + x_coord.ToString() + "_" + y_coord.ToString()) );
				InstantiateBuilding(type);
				InstantiateUI();
				
				// Start a coroutine to complete the construction
				StartCoroutine(CompleteConstruction());
				// Start a recursive coroutine to update the under construction ui element
				StartCoroutine(UpdateConstructionUI());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		// Apply actions only when player has selected a building type
		if (gm_Instance.SelectedBType != GlobalManager.BuildingType.None){
			if (!building){
				StartBuilding(gm_Instance.SelectedBType);
				
				// Set the selected building to none
				gm_Instance.SelectedBType = GlobalManager.BuildingType.None;
				
				// Change the status message
				gm_Instance.statusText.text = "Select a building type";
			}
		}

	}
	
	// Create a mouse hover effect when the mouse is over a buildable area
	void OnMouseEnter() {
		// Apply actions only when player has selected a building type
		if (gm_Instance.SelectedBType != GlobalManager.BuildingType.None){
			// Enlight the surface of the area
			if (!built){
				GetComponentsInChildren<Renderer>()[1].material.color = Color.white;
			}
		}
    }	
    void OnMouseExit() {
		// Set back the original color
		if (!built){
			GetComponentsInChildren<Renderer>()[1].material.color = originalColor;
		}
    }
	
	// Function that handles a building construction initialisation
	void StartBuilding(GlobalManager.BuildingType bt){
		InstantiateBuilding(bt);
		InstantiateUI();
		
		// Store the construction starting timestamp in seconds
		PlayerPrefs.SetInt("CreationTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString(), (int)(System.DateTime.UtcNow - gm_Instance.ReferenceDate).TotalSeconds);
		
		// Store the building's type
		PlayerPrefs.SetString("BuildingType_" + x_coord.ToString() + "_" + y_coord.ToString(), bt.ToString());
		
		// Charge the player 
		PlayerPrefs.SetInt("Balance", PlayerPrefs.GetInt("Balance") - buildingTypeCost);

		// Start constructing
		StartCoroutine(CompleteConstruction());
		StartCoroutine(UpdateConstructionUI());
	}
	
	void InstantiateBuilding(GlobalManager.BuildingType bt){
		// Set the selected 3D object to instantiate
		switch (bt)
		{
			case GlobalManager.BuildingType.Apartment:
				objToBuild = gm_Instance.apartmentBuildingObj;
				incomeAmountPerSec = 1;
				buildingTypeString = "Apartment";
				buildingTypeCost = 20;
				break;
			case GlobalManager.BuildingType.LuxApartment:
				objToBuild = gm_Instance.luxAptBuildingObj;
				incomeAmountPerSec = 4;
				buildingTypeString = "Luxury Apartment";
				buildingTypeCost = 80;
				break;
			case GlobalManager.BuildingType.Hotel:
				objToBuild = gm_Instance.hotelBuildingObj;
				incomeAmountPerSec = 10;
				buildingTypeString = "Hotel";
				buildingTypeCost = 250;
				break;
		}
		
		if (objToBuild != null){
			// Instantiate the selected building object
			building = GameObject.Instantiate(objToBuild, transform, false) as GameObject;
			// Translate the 3d building 0.5 meters up.
			building.transform.localPosition = Vector3.up * 0.5f;
		}
	}
	
	void InstantiateUI(){
		//Instantiate the 'Under Construction' UI element and place it according to the current grid cell positin
		underConstruction = GameObject.Instantiate(gm_Instance.underConstructionUI, gm_Instance.canvasObj.transform, false) as GameObject;
		//convert grid cell object position in 3D Space to its correspondand sceen position. 
		Vector2 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		// Place the UI element at the exact screen position of the 3D object. In order to work, the UI element's anchor must be set at the bottom left corner of the screen. 
		// Translate the UI element 70 pixels up.
		underConstruction.GetComponent<RectTransform>().anchoredPosition = screenPoint + Vector2.up * 70;		
	}
	
	public void InstantiateIncomeUI(){
		//Instantiate the 'Generated Income' UI element and place it according to the current grid cell positin
		incomeUI = GameObject.Instantiate(gm_Instance.generatedIncomeUI, gm_Instance.canvasObj.transform, false) as GameObject;
		//convert grid cell object position in 3D Space to its correspondand sceen position. 
		Vector2 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		// Place the UI element at the exact screen position of the 3D object. In order to work, the UI element's anchor must be set at the bottom left corner of the screen. 
		// Translate the UI element 70 pixels up.
		incomeUI.GetComponent<RectTransform>().anchoredPosition = screenPoint + Vector2.up * 70;	
		
		// Update the text values;
		incomeUI.GetComponentsInChildren<Text>()[0].text = buildingTypeString;
		incomeUI.GetComponentsInChildren<Text>()[1].text = "Income: " + PlayerPrefs.GetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString());
		StartCoroutine(UpdateUIIncome());
	}
	
	// A coroutine that will update the UI in the construction phase
	IEnumerator UpdateConstructionUI(){
		int creationTimestamp = PlayerPrefs.GetInt("CreationTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString());
		int now = (int)(System.DateTime.UtcNow - gm_Instance.ReferenceDate).TotalSeconds;
		if (underConstruction)
		{
			underConstruction.GetComponent<UnderConstruction>().percentText.text = ((now - creationTimestamp)*100/60).ToString() + "%";
			underConstruction.GetComponent<UnderConstruction>().percentImage.sizeDelta = new Vector2(((now - creationTimestamp)*125/60), 36);// 125 is the maximum size of the foreground slider
			yield return new WaitForSecondsRealtime(1f);
			StartCoroutine(UpdateConstructionUI());
		}
	}
	
	// A coroutine that will complete the construction of a building
	IEnumerator CompleteConstruction(){
		int creationTimestamp = PlayerPrefs.GetInt("CreationTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString());
		int now = (int)(System.DateTime.UtcNow - gm_Instance.ReferenceDate).TotalSeconds;
		
		yield return new WaitForSecondsRealtime(/*timeToConstruct*/ 60 - (now - creationTimestamp));
		int completionTimestamp = creationTimestamp + 60;
		
		// Set the construction phase as 'built'
		built = true;
		
		// Store the construction completion timestamp in seconds
		PlayerPrefs.SetInt("CompletionTimestamp_" + x_coord.ToString() + "_" + y_coord.ToString(), creationTimestamp + 60);
		
		// Start generating income
		// First calculate the already generated income
		now = (int)(System.DateTime.UtcNow - gm_Instance.ReferenceDate).TotalSeconds;
		PlayerPrefs.SetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString(), (now - completionTimestamp) * incomeAmountPerSec);
		// Now start updating every second
		StartCoroutine(CalculateIncome());
		
		StopCoroutine(UpdateConstructionUI());
		Destroy (underConstruction);		
	}
	
	// A coroutine that calculates and updates income every second
	IEnumerator CalculateIncome(){	
		//Wait for a second to pass, then add the income
		yield return new WaitForSecondsRealtime(1f);
		
		// Add the building type's income amount
		PlayerPrefs.SetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString(), PlayerPrefs.GetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString()) + incomeAmountPerSec);
		
		/*
		// Instantiate income effect
		GameObject incomeEffect = GameObject.Instantiate(gm_Instance.incomeEffectUI, gm_Instance.canvasObj.transform, false) as GameObject;

		Vector2 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		// Place the UI element at the exact screen position of the 3D object. In order to work, the UI element's anchor must be set at the bottom left corner of the screen. 
		incomeEffect.GetComponent<RectTransform>().anchoredPosition = screenPoint + Vector2.up * 70;
		
		incomeEffect.GetComponentInChildren<Text>().text = "+" + incomeAmountPerSec;
		*/
		
		// Restart the coroutine
		StartCoroutine(CalculateIncome());
	}
	
	// A coroutine that updates the income text value of the UI object 
	public IEnumerator UpdateUIIncome(){	
		yield return new WaitForSecondsRealtime(1f);
		if (incomeUI){
			incomeUI.GetComponentsInChildren<Text>()[1].text = "Income: " + PlayerPrefs.GetInt("Income_" + x_coord.ToString() + "_" + y_coord.ToString());
		}
		StartCoroutine(UpdateUIIncome());
	}
}
