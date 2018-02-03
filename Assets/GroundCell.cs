using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCell : MonoBehaviour {
	
	// reference to the GlobalManager singleton class
	GlobalManager gm_Instance;

	// The original color of the ground cube's material
	Color32 originalColor;
	
	void Start () {
		// Get the reference to the GlobalManager singleton class
		gm_Instance = GlobalManager.gm_Instance;
		
		//Get the color from the 'Top' child object
		originalColor = GetComponentsInChildren<Renderer>()[1].material.color; 
	}
	
	// Temp object reference
	GameObject objToBuild;
	// The Building object that is built on this spesific grid cell
	GameObject building;
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		Debug.Log("Received click on: " + gameObject.name);
		Debug.Log("Ready to start building: " + gm_Instance.SelectedBType);
		if (!building)
			StartBuilding(gm_Instance.SelectedBType);
		else
			building.GetComponent<Renderer>().materials[1].color = Color.black;
	}
	
	// Create a mouse hover effect when the mouse is over a buildable area
	void OnMouseEnter() {
        // Enlight the surface of the area
		GetComponentsInChildren<Renderer>()[1].material.color = Color.white;
    }	
    void OnMouseExit() {
		// Set back the original color
        GetComponentsInChildren<Renderer>()[1].material.color = originalColor;
    }
	
	// Function that handles a building construction
	void StartBuilding(GlobalManager.BuildingType bt){
		switch (bt)
		{
			case GlobalManager.BuildingType.Apartment:
				objToBuild = gm_Instance.apartmentBuildingObj;
				break;
			case GlobalManager.BuildingType.LuxApartment:
				objToBuild = gm_Instance.luxAptBuildingObj;
				break;
			case GlobalManager.BuildingType.Hotel:
				objToBuild = gm_Instance.hotelBuildingObj;
				break;
		}
		
		if (objToBuild != null){
			building = GameObject.Instantiate(objToBuild) as GameObject;
			building.transform.SetParent (transform);
			building.transform.localPosition = Vector3.up * 0.5f;
			
			//building.GetComponent<Renderer>().materials[0].color = Color.black;
		}
	}
}
