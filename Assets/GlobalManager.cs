using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour {
	
	// Class that holds global settings configuration
	
	// Singleton object
	public static GlobalManager gm_Instance;
	
	// The exact reference date, to calculate timestamps
	System.DateTime referenceDate;
	public System.DateTime ReferenceDate
	{
		get {return referenceDate;}
		set {referenceDate = value;}
	}
	
	public enum BuildingType {None, Apartment, LuxApartment, Hotel};
	// The selected building type
	BuildingType selectedBType;
	public BuildingType SelectedBType
	{
		get {return selectedBType;}
		set {selectedBType = value;}
	}
	
	//
	public GameObject apartmentBuildingObj;
	public GameObject luxAptBuildingObj;
	public GameObject hotelBuildingObj;
	
	// Editor exposed variable to get reference of the status text object
	public Text statusText;
	
	// Use this for initialization
	void Awake () {
		// Create the singleton instance
		gm_Instance = this;
		
		// Set the 2nd of Febryary 2018 as the exact ref date, to calculate timestamps. (Keeps the variables' values small)
		referenceDate = new System.DateTime(2018, 2, 2, 0, 0, 0, System.DateTimeKind.Utc);
		
		// A usage example of the reference date. Get current timestamps in seconds
		int cur_time = (int)(System.DateTime.UtcNow - referenceDate).TotalSeconds;
		Debug.Log(cur_time);
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
}
