using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeBtn : MonoBehaviour {
	
	// Script that controls the behaviour of a building type UI button element. Checks if the player affords the type's cost 
	
	// reference to the GlobalManager singleton class
	GlobalManager gm_Instance;
	
	public GlobalManager.BuildingType buildingType;
	public int buildingTypeCost;
	
	void Start () {
		// Get the reference to the GlobalManager singleton class
		gm_Instance = GlobalManager.gm_Instance;
	}
	
	// Exposed in Editor function to receive UI click events
	public void SetBuildingType()
	{
		gm_Instance.SelectBuilding(buildingType);
	}
	
	// Exposed in Editor function to receive UI click events
	public void StartCheckingBalance(){
		StartCoroutine(CheckBalance());
	}
	
	// Coroutine that controls every second (plus 0.05 seconds :P) if the player can afford buying a specific type of building
	IEnumerator CheckBalance(){
		yield return new WaitForSecondsRealtime(0.05f); // Wait to get the updated total balance value
		if (gm_Instance.GetTotalBalance() >= buildingTypeCost){
			GetComponent<Button>().interactable = true;
		}
		else{
			GetComponent<Button>().interactable = false;
		}
		
		yield return new WaitForSecondsRealtime(1.0f);
		StartCoroutine(CheckBalance());
	}
}
