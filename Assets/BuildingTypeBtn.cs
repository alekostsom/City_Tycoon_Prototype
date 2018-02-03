using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeBtn : MonoBehaviour {
	
	// reference to the GlobalManager singleton class
	GlobalManager gm_Instance;
	
	public GlobalManager.BuildingType buildingType;
	
	void Start () {
		// Get the reference to the GlobalManager singleton class
		gm_Instance = GlobalManager.gm_Instance;
	}
	
	public void SetBuildingType()
	{
		gm_Instance.SelectBuilding(buildingType);
	}
}
